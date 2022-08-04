using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

#pragma warning disable CS1591

namespace EbayAPI.Helpers;

public class QuerySortHelper<T>
{
    public IQueryable<T> QuerySort(IQueryable<T> entities, string orderByString)
    {
        if (!entities.Any())
            return entities;

        if (string.IsNullOrWhiteSpace(orderByString))
            return entities;

        var orderParams = orderByString.Trim().Split(',');
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param))
                continue;

            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos.FirstOrDefault((pi =>
                pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase)));

            if (objectProperty == null)
                continue;

            var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

            orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
        }

        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

        return entities.OrderBy(orderQuery);
    }
}