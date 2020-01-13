using Graph.Application.Graph;
using Graph.Infrastructure.Database.Command.Interfaces;
using Graph.Infrastructure.Database.Command.Repository;
using Microsoft.Extensions.DependencyInjection;
using GraphQL.Types;
using Graph.Infrastructure.Database.Query;
using Graph.Infrastructure.Database.Query.UserSchema;
using Graph.Infrastructure.Database.Query.TaskSchema;
using Graph.Infrastructure.Database.Query.Manager;
using GraphQL;
using Graph.Application.Graph.Types.Input;
using Graph.Application.Graph.User.Types.Query;
using MediatR;
using Graph.Domain.Service.CommandHandler;
using Graph.Application.Graph.Query;
using Graph.Infrastructure.Database.Query.ProjectSchema;
using Graph.Application.Graph.Project.Types.Input;
using Graph.Application.Commands.Project;
using Graph.Infrastructure.Database.Command;
using Graph.Application.Commands.User;
using Graph.Application.Commands.Task;
using Microsoft.Extensions.Options;
using Graph.Infrastructure.Database;
using Graph.Infrastructure.ServiceBus;
using Graph.Application.MessageHandler;
using AutoMapper;
using Graph.Domain.Service.Mappings;
using Graph.Application.Graph.Common;
using Graph.Application.Graph.User.Types.Input;
using Graph.Application.Graph.Project.Types.Query;
using Graph.Application.Graph.Mutation;
using Graph.Application.Graph.Task.Types.Input;
using Graph.Application.Graph.Task.Types.Query;

