using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace MITD.Fuel.Presentation.Contracts
{
    public static class QueryConditionHelper
    {
        public static string GetSortByQueryString(Dictionary<string, string> sortBy)
        {
            var result = sortBy.Aggregate("", (current, pair) => current + (pair.Key + " " + pair.Value + ","));
            if (result.Length > 0)
                result = result.Remove(result.Count() - 1, 1);
            return result;
        }

        public static string GetQueryString<T>(T criteria) where T : class
        {
            //var propertyInfos = criteria.GetType().GetProperties();
            //var queryString = new NameValueCollection();
            //foreach (var p in propertyInfos)
            //{
            //    queryString.Add(p.Name, p.GetValue(criteria).ToString());
            //}
            //return queryString.ToString();
            return "";
        }
        public static string GetColumnQueryString(Dictionary<string, string> columns)
        {
            return "";
        }

        public static Dictionary<string, string> GetSortByDictionary(string sortBy)
        {

            var sortCollection = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var sortByList = sortBy.Split(',');
                foreach (var item in sortByList.Select(s => s.Split(' ')))
                    sortCollection.Add(item[0], item[1]);
            }
            return sortCollection;
        }


    }
}
