using AutoMapper;
using Graph.Domain.Service.Mappings;
using Graph.Infrastructure.Database.Command.Interfaces;
using Graph.Infrastructure.Database.Command.Model;
using QueryUser = Graph.Infrastructure.Database.Query.UserSchema;
using QueryProject = Graph.Infrastructure.Database.Query.ProjectSchema;
using QueryTask = Graph.Infrastructure.Database.Query.TaskSchema;
using Graph.Infrastructure.ServiceBus;
using Moq;
using System;
using System.Collections.Generic;
using Thread = System.Threading.Tasks;
using System.Linq;
using Graph.CrossCutting;
using Graph.Tests.Comparers;
using Graph.Infrastructure.Database.Query;
using Bogus;
using Graph.CrossCutting.Extensions;

namespace Graph.Tests
{
    public class MockHelper
    {
        public static Guid[] Guids = Enumerable.Range(0,7).Select(i => Guid.NewGuid()).ToArray();

        public static IServiceBus GetServiceBus()
        {
            var serviceBus = new Mock<IServiceBus>();
            return serviceBus.Object;
        }

        public static IMapper GetMapper()
        {
            var profiles = new Profile[] { new UserProfile(), new TaskProfile(), new ProjectProfile() };
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(profiles));
            var mapper = new Mapper(configuration);

            return mapper;
        }

        public static IUnitOfWork GetUnitOfWork()
        {
            var uow = new Mock<IUnitOfWork>();

            return uow.Object;
        }

        public static IManager<QueryUser.User> GetUserManager()
        {
            var userPosition = 0;
            var projectPosition = 3;
            var projectFaker = new Faker<QueryUser.UserProject>().StrictMode(true)
                                                       .RuleFor(u => u.Id, f => Guids[projectPosition++].ToString())
                                                       .RuleFor(u => u.Description, f => f.Company.CompanyName())
                                                       .RuleFor(u => u.LongDescription, f => f.Lorem.Paragraphs(1));

            var projects = projectFaker.Generate(2);

            var userFaker = new Faker<QueryUser.User>().StrictMode(true)
                                                       .RuleFor(u => u.Id, f => Guids[userPosition++].ToString())
                                                       .RuleFor(u => u.Email, f => f.Internet.Email())
                                                       .RuleFor(u => u.Name, f => f.Name.FullName())
                                                       .RuleFor(u => u.Projects, f => projects);

            var manager = new InMemoryManager<QueryUser.User>(userFaker.Generate(3));

            return manager;
        }

        public static IManager<QueryProject.Project> GetProjectManager()
        {
            var userPosition = 0;
            var projectPosition = 3;
            var taskPosition = 5;
            var userFaker = new Faker<QueryProject.ProjectUser>().StrictMode(true)
                                                       .RuleFor(u => u.Id, f => Guids[userPosition++].ToString())
                                                       .RuleFor(u => u.Name, f => f.Name.FullName());

            var users = userFaker.Generate(3);

            var taskFaker = new Faker<QueryProject.ProjectTask>().StrictMode(true)
                                                       .RuleFor(u => u.Id, f => Guids[taskPosition++].ToString())
                                                       .RuleFor(u => u.Description, f => f.Lorem.Sentence(25))
                                                       .RuleFor(u => u.Responsible, f => users[f.Random.Number(0,2)].Name)
                                                       .RuleFor(u => u.Status, f => ((TaskStatusEnum)f.Random.Number(1, 4)).ToString());

            var tasks = taskFaker.Generate(2);

            var projectFaker = new Faker<QueryProject.Project>().StrictMode(true)
                                                       .RuleFor(u => u.Id, f => Guids[projectPosition++].ToString())
                                                       .RuleFor(u => u.Description, f => f.Company.CompanyName())
                                                       .RuleFor(u => u.Participants, f => users)
                                                       .RuleFor(u => u.Tasks, f => tasks)
                                                       .RuleFor(u => u.UnfinishedCount, f => tasks.Count(i => i.Status != TaskStatusEnum.DONE.ToString()))
                                                       .RuleFor(u => u.FinishedCount, f => tasks.Count(i => i.Status == TaskStatusEnum.DONE.ToString()))
                                                       .RuleFor(u => u.LongDescription, f => f.Lorem.Paragraphs(1));

            var projects = projectFaker.Generate(2);

            var manager = new InMemoryManager<QueryProject.Project>(projects);

            return manager;
        }

