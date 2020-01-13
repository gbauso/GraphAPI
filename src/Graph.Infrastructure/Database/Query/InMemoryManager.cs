using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Graph.CrossCutting.Extensions.GraphQL;
using Graph.CrossCutting.Interfaces;
using System.Linq;
using System.Linq.Dynamic.Core;
using Graph.CrossCutting.Extensions;
using Newtonsoft.Json;
using System.Text;

namespace Graph.Infrastructure.Database.Query
{
    public class InMemoryManager<T> : IManager<T> where T : class, IQueryModel
    {
        private ICollection<T> _Data;

        public InMemoryManager()
        {
            _Data = new HashSet<T>();
        }

        public InMemoryManager(ICollection<T> data)
        {
            _Data = data;
        }

        public Task<IEnumerable<T>> Get(string[] fields, IDictionary<string, GraphFilter> filters, string order, int skip, int take)
        {
            var query = _Data.AsQueryable()
                              .Where(GetFilter(filters))
                              .OrderBy(order ?? "id asc")
                              .Skip(skip)
                              .Take(take)
                              .Select(GetProjection(fields));

            var json = JsonConvert.SerializeObject(query);
            var result = JsonConvert.DeserializeObject<IEnumerable<T>>(json);

            return Task.FromResult(result);
        }

        public Task<T> GetById(Guid id, string[] fields)
        {
            var query = _Data.AsQueryable()
                              .Where(i => i.Id == id.ToString())
                              .Skip(0).Take(1)
                              .Select(GetProjection(fields))
                              .FirstOrDefault();

            var json = JsonConvert.SerializeObject(query);
            var result = JsonConvert.DeserializeObject<T>(json);

            return Task.FromResult(result);
        }

        public Task<bool> Index(T entry)
        {
            _Data.Add(entry);

            return Task.FromResult(true);
        }

        public Task<bool> Remove(Guid entryId)
        {
            var original = _Data.FirstOrDefault(i => i.Id == entryId.ToString());
            _Data.Remove(original);

            return Task.FromResult(true);
        }

        private string GetProjection(string[] fields)
        {
            var projection = new List<string>();
            var grouppedFields = fields.GroupBy(i => i.Split('.')[0])
                                       .ToDictionary(
                                                        k => k.Key,
                                                        v => v.Select(i => i.Split('.').ElementAtOrDefault(1))
                                                     );

            foreach (var grouppedField in grouppedFields)
            {
                if (grouppedField.Value.ListIsNullOrEmpty()) projection.Add(grouppedField.Key);
                else
                {
                    var innerFields = string.Join(",", grouppedField.Value);

                    if (grouppedField.Key.EndsWith("s"))
                        projection.Add(string.Format("{0}.Select(new ({1})) as {0}", grouppedField.Key, innerFields));
                    else
                        projection.Add(string.Format("new ({0}) as {0}", innerFields));
                }
            }

            var inerProjection = string.Join(",", projection);

            return string.Format("new({0})", inerProjection);
        }

        private string GetFilter(IDictionary<string, GraphFilter> filters)
        {
            if (filters.ListIsNullOrEmpty()) return "1 == 1";

            var filterDefinition = new List<string>();

            foreach (var filter in filters)
            {
                filterDefinition.Add(GetFilterType(filter.Key, filter.Value));
            }

            return string.Join(" && ", filterDefinition);
        }

        private string GetFilterType(string field, GraphFilter filter)
        {
            switch (filter.Operation)
            {
                case "e":
                    return string.Format("{0} == \"{1}\"", field, filter.Value);
                case "c":
                    return string.Format("{0}.Contains(\"{1}\")", field, filter.Value);
                case "g":
                    return string.Format("{0} < {1}", field, filter.Value);
                case "ge":
                    return string.Format("{0} <= {1}", field, filter.Value);
                case "l":
                    return string.Format("{0} > {1}", field, filter.Value);
                case "le":
                    return string.Format("{0} >= {1}", field, filter.Value);
                case "ne":
                    return string.Format("{0} != \"{1}\"", field, filter.Value);
                default:
                    return string.Empty;
            }
        }

    }
}
