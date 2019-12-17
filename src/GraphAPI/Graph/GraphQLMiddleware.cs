using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Instrumentation;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Graph.API
{
    public class GraphQLMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly GraphQLSettings _settings;
        private readonly IDocumentExecuter _executer;

        private static readonly JsonArrayPool _ArrayPool = new JsonArrayPool(ArrayPool<char>.Shared);

        public GraphQLMiddleware(
            RequestDelegate next,
            GraphQLSettings settings,
            IDocumentExecuter executer
            )
        {
            _next = next;
            _settings = settings;
            _executer = executer;
        }

        public async Task Invoke(HttpContext context, ISchema schema)
        {
            if (!IsGraphQLRequest(context))
            {
                await _next(context);
                return;
            }

            await ExecuteAsync(context, schema);
        }

        private bool IsGraphQLRequest(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments(_settings.Path)
                && string.Equals(context.Request.Method, "POST", StringComparison.OrdinalIgnoreCase);
        }

        private async Task ExecuteAsync(HttpContext context, ISchema schema)
        {
            var request = await Deserialize<GraphQLRequest>(context.Request.Body);

            var result = await _executer.ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = request?.Query;
                _.OperationName = request?.OperationName;
                _.Inputs = request?.Variables.ToInputs();
                _.UserContext = _settings.BuildUserContext?.Invoke(context);
                _.ValidationRules = DocumentValidator.CoreRules().Concat(new [] { new InputValidationRule() });
                _.EnableMetrics = _settings.EnableMetrics;
                if (_settings.EnableMetrics)
                {
                    _.FieldMiddleware.Use<InstrumentFieldsMiddleware>();
                }
            });

            await WriteResponseAsync(context, result);
        }

        private async Task WriteResponseAsync(HttpContext context, ExecutionResult result)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = result.Errors?.Any() == true ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.OK;

            await WriteAsync(context.Response.Body, result);
        }

        private async static Task<T> Deserialize<T>(Stream s)
        {
            using (var reader = new StreamReader(s))
            {
                var content = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        private async static Task WriteAsync(Stream stream, ExecutionResult value)
        {
            using (var writer = new HttpResponseStreamWriter(stream, new UTF8Encoding(false)))
            using (var jsonWriter = new JsonTextWriter(writer)
            {
                ArrayPool = _ArrayPool,
                CloseOutput = false,
                AutoCompleteOnClose = false,
            })
            {
                await jsonWriter.WriteRawValueAsync(JsonConvert.SerializeObject(value.GetValue(), Formatting.Indented));
                await jsonWriter.FlushAsync().ConfigureAwait(false);
            }
        }


    }
}