        public static IManager<QueryTask.Task> GetTaskManager()
        {
            var userPosition = 0;
            var projectPosition = 3;
            var taskPosition = 5;
            var userFaker = new Faker<QueryTask.TaskUser>().StrictMode(true)
                                                       .RuleFor(u => u.Id, f => Guids[userPosition++].ToString())
                                                       .RuleFor(u => u.Name, f => f.Name.FullName());

            var users = userFaker.Generate(3);

            var projectFaker = new Faker<QueryTask.TaskProject>().StrictMode(true)
                                                       .RuleFor(u => u.Id, f => Guids[projectPosition++].ToString())
                                                       .RuleFor(u => u.Description, f => f.Company.CompanyName());

            var projects = projectFaker.Generate(2);

            var taskFaker = new Faker<QueryTask.Task>().StrictMode(true)
                                                       .RuleFor(u => u.Id, f => Guids[taskPosition++].ToString())
                                                       .RuleFor(u => u.Description, f => f.Lorem.Sentence(25))
                                                       .RuleFor(u => u.LongDescription, f => f.Lorem.Paragraphs(2))
                                                       .RuleFor(u => u.CreatedDate, f => f.Date.Past().ToUnixTime())
                                                       .RuleFor(u => u.DeadLine, f => f.Date.Soon(f.Random.Number(1, 25)).ToUnixTime())
                                                       .RuleFor(u => u.Assignee, f => users[f.Random.Number(0, 2)])
                                                       .RuleFor(u => u.Reporter, f => users[f.Random.Number(0, 2)])
                                                       .RuleFor(u => u.Project, f => projects.FirstOrDefault(i => i.Id == Guids[f.Random.Number(0, 1)].ToString()))
                                                       .RuleFor(u => u.Status, f => ((TaskStatusEnum)f.Random.Number(1, 4)).ToString());

            var tasks = taskFaker.Generate(2);

            var manager = new InMemoryManager<QueryTask.Task>(tasks);

            return manager;
        }

        public static IUserRepository GetUserRepository(IEqualityComparer<User> comparable = null, Guid[] ids = null)
        {
            if (comparable == null) comparable = new UserComparer();
            if (ids == null) ids = Guids;

            var repository = new Mock<IUserRepository>();

            var facebookProject =
                new Project()
                {
                    Id = ids[4],
                    Description = "Facebook",
                    UserProjects = new[]
                        {
                            new UserProject() { Projectid = ids[4], UserId = ids[1] },
                            new UserProject() { Projectid = ids[4], UserId = ids[2] }
                        }
                };

            var values = new List<User>()
            {
                new User() {Id = ids[0], Name = "Bill Gates", Email = "bill@gates.com", UserProjects = new[] { new UserProject() { Projectid = ids[3], UserId = ids[0] } } },
                new User() {Id = ids[1], Name = "Mark Zuckerberg", Email = "mark@zuckerberg.com", UserProjects = new[] { new UserProject() { Project = facebookProject, Projectid = ids[4], UserId = ids[1] } }},
                new User() {Id = ids[2], Name = "Carl", Email = "carl@facebook.com", UserProjects = new[] { new UserProject() { Project = facebookProject, Projectid = ids[4], UserId = ids[2] } }}
            };

            var list = new HashSet<User>(values, comparable);

            repository
                .Setup(i => i.Add(It.IsAny<User>()))
                .Callback<User>(i => { if (!list.Add(i)) throw new Exception(); });

            repository
                .Setup(i => i.GetById(It.IsAny<Guid>()))
                .Returns((Guid param) => Thread.Task.FromResult(list.FirstOrDefault(i => i.Id == param)));

            repository
                .Setup(i => i.Update(It.IsAny<User>()))
                .Callback<User>(i =>
                {
                    if (!list.Any(j => j.Id == i.Id)) throw new Exception();

                    list.RemoveWhere(j => j.Id == i.Id);
                    if (!list.Add(i)) throw new Exception();
                });

            return repository.Object;
        }

