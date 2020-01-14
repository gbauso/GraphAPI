using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;
using Graph.CrossCutting.IoC;
using Graph.Infrastructure.Database.Query.UserSchema;
using Graph.Infrastructure.Database.Query.Manager;
using Graph.Infrastructure.Database.Query.ProjectSchema;
using Graph.Infrastructure.Database.Query.TaskSchema;

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

            services.AddTransient(sp => MockHelper.GetUserManager());
            services.AddTransient<IEntityManager<User>, UserManager>();

            services.AddTransient(sp => MockHelper.GetProjectManager());
            services.AddTransient<IEntityManager<Project>, ProjectManager>();

            services.AddTransient(sp => MockHelper.GetTaskManager());
            services.AddTransient<IEntityManager<Task>, TaskManager>();

            services.AddTransient(sp => MockHelper.GetServiceBus());
            services.AddTransient(sp => MockHelper.GetUserRepository());
            services.AddTransient(sp => MockHelper.GetProjectRepository());
            services.AddTransient(sp => MockHelper.GetTaskRepository());
            services.AddTransient(sp => MockHelper.GetUnitOfWork());
        }
    }
}