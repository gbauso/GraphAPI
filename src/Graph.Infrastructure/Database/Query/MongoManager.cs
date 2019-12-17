using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Graph.CrossCutting.Extensions.GraphQL;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Graph.CrossCutting.Exceptions;
using MongoDB.Bson;
using System.Linq;
using Graph.CrossCutting.Interfaces;
using Graph.CrossCutting.Extensions;

namespace Graph.Infrastructure.Database.Query
{
    public class MongoManager<T> : IManager<T> where T : IQueryModel
    {
        protected readonly IMongoCollection<T> _Collection;
        protected const string MONGO_ID = "_id";
        protected const string ENTITY_ID = "id";
        protected const string DATABASE = "graph";

        public MongoManager(IOptions<DatabaseConfiguration> options)
        {
            var client = new MongoClient(options.Value.ReadDatabase);
            var database = client.GetDatabase(DATABASE);
            _Collection = database.GetCollection<T>(typeof(T).Name.ToLower());
        }

        public virtual async Task<IEnumerable<T>> Get(string[] fields, IDictionary<string, GraphFilter> filters, string order, int skip = 0, int take = 10)
        {
            var filter = GetFilter(filters);
            var projection = GetProjection(fields);
            var sort = GetSort(order);

            var finder = await _Collection.Find(filter)
                                           .Sort(sort)
                                           .Skip(skip)
                                           .Limit(take)
                                           .Project(projection)
                                           .ToListAsync();

            var json = ReplaceIdentification(finder.ToJson());

            return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
        }

        

        public virtual async Task<T> GetById(Guid id, string[] fields)
        {
            var filter = Builders<T>.Filter.Eq(MONGO_ID, id.ToString());
            var projection = GetProjection(fields);

            try
            {
                var finder = await _Collection.Find(filter).Project(projection).FirstAsync();

                var json = ReplaceIdentification(finder.ToJson());

                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default;
            }

        }

        public virtual async Task<bool> Index(T entry)
        {
            await _Collection.InsertOneAsync(entry);

            return true;
        }

        public virtual async Task<bool> Remove(Guid entryId)
        {
            var filter = Builders<T>.Filter.Eq(MONGO_ID, entryId.ToString());

            await _Collection.DeleteOneAsync(filter);

            return true;
        }

        private ProjectionDefinition<T> GetProjection(string[] fields)
        {
            var projection = Builders<T>.Projection.Include(MONGO_ID);

            fields = fields.Select(i => i.EndsWith(ENTITY_ID) ? ReplaceIdentification(i, true) : i).ToArray();

            foreach (var field in fields)
            {
                projection = projection.Include(field);
            }

            return projection;
        }

        private FilterDefinition<T> GetFilter(IDictionary<string, GraphFilter> filters)
        {
            var builder = Builders<T>.Filter;
            var filterDefinition = builder.Empty;

            foreach(var filter in filters)
            {
                filterDefinition &= GetFilterType(builder, filter.Key, filter.Value);
            }

            return filterDefinition;
        }

        private FilterDefinition<T> GetFilterType(FilterDefinitionBuilder<T> builder ,string field, GraphFilter filter)
        {
            if (field.Equals(ENTITY_ID)) ReplaceIdentification(field, true);

            switch(filter.Operation)
            {
                case "e":
                    return builder.Eq(field, filter.Value);
                case "c":
                    return builder.Regex(field, new BsonRegularExpression($".*{filter.Value}.*","i"));
                case "g":
                    return builder.Gt(field, filter.Value);
                case "ge":
                    return builder.Gte(field, filter.Value);
                case "l":
                    return builder.Lt(field, filter.Value);
                case "le":
                    return builder.Lte(field, filter.Value);
                case "ne":
                    return builder.Ne(field, filter.Value);
                default:
                    return builder.Empty;
            }
        }

        private SortDefinition<T> GetSort(string order)
        {
            if(string.IsNullOrEmpty(order)) return Builders<T>.Sort.Ascending(MONGO_ID);

            var sortTypes = new[] { "asc", "desc" };

            var pairFieldSortType = order.Split(' ');

            if (pairFieldSortType.Count() != 2) throw new QueryArgumentException("QA-01");

            var field = this.ReplaceIdentification(pairFieldSortType.ElementAtOrDefault(0).ToLower(), true);
            var sortType = pairFieldSortType.ElementAtOrDefault(1).ToLower();

            if (typeof(T).GetField(field) != null) throw new QueryArgumentException("QA-02");
            if (!sortTypes.Contains(sortType)) throw new QueryArgumentException("QA-03");

            if (sortType == "asc") return Builders<T>.Sort.Ascending(field);
            else return Builders<T>.Sort.Descending(field);
        }

        protected string ReplaceIdentification(string text, bool reverse = false)
        {
            if (reverse) return text.Replace(ENTITY_ID, MONGO_ID);
            else return text.Replace(MONGO_ID, ENTITY_ID);
        }
    }
}
