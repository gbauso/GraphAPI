using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;
using Graph.CrossCutting.IoC;

[assembly: TestFramework("Graph.Tests.Startup", "Graph.Tests")]

namespace Graph.Tests
{
    public class Startup : DependencyInjectionTestFramework
    {
        public Startup(IMessageSink messageSink) : base(messageSink) { }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.ResolveGraphDependencies(true);
            services.ResolveRequestHandlers();
            services.ResolveAuxiliaries();

            services.AddTransient(sp => MockHelper.GetServiceBus());
            services.AddTransient(sp => MockHelper.GetUserRepository());
            services.AddTransient(sp => MockHelper.GetProjectRepository());
            services.AddTransient(sp => MockHelper.GetTaskRepository());
            services.AddTransient(sp => MockHelper.GetUnitOfWork());
        }
    }
}