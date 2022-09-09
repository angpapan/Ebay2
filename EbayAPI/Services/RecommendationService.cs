using EbayAPI.Data;
using EbayAPI.Dtos;
using EbayAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using NumSharp;

namespace EbayAPI.Services;

public class RecommendationService
{
    private readonly EbayAPIDbContext _dbContext;

    private NDArray UserArray;
    private NDArray ItemArray;
    private List<int> Users;
    private List<int> Items;
    private int Features;
    private double LearningRate;
    private double Sensitivity;
    private Dictionary<int, List<int>> BaseDict;
    private int Steps;
    private int SampleSize;

    public RecommendationService(EbayAPIDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private class ItemLatents
    {
        public int ItemId { get; set; }
        public NDArray Latents { get; set; }
    }
    
    /// <summary>
    /// Initializes the object to factorize it later.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="learningRate">The learning rate</param>
    /// <param name="sensitivity">Minimum error to stop the descent</param>
    /// <param name="features">Number of features</param>
    /// <param name="steps">Maximum steps for the factorization</param>
    public void InitNew(List<UserItem> data, double learningRate = 0.001, double sensitivity = 0.001,
        int features = 25, int steps = 1000)
    {
        
        // Convert List to Dictionary from userIds to viewed/bid items.
        // Justified because of the number of searches during factorization
        BaseDict = new Dictionary<int, List<int>>();
        this.SampleSize = 0;
        foreach (UserItem ui in data)
        {
            if (!BaseDict.ContainsKey(ui.UserId))
            {
                BaseDict[ui.UserId] = new List<int>();
            }

            BaseDict[ui.UserId].Add(ui.ItemId);
            this.SampleSize++;
        }
        
        // get a list of users
        Users = data.Select(i => i.UserId).Distinct().ToList();
        Users.Sort();
        // get a list of items
        Items = data.Select(i => i.ItemId).Distinct().ToList();
        Items.Sort();

        Features = features;
        LearningRate = learningRate;
        Steps = steps;
        Sensitivity = sensitivity;
        
        // initialize arrays for users and items with random values in (-1, 1)
        UserArray = np.random.stardard_normal(new int[] { Users.Count, this.Features }).astype(typeof(float));
        ItemArray = np.random.stardard_normal(new int[] { Items.Count, this.Features }).astype(typeof(float));
    }

    /// <summary>
    /// Factorizes the initial data and saves them to the database.
    /// The InitNew method should be executed before Factorize.
    /// </summary>
    /// <param name="type">It can be "bid" or "view" depending on the given data</param>
    public void Factorize(string type = "bid")
    {
        this.ItemArray = this.ItemArray.transpose();
        double previousError = 10e9;    
        
        for (int step = 0; step < this.Steps; step++)
        {
            for (int i = 0; i < this.Users.Count; i++)
            {
                for (int j = 0; j < this.Items.Count; j++)
                {
                    int userId = this.Users[i];
                    int itemId = this.Items[j];

                    int occurrences = this.CountOccurrences(this.BaseDict[userId], itemId); 
                    if (occurrences > 0)
                    {
                        // get the dot product of i-th user row with j-th item column
                        NDArray userTemp = this.UserArray[$"{i},:"];
                        NDArray itemTemp = this.ItemArray[$":,{j}"];
                        NDArray dotted = userTemp.dot(itemTemp);
                        string str = dotted.ToString();
                        double compValue = double.Parse(str);
                        
                        // The number of the times the user bidded/viewed the item 
                        // is a meter of how much he likes it
                        double eij = occurrences - compValue;
                        
                        // change the matrices values 
                        for (int f = 0; f < this.Features; f++)
                        {
                            this.UserArray[i][f] += this.LearningRate * 2 * eij * this.ItemArray[f][j];
                            this.ItemArray[f][j] += this.LearningRate * 2 * eij * this.UserArray[i][f];
                        }
                    }
                }
            }
            
            // calculate global squared error
            double error = 0;
            for (int i = 0; i < this.Users.Count; i++)
            {
                for (int j = 0; j < this.Items.Count; j++)
                {
                    int userId = this.Users[i];
                    int itemId = this.Items[j];
                    int occurrences = this.CountOccurrences(this.BaseDict[userId], itemId); 
                    if (occurrences > 0)
                    {
                        error += Math.Pow(
                            occurrences - (double)this.UserArray[$"{i}, :"].dot(this.ItemArray[$":, {j}"]),
                            2);
                    }
                }
            }
            
            // get MSRE
            error = Math.Sqrt(error / this.SampleSize);

            // Stop if MSRE did not change
            // Sensitivity is used to avoid possible loss of precision
            // in floating point numbers
            double difference = Math.Abs(error - previousError); 
            Console.WriteLine($"Step: {step}, MSRE: {error}, Difference: {difference}");
            if (difference < Sensitivity)
                break;
            
            previousError = error;
        }
        
        // transpose again to save to db
        this.ItemArray = this.ItemArray.transpose();
        
        // convert latent feature arrays to string to save to db
        if (type == "bid")
        {
            List<UserBidLatent> ubl = new List<UserBidLatent>();
            for (int i = 0; i < this.Users.Count; i++)
            {
                UserBidLatent u = new UserBidLatent();
                u.UserId = this.Users[i];
                u.LatentFeatures = String.Join(";", Array.ConvertAll((float[])UserArray[i], x => x.ToString()));
                ubl.Add(u);
            }

            List<ItemBidLatent> ibl = new List<ItemBidLatent>();
            for (int i = 0; i < this.Items.Count; i++)
            {
                ItemBidLatent it = new ItemBidLatent();
                it.ItemId = this.Items[i];
                it.LatentFeatures = String.Join(";", Array.ConvertAll((float[])ItemArray[i], x => x.ToString()));
                ibl.Add(it);
            }

            _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE UserBidLatents");
            _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE ItemBidLatents");
            _dbContext.SaveChanges();

            _dbContext.UserBidLatents.AddRange(ubl);
            _dbContext.ItemBidLatents.AddRange(ibl);
            _dbContext.SaveChanges();
        }
        else if (type == "view")
        {
            List<UserViewLatent> ubl = new List<UserViewLatent>();
            for (int i = 0; i < this.Users.Count; i++)
            {
                UserViewLatent u = new UserViewLatent();
                u.UserId = this.Users[i];
                u.LatentFeatures = String.Join(";", Array.ConvertAll((float[])UserArray[i], x => x.ToString()));
                ubl.Add(u);
            }

            List<ItemViewLatent> ibl = new List<ItemViewLatent>();
            for (int i = 0; i < this.Items.Count; i++)
            {
                ItemViewLatent it = new ItemViewLatent();
                it.ItemId = this.Items[i];
                it.LatentFeatures = String.Join(";", Array.ConvertAll((float[])ItemArray[i], x => x.ToString()));
                ibl.Add(it);
            }

            _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE UserViewLatents");
            _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE ItemViewLatents");
            _dbContext.SaveChanges();

            _dbContext.UserViewLatents.AddRange(ubl);
            _dbContext.ItemViewLatents.AddRange(ibl);
            _dbContext.SaveChanges();
        }
    }

    /// <summary>
    /// Gets num recommendations for a user based on the db saved data.
    /// </summary>
    /// <param name="userId">The user to get recommendations</param>
    /// <param name="num">Number of items to recommend</param>
    /// <returns></returns>
    public List<int>? GetRecommendations(int userId, int num = 5)
    {
        User? usr = _dbContext.Users
            .Include(u => u.Bids)
            .Include(u => u.VisitedItems)
            .SingleOrDefault(u => u.UserId == userId);

        if (usr == null) return null;
        
        // try to get latents for bid
        NDArray? userLatents = LoadUserLatents(userId, "bid");
        List<ItemLatents> itemsLatents;
        if (userLatents == null)
        {
            // go to viewed latents otherwise
            userLatents = LoadUserLatents(userId, "view");
            if (userLatents == null) return null;
            
            itemsLatents = LoadItemsLatents("view");
        }
        else
        {
            itemsLatents = LoadItemsLatents("bid");
        }
        
        // get bidded and viewed items by user to avoid recommending them again
        List<int> biddedItems = usr.Bids != null ? usr.Bids.Select(b => b.ItemId).Distinct().ToList() : new List<int>();
        List<int> visitedItems = usr.VisitedItems != null ? usr.VisitedItems.Select(b => b.ItemId).Distinct().ToList() : new List<int>();
        
        // lists to keep the best match items
        List<double> maxValues = new List<double>();
        List<int> maxItems = new List<int>();

        for (int i = 0; i < itemsLatents.Count; i++)
        {
            // get the user-item score by calculating the dot product
            double value = double.Parse(userLatents!.dot(itemsLatents[i].Latents.transpose()).ToString());
            int itemId = itemsLatents[i].ItemId;
            
            // have already seen the item - do not recommend
            if (biddedItems.Contains(itemId) || visitedItems.Contains(itemId))
            {
                continue;
            }
            
            // check if the auction is active ---- too expensive ??
            Item itm = _dbContext.Items.Find(itemId)!;
            if (itm.Ends < DateTime.Now || itm.Price >= itm.BuyPrice)
            {
                continue;
            }

            // fill the list first
            if (maxValues.Count < num)
            {
                maxValues.Add(value);
                maxItems.Add(itemId);
                continue;
            }

            // replace min value with a larger one
            if (maxValues.Any(v => v < value))
            {
                double minValue = maxValues.Min();
                int minIndex = maxValues.IndexOf(minValue);
                maxValues.RemoveAt(minIndex);
                maxItems.RemoveAt(minIndex);

                maxValues.Add(value);
                maxItems.Add(itemId);
            }
        }

        return maxItems;
    }

    private NDArray? LoadUserLatents(int user_id, string type = "bid")
    {
        string latents;
        if (type == "bid")
        {
            UserBidLatent? user = _dbContext.UserBidLatents.Find(user_id);
            if (user == null) return null;
            latents = user.LatentFeatures;
        }
        else
        {
            UserViewLatent? user = _dbContext.UserViewLatents.Find(user_id);
            if (user == null) return null;
            latents = user.LatentFeatures;
        }
        
        // convert back saved string to array
        NDArray lats = (NDArray)Array.ConvertAll(latents.Split(";"), Single.Parse);
        return lats;
    }

    private List<ItemLatents> LoadItemsLatents(string type = "bid")
    {
        List<ItemLatents> lst = new List<ItemLatents>();
        if (type == "bid")
        {
            List<ItemBidLatent> items = _dbContext.ItemBidLatents.ToList();
            lst = items.Select(i => new ItemLatents
            {
                ItemId = i.ItemId,
                // convert back saved string to array
                Latents = (NDArray)Array.ConvertAll(i.LatentFeatures.Split(";"), Single.Parse)
            }).ToList();
        }
        else
        {
            List<ItemViewLatent> items = _dbContext.ItemViewLatents.ToList();
            lst = items.Select(i => new ItemLatents
            {
                ItemId = i.ItemId,
                // convert back saved string to array
                Latents = (NDArray)Array.ConvertAll(i.LatentFeatures.Split(";"), Single.Parse)
            }).ToList();
        }

        return lst;
    }

    private int CountOccurrences<T>(List<T> lista, T value)
    {
        int count = 0;
        foreach (T t in lista)
        {
            if (t.Equals(value))
                count++;
        }

        return count;
    }
}