using Graph.CrossCutting.Extensions.GraphQL;
using Graph.CrossCutting.Interfaces;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graph.Infrastructure.Database.Query
{
    public class ElasticSearchManager<T> : IManager<T> where T : class, IQueryModel
    {
        protected readonly IElasticClient _Client;
        protected readonly string _Index;
        public ElasticSearchManager(IOptions<DatabaseConfiguration> options)
        {
            _Index = typeof(T).Name.ToLower();
            var node = new Uri(options.Value.ReadDatabase);
            var settings = new ConnectionSettings(node);
            _Client = new ElasticClient(settings);
        }

        public async Task<IEnumerable<T>> Get(string[] fields,
                                        IDictionary<string, GraphFilter> filters,
                                        string order,
                                        int skip,
                                        int take)
        {
            var request = new SearchRequest<T>(_Index)
            {
                From = skip,
                Size = take,
                Sort = GetSort(order),
                Query = GetQueryContainer(filters),
                Source = GetProjection(fields)
            };

            var response = await _Client.SearchAsync<T>(request);

            return response.Documents.AsEnumerable();
        }

        public async Task<T> GetById(Guid id, string[] fields)
        {
            var request = new SearchRequest(_Index)
            {
                Query = new TermQuery() { Field = "_id", Value = id.ToString(), IsStrict = true },
                Source = GetProjection(fields)
            };

            var response = await _Client.SearchAsync<T>(request);

            return response.Documents.FirstOrDefault();
        }

        public async Task<bool> Index(T entry)
        {
            await _Client.IndexAsync(entry, idx => idx.Index(_Index));

            return true;
        }

        public async Task<bool> Remove(Guid entryId)
        {
            var request = new DeleteByQueryRequest<T>(_Index)
            {
                Query = new TermQuery() { Field = "_id", Value = entryId.ToString(), IsStrict = true }
            };

            var response = await _Client.DeleteByQueryAsync(request);

            return response.Took > 0;
        }

        private Union<bool, ISourceFilter> GetProjection(string[] fields)
        {
            var projection = new SourceFilterDescriptor<T>();

            projection.Includes(i => i.Fields(fields));

            return projection;
        }

        private IList<ISort> GetSort(string sort)
        {
            var list = new List<ISort>();
            if (string.IsNullOrEmpty(sort)) return null;

            var sortArray = sort.Split(' ');

            var sortField = new FieldSort()
            {
                Field = $"{sortArray[0]}.keyword",
                Order = (sortArray[1] == "asc") ? SortOrder.Ascending : SortOrder.Descending,
                UnmappedType = FieldType.Text
            };

            list.Add(sortField);

            return list;
        }

        private QueryContainer GetQueryContainer(IDictionary<string, GraphFilter> filters)
        {
            if (filters.Count == 0) return new MatchAllQuery();

            var container = new QueryContainer();

            foreach (var filter in filters)
            {
                container = container && GetQueryType(filter.Key, filter.Value);
            }

            return container;
        }

        private QueryContainer GetQueryType(string field, GraphFilter graphFilter)
        {
            switch (graphFilter.Operation)
            {
                case "c":
                    return new WildcardQuery()
                    {
                        Field = field,
                        Value = $"*{graphFilter.Value.ToString().ToLower()}*"
                    };
                case "e":
                    return new MatchQuery()
                    {
                        Field = $"{field}.keyword",
                        Query = graphFilter.Value.ToString(),
                    };
                case "g":
                    return new NumericRangeQuery()
                    {
                        Field = field,
                        GreaterThan = (double)graphFilter.Value
                    };
                case "ge":
                    return new NumericRangeQuery()
                    {
                        Field = field,
                        GreaterThanOrEqualTo = (double)graphFilter.Value
                    };
                case "l":
                    return new NumericRangeQuery()
                    {
                        Field = field,
                        LessThan = (double)graphFilter.Value
                    };
                case "le":
                    return new NumericRangeQuery()
                    {
                        Field = field,
                        LessThanOrEqualTo = (double)graphFilter.Value
                    };
                case "ne":
                    var newFilter = graphFilter;
                    newFilter.Operation = "e";

                    return new BoolQuery()
                    {
                        MustNot = new QueryContainer[] { GetQueryType(field, newFilter) }
                    };
                default:
                    throw new ArgumentException();
            }
        }
    }
}
