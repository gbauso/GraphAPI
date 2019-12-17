using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.CrossCutting.Extensions.GraphQL
{
    public static class FilterUtils
    {
        public static IDictionary<string, GraphFilter> ParseArgumentFilter(this IDictionary<string, object> filters)
        {
            var dictionary = new Dictionary<string, GraphFilter>();
            if (filters == null) return dictionary;

            foreach (var filter in filters)
            {
                var innerDictionary = (IDictionary<string, object>)filter.Value;

                var filterObject = new GraphFilter()
                {
                    Value = GetValue(innerDictionary["value"]),
                    Operation = innerDictionary["operation"].ToString()
                };

                ValidateOperation(filterObject.Operation);

                dictionary.Add(filter.Key, filterObject);
            }

            return dictionary;

        }

        private static void ValidateOperation(string operation)
        {
            var allowedOperations = new[] { "le", "l", "c", "g", "ge", "e", "ne" };

            if (!allowedOperations.Any(i => i == operation))
                throw new ArgumentException("The type of provided filter is invalid");
        }

        private static object GetValue(object obj)
        {
            DateTime date;
            if(DateTime.TryParse(obj.ToString(), out date))
            {
                return date.ToUnixTime();
            }

            return obj.ToString();
        }
    }
}