namespace Graph.CrossCutting.IoC
{
    public static class DependencyInjectionExtensions
    {
        #region Infrastructure
        public static void ResolveCommandDatabase(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<GraphContext>((serviceProvider, opts) => {
                opts.SetConnectionConfig(serviceProvider.GetService<IOptions<DatabaseConfiguration>>());
            });

            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IProjectRepository, ProjectRepository>();
            serviceCollection.AddScoped<ITaskRepository, TaskRepository>();

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ResolveCommandDatabaseIntegrationTests(this IServiceCollection serviceCollection,
                                                                  IUserRepository userRepository,
                                                                  IProjectRepository projectRepository,
                                                                  ITaskRepository taskRepository,
                                                                  IUnitOfWork unitOfWork)
        {
            serviceCollection.AddScoped(sp => userRepository);
            serviceCollection.AddScoped(sp => projectRepository);
            serviceCollection.AddScoped(sp => taskRepository);
            serviceCollection.AddScoped(sp => unitOfWork);
        }

        public static void ResolveQueryDatabase(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ManagerFactory>();

            serviceCollection.AddSingleton((sp) => sp.GetRequiredService<ManagerFactory>().GetManager<User>());
            serviceCollection.AddSingleton((sp) => sp.GetRequiredService<ManagerFactory>().GetManager<Project>());
            serviceCollection.AddSingleton((sp) => sp.GetRequiredService<ManagerFactory>().GetManager<Task>());

            serviceCollection.AddSingleton<IEntityManager<User>, UserManager>();
            serviceCollection.AddSingleton<IEntityManager<Project>, ProjectManager>();
            serviceCollection.AddSingleton<IEntityManager<Task>, TaskManager>();
        }

        public static void ResolveServiceBus(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IServiceBus, MassTransitSB>();
            serviceCollection.AddSingleton<ISubscribe, BusMessageHandler>();
        }

        #endregion

        #region Domain Service
        public static void ResolveRequestHandlers(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(typeof(UserCommandHandler), typeof(ProjectCommandHandler), typeof(TaskCommandHandler));

            serviceCollection.AddScoped<IRequestHandler<AddUserCommand, bool>, UserCommandHandler>();
            serviceCollection.AddScoped<IRequestHandler<UpdateUserInfoCommand, bool>, UserCommandHandler>();

            serviceCollection.AddScoped<IRequestHandler<AddProjectCommand, bool>, ProjectCommandHandler>();
            serviceCollection.AddScoped<IRequestHandler<AddUserProjectCommand, bool>, ProjectCommandHandler>();
            serviceCollection.AddScoped<IRequestHandler<RemoveUserProjectCommand, bool>, ProjectCommandHandler>();

            serviceCollection.AddScoped<IRequestHandler<AddTaskCommand, bool>, TaskCommandHandler>();
            serviceCollection.AddScoped<IRequestHandler<RemoveTaskCommand, bool>, TaskCommandHandler>();
            serviceCollection.AddScoped<IRequestHandler<ChangeAssigneeCommand, bool>, TaskCommandHandler>();
            serviceCollection.AddScoped<IRequestHandler<UpdateTaskStatusCommand, bool>, TaskCommandHandler>();
            serviceCollection.AddScoped<IRequestHandler<UpdateDeadlineCommand, bool>, TaskCommandHandler>();
        }

        #endregion

        public static void ResolveGraphDependencies(this IServiceCollection serviceCollection, bool testing = false)
        {
            serviceCollection.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

            serviceCollection.AddSingleton<IDocumentExecuter, DocumentExecuter>();

            #region Common

            
            serviceCollection.AddSingleton<MutationResultType>();
            
            serviceCollection.AddSingleton<UpdateDescriptionInput>();

            serviceCollection.AddSingleton<TaskStatusEnumType>();

            serviceCollection.AddSingleton<PaginationType>();
            serviceCollection.AddSingleton<FilterType>();
            serviceCollection.AddSingleton<EnumFilterType>();
            serviceCollection.AddSingleton<DateFilterType>();

            #endregion

            #region User

            #region Query

            serviceCollection.AddSingleton<UserQuery>();

            serviceCollection.AddSingleton<UserFilterType>();
            
            serviceCollection.AddSingleton<UserType>();
            serviceCollection.AddSingleton<UserProjectType>();

            #endregion

            #region Mutation

            serviceCollection.AddSingleton<UserMutation>();
            
            serviceCollection.AddSingleton<AddUserInput>();
            serviceCollection.AddSingleton<EditUserInput>();

            #endregion

            #endregion

            #region Project

            #region Query

            serviceCollection.AddSingleton<ProjectQuery>();

            serviceCollection.AddSingleton<ProjectFilterType>();
            
            serviceCollection.AddSingleton<ProjectType>();
            serviceCollection.AddSingleton<ProjectUserType>();
            serviceCollection.AddSingleton<ProjectTaskType>();

            #endregion

            #region Mutation

            serviceCollection.AddSingleton<ProjectMutation>();

            serviceCollection.AddSingleton<UserProjectInput>();
            serviceCollection.AddSingleton<AddProjectInput>();

            #endregion

            #endregion

            #region Task

            #region Query

            serviceCollection.AddSingleton<TaskQuery>();

            serviceCollection.AddSingleton<TaskFilterType>();
            
            serviceCollection.AddSingleton<TaskType>();
            serviceCollection.AddSingleton<TaskUserType>();
            serviceCollection.AddSingleton<TaskProjectType>();

            #endregion

            #region Mutation

            serviceCollection.AddSingleton<TaskMutation>();

            serviceCollection.AddSingleton<AddTaskInput>();
            serviceCollection.AddSingleton<ChangeAssigneeInput>();
            serviceCollection.AddSingleton<UpdateDeadlineInput>();
            serviceCollection.AddSingleton<UpdateTaskStatusInput>();

            #endregion

            #endregion

            if(!testing) serviceCollection.AddSingleton<ISchema, GraphSchema>();
        }

        public static void ResolveAuxiliaries(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(UserProfile),
                                            typeof(ProjectProfile),
                                            typeof(TaskProfile),
                                            typeof(StatusProfile));
        }
    }
}
