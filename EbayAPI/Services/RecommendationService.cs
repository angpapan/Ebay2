using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Dynamic.Core;
using EbayAPI.Data;
using EbayAPI.Dtos;
using EbayAPI.Models;
using Microsoft.AspNetCore.Mvc;
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
    //private Dictionary<int, List<int>> BaseDict;
    private Dictionary<int, Dictionary<int,int>> BaseDict;
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
        BaseDict = new Dictionary<int, Dictionary<int,int>>();
        this.SampleSize = 0;
        /*
        foreach (UserItem ui in data)
        {
            if (!BaseDict.ContainsKey(ui.UserId))
            {
                BaseDict[ui.UserId] = new List<int>();
            }

            BaseDict[ui.UserId].Add(ui.ItemId);
            this.SampleSize++;
        }
        */
        
        // get a list of users
        Users = _dbContext.Users.Select(user => user.UserId).ToList();
        //Users = data.Select(i => i.UserId).Distinct().ToList();
        //Users.Sort();
        
        // get a list of items
        Items = _dbContext.Items.Select(item => item.ItemId).ToList();
        //Items = data.Select(i => i.ItemId).Distinct().ToList();
        //Items.Sort();
        
        // get rates
        foreach (var user in Users)
        {
            BaseDict[user] = GetRates(user);
        }

        Features = features;
        LearningRate = learningRate;
        Steps = steps;
        Sensitivity = sensitivity;
        
        // initialize arrays for users and items with random values in (-1, 1)
        UserArray = np.random.stardard_normal(new int[] { Users.Count, this.Features }).astype(typeof(float));
        ItemArray = np.random.stardard_normal(new int[] { Items.Count, this.Features }).astype(typeof(float));
    }
    
    
    public void AddNewItem(int itemId)
    {
        var newItemLatens = np.random.stardard_normal( Features ).astype(typeof(float));
        
        ItemBidLatent il = new ItemBidLatent();
        il.ItemId = itemId;
        il.LatentFeatures = String.Join(";", Array.ConvertAll((float[])newItemLatens, x => x.ToString()));
        Console.WriteLine($"New latents for item : {itemId}");
        _dbContext.ItemBidLatents.Add(il);
        _dbContext.SaveChanges();
        
    }
    
    private NDArray? AddNewUser(int userId)
    {
        var t = _dbContext.Users.FirstOrDefault(user => user.UserId == userId);
        if(t == null) 
            return null;
        
        int features = Features > 0 ? Features : 25; 
        var newUserLatens = np.random.stardard_normal(  features  ).astype(typeof(float));
       
        UserBidLatent ul = new UserBidLatent();
        ul.UserId = userId;
        ul.LatentFeatures = String.Join(";", Array.ConvertAll((float[])newUserLatens, x => x.ToString()));
        
        _dbContext.UserBidLatents.Add(ul);
        _dbContext.SaveChanges();
        
        return newUserLatens;

    }
    
    public void InitNew2( double learningRate = 0.001, double sensitivity = 0.001,
        int features = 25, int steps = 1000)
    {
        
        // Convert List to Dictionary from userIds to viewed/bid items.
        // Justified because of the number of searches during factorization
        BaseDict = new Dictionary<int, Dictionary<int,int>>();
        SampleSize = 0;
       
        
        // get a list of users
        Users = _dbContext.Users.Select(user => user.UserId).ToList();
        //Users = data.Select(i => i.UserId).Distinct().ToList();
        //Users.Sort();
        
        // get a list of items
        Items = _dbContext.Items.Select(item => item.ItemId).ToList();
        //Items = data.Select(i => i.ItemId).Distinct().ToList();
        //Items.Sort();
        // get rates
        foreach (var user in Users)
        {
            BaseDict[user] = GetRates(user);
        }
        Features = features;
        LearningRate = learningRate;
        Steps = steps;
        Sensitivity = sensitivity;
        
        // initialize arrays for users and items with random values in (-1, 1)
        UserArray = np.random.stardard_normal(new int[] { Users.Count, this.Features }).astype(typeof(float));
        ItemArray = np.random.stardard_normal(new int[] { Items.Count, this.Features }).astype(typeof(float));
    }
    
    /// <summary>
    /// Returns the rates of a user to every item.Rates are in range 1-3
    /// 3 -> user has bid on item / high views => likes it for sure
    /// 2 -> user has view item more than once / medium views => possibly likes it
    /// 1 -> user has one view on item / low views => low chance likes it
    /// 0 -> no rating
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>A dictionary with rate for every item that user has view or bid</returns>

    private Dictionary<int,int> GetRates(int userId)
    {
        List<int> bidsOfUser = _dbContext.Bids.Where(bid => bid.UserId == userId).Select(bid=>bid.ItemId).ToList();
        List<int> viewsOfUser = _dbContext.UserVisitedItems.Where(u => u.UserId == userId).Select(u => u.ItemId).ToList();
        Dictionary<int, int> Rates = new Dictionary<int, int>();
        //Console.WriteLine("RatesOk");
        IQueryable<int> viewsWithOutBid = viewsOfUser.AsQueryable().Where(i => !bidsOfUser.Contains(i));
        
        

        if (bidsOfUser.Count > 0)
        {
            foreach (var item in bidsOfUser)
            {
                Rates[item] = 3;
            }
            foreach (var item in viewsWithOutBid)
            {
                int viewsOnItem = viewsOfUser.Count(i => i.Equals(item));
                Rates[item] = viewsOnItem > 1 ? 2 : viewsOnItem;
            }
        }
        else if(viewsOfUser.Any())
        {
            var maxViews = viewsOfUser.GroupBy(i => i).Select(i=>i.Count()).Max();
            foreach (var item in viewsOfUser)
            {
                var viewsOnItem = viewsOfUser.Count(i => i.Equals(item));
                Rates[item] = 3 * viewsOnItem / maxViews;
            }
        }
        
//::todo make rates canonical 0-1 
        return Rates;

    }

    private int getRate(int userId, int itemId)
    {
        if (BaseDict[userId].Count > 0 && BaseDict[userId].ContainsKey(itemId))
        {
            return BaseDict[userId][itemId];
        }

        return 0;
    }

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

                    //int occurrences = this.CountOccurrences(this.BaseDict[userId], itemId); 
                    var occurrences = getRate(userId, itemId);//this.BaseDict[userId][itemId];
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
            SampleSize = 0;
            for (int i = 0; i < this.Users.Count; i++)
            {
                for (int j = 0; j < this.Items.Count; j++)
                {
                    int userId = this.Users[i];
                    int itemId = this.Items[j];
                    int occurrences = getRate(userId, itemId);//this.BaseDict[userId][itemId]; 
                    if (occurrences > 0)
                    {
                        SampleSize++;
                        error += Math.Pow(
                            occurrences - (double)this.UserArray[$"{i}, :"].dot(this.ItemArray[$":, {j}"]),
                            2);
                    }
                }
            }
            
            // get MSRE
            error = Math.Sqrt(error / SampleSize);

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
    
    public void Factorize2()
    {
        this.ItemArray = this.ItemArray.transpose();
        double previousError = 10e9;
        
        for (int step = 0; step < this.Steps; step++)
        {
            foreach (var userId in BaseDict.Keys)
            {
                foreach (var itemId in BaseDict[userId].Keys)
                {
                    
                    //int occurrences = this.CountOccurrences(this.BaseDict[userId], itemId); 
                    var occurrences = getRate(userId, itemId);//this.BaseDict[userId][itemId];
                    if (occurrences > 0)
                    {
                        // get the dot product of i-th user row with j-th item column
                        NDArray userTemp = this.UserArray[$"{userId-1},:"];
                        NDArray itemTemp = this.ItemArray[$":,{itemId-1}"];
                        NDArray dotted = userTemp.dot(itemTemp);
                        string str = dotted.ToString();
                        double compValue = double.Parse(str);
                        
                        // The number of the times the user bidded/viewed the item 
                        // is a meter of how much he likes it
                        double eij = occurrences - compValue;
                        
                        // change the matrices values 
                        for (int f = 0; f < this.Features; f++)
                        {
                            this.UserArray[userId-1][f] += this.LearningRate * 2 * eij * this.ItemArray[f][itemId-1];
                            this.ItemArray[f][itemId-1] += this.LearningRate * 2 * eij * this.UserArray[userId-1][f];
                        }
                    }
                }
            }
            
            // calculate global squared error
            double error = 0;
            SampleSize = 0;
            
            foreach (var userId in BaseDict.Keys)
            {
                foreach (var itemId in BaseDict[userId].Keys)
                {
                    int occurrences = getRate(userId, itemId);//this.BaseDict[userId][itemId]; 
                    if (occurrences > 0)
                    {
                        SampleSize++;
                        error += Math.Pow(
                            occurrences - (double)this.UserArray[$"{userId-1}, :"].dot(this.ItemArray[$":, {itemId-1}"]),
                            2);
                    }
                }
            }
            
            // get MSRE
            error = Math.Sqrt(error / SampleSize);

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
    
    
    
    /// <summary>
    /// Gets num recommendations for a user based on the db saved data.
    /// </summary>
    /// <param name="userId">The user to get recommendations</param>
    /// <param name="num">Number of items to recommend</param>
    /// <returns></returns>
    public List<int>? GetRecommendations2(int userId, int num = 5)
    {

        var userEntry = _dbContext.UserBidLatents.Find(userId);
        var userLatents = 
            userEntry == null ? AddNewUser(userId) : (NDArray)Array.ConvertAll(userEntry.LatentFeatures.Split(';'), Single.Parse);
        if (userLatents == null)
            return null;
        var temp = new List<itemRating>();
        foreach (var item in _dbContext.ItemBidLatents)
        {
            var itemL = (NDArray)Array.ConvertAll(item.LatentFeatures.Split(";"), Single.Parse);
            temp.Add( new itemRating {
                itemId = item.ItemId,
                Rating = double.Parse(userLatents.dot(itemL).ToString())
            });
        }

        var l = temp.OrderByDescending(i => i.Rating).Select(i => i.itemId).ToArray();
        var entry = new UserViewLatent
        {
            UserId = userId,
            LatentFeatures = String.Join(";", Array.ConvertAll(l, x => x.ToString()))
        };
        return l.Take(num).ToList();
    }

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: NumSharp.Backends.UnmanagedStorage")]
    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public void UpdateRecommendationTable()
    {
        var stopWatch = Stopwatch.StartNew();
        _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE UserViewLatents");
        _dbContext.SaveChanges();
        var itemLatents = new Dictionary<int, NDArray>();

        foreach (var ibl in _dbContext.ItemBidLatents.ToList())
        {
            itemLatents[ibl.ItemId] = (NDArray)Array.ConvertAll(ibl.LatentFeatures.Split(";"), Single.Parse);
        }

        foreach (var ubl in _dbContext.UserBidLatents.ToList())
        {
            var user = ubl.UserId;
            var userLatents = (NDArray)Array.ConvertAll(ubl.LatentFeatures.Split(";"), Single.Parse);

            var temp = new List<itemRating>();
            foreach (var item in itemLatents.Keys)
            {
                var itemR = new itemRating
                {
                    itemId = item,
                    Rating = double.Parse(userLatents!.dot(itemLatents[item]).ToString())
                };
                temp.Add(itemR);
            }
            var l = temp.OrderByDescending(i => i.Rating).Select(i=>i.itemId).ToArray();
            var entry = new UserViewLatent
            {
                UserId = user,
                LatentFeatures = String.Join(";", Array.ConvertAll((int[])l, x => x.ToString()))
            };
            _dbContext.UserViewLatents.Add(entry);
            _dbContext.SaveChanges();
            if(user%50 == 0)
                //break;
                Console.WriteLine($"User {user} done");
        }
        
        stopWatch.Stop();
        Console.WriteLine($"Finish Init and starting Factorize in {stopWatch.Elapsed.Minutes} : {stopWatch.Elapsed.Seconds} : {stopWatch.Elapsed.Milliseconds}");


    }

    private class itemRating
    {
        public int itemId;
        public double Rating;

    } 


}