        public static IProjectRepository GetProjectRepository(IEqualityComparer<Project> comparable = null, Guid[] ids = null)
        {
            if (comparable == null) comparable = new ProjectComparer();
            if (ids == null) ids = Guids;

            var repository = new Mock<IProjectRepository>();

            var users = new List<User>()
            {
                new User() {Id = ids[0], Name = "Bill Gates", Email = "bill@gates.com"},
                new User() {Id = ids[1], Name = "Mark Zuckerberg", Email = "mark@zuckerberg.com"},
                new User() {Id = ids[2], Name = "Carl", Email = "carl@facebook.com"}
            };

            var tasks = new List<Task>()
            {
                new Task()
                {
                    Id = ids[5],
                    Reporter = users[0],
                    Assignee = users[0],
                    CreatedDate = DateTime.UtcNow,
                    DeadLine = DateTime.UtcNow.AddDays(2),
                    Description = "Fix Bug #222",
                    StatusId = (int)TaskStatusEnum.IN_PROGRESS
                },
                new Task()
                {
                    Id = ids[6],
                    Reporter = users[1],
                    Assignee = users[1],
                    CreatedDate = DateTime.UtcNow,
                    DeadLine = DateTime.UtcNow.AddDays(3),
                    Description = "Fix Bug #548",
                    StatusId = (int)TaskStatusEnum.BACKLOG
                }
            };

            var values = new List<Project>()
            {
                new Project() 
                {
                    Id = ids[3],
                    Description = "Windows",
                    UserProjects = new [] 
                    { 
                        new UserProject() { Projectid = ids[3], User = users[0] }
                    },
                    Tasks = new [] { tasks[0] }
                },
                new Project() 
                {
                    Id = ids[4],
                    Description = "Facebook",
                    UserProjects = new [] 
                    { 
                        new UserProject() { Projectid = ids[4], User = users[1] },
                        new UserProject() { Projectid = ids[4], User = users[2] }
                    },
                    Tasks = new [] { tasks[1] }
                }
            };

            tasks[0].Project = values[0];
            tasks[1].Project = values[1];

            var list = new HashSet<Project>(values, comparable);

            repository
                .Setup(i => i.Add(It.IsAny<Project>()))
                .Callback<Project>(i => { if (!list.Add(i)) throw new Exception(); });

            repository
                .Setup(i => i.GetById(It.IsAny<Guid>()))
                .Returns((Guid param) => Thread.Task.FromResult(list.FirstOrDefault(i => i.Id == param)));

            repository
                .Setup(i => i.Update(It.IsAny<Project>()))
                .Callback<Project>(i =>
                {
                    if (!list.Any(j => j.Id == i.Id)) throw new Exception();

                    list.RemoveWhere(j => j.Id == i.Id);
                    if (!list.Add(i)) throw new Exception();
                });

            return repository.Object;
        }

        public static ITaskRepository GetTaskRepository(IEqualityComparer<Task> comparable = null, Guid[] ids = null)
        {
            if (comparable == null) comparable = new TaskComparer();
            if (ids == null) ids = Guids;

            var repository = new Mock<ITaskRepository>();

            var users = new List<User>()
            {
                new User() {Id = ids[0], Name = "Bill Gates", Email = "bill@gates.com"},
                new User() {Id = ids[1], Name = "Mark Zuckerberg", Email = "mark@zuckerberg.com"},
                new User() {Id = ids[2], Name = "Carl", Email = "carl@facebook.com"}
            };

            var projects = new List<Project>()
            {
                new Project()
                {
                    Id = ids[3],
                    Description = "Windows",
                    UserProjects = new []
                    {
                        new UserProject() { Projectid = ids[3], User = users[0] }
                    }
                },
                new Project()
                {
                    Id = ids[4],
                    Description = "Facebook",
                    UserProjects = new []
                    {
                        new UserProject() { Projectid = ids[4], User = users[1] },
                        new UserProject() { Projectid = ids[4], User = users[2] }
                    }
                }
            };

            var values = new List<Task>()
            {
                new Task()
                {
                    Id = ids[5],
                    Reporter = users[0],
                    Assignee = users[0],
                    CreatedDate = DateTime.UtcNow,
                    DeadLine = DateTime.UtcNow.AddDays(2),
                    Description = "Fix Bug #222",
                    Project = projects[0],
                    StatusId = (int)TaskStatusEnum.IN_PROGRESS
                },
                new Task()
                {
                    Id = ids[6],
                    Reporter = users[1],
                    Assignee = users[1],
                    CreatedDate = DateTime.UtcNow,
                    DeadLine = DateTime.UtcNow.AddDays(3),
                    Description = "Fix Bug #548",
                    Project = projects[1],
                    StatusId = (int)TaskStatusEnum.BACKLOG
                }
            };

            projects[0].Tasks.Add(values[0]);
            projects[1].Tasks.Add(values[1]);

            var list = new HashSet<Task>(values, comparable);

            repository
                .Setup(i => i.Add(It.IsAny<Task>()))
                .Callback<Task>(i => { if (!list.Add(i)) throw new Exception(); });

            repository
                .Setup(i => i.GetById(It.IsAny<Guid>()))
                .Returns((Guid param) => Thread.Task.FromResult(list.FirstOrDefault(i => i.Id == param)));

            repository
                .Setup(i => i.Update(It.IsAny<Task>()))
                .Callback<Task>(i =>
                {
                    if (!list.Any(j => j.Id == i.Id)) throw new Exception();

                    list.RemoveWhere(j => j.Id == i.Id);
                    if (!list.Add(i)) throw new Exception();
                });

            return repository.Object;
        }


    }
}
