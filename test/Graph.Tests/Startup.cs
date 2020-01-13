using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;
using Graph.CrossCutting.IoC;
using Graph.API;
using Moq;
using Microsoft.AspNetCore.Http;

[assembly: TestFramework("Graph.Tests.Startup", "Graph.Tests")]

namespace Graph.Tests
{
    public class Startup : DependencyInjectionTestFramework
    {
        public Startup(IMessageSink messageSink) : base(messageSink) { }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.ResolveGraphDependencies(true);
            services.ResolveCommandDatabaseIntegrationTests(MockHelper.GetUserRepository(),
                                                            MockHelper.GetProjectRepository(),
                                                            MockHelper.GetTaskRepository(),
                                                            MockHelper.GetUnitOfWork());
            services.ResolveRequestHandlers();
            services.ResolveAuxiliaries();
            services.AddSingleton(sp => MockHelper.GetServiceBus());
        }
    }
}