using Graph.CrossCutting.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph.CrossCutting.Extensions
{
    public static class CodeBaseExtensions
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static void Update<L>(this ICollection<L> list, L toUpdate) where L : IDomain
        {
            var original = list.FirstOrDefault(i => i.Id == toUpdate.Id);
            
            if(!original.IsNull())
            {
                list.Remove(original);
                list.Add(toUpdate);
            }
        }

        public static bool ListIsNullOrEmpty<L>(this IEnumerable<L> list)
        {
            return list.IsNull() || list.Count() == 0 || list.Any(i => i.IsNull());
        }

        public static long ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }

        public static DateTime UnixToDateTime(this long unixTimeStamp)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return date.AddSeconds(unixTimeStamp).ToLocalTime();
        }
    }
}
