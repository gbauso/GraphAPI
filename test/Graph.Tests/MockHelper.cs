using AutoMapper;
using Graph.Domain.Service.Mappings;
using Graph.Infrastructure.Database.Command.Interfaces;
using Graph.Infrastructure.Database.Command.Model;
using Graph.Infrastructure.ServiceBus;
using Moq;
using System;
using System.Collections.Generic;
using Thread = System.Threading.Tasks;
using System.Text;
using System.Linq;
using Graph.CrossCutting;

namespace Graph.Tests
{
    public class MockHelper
    {
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

        public static IUserRepository GetUserRepository(IEqualityComparer<User> comparable, Guid[] ids)
        {
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

        public static IProjectRepository GetProjectRepository(IEqualityComparer<Project> comparable, Guid[] ids)
        {
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

        public static ITaskRepository GetTaskRepository(IEqualityComparer<Task> comparable, Guid[] ids)
        {
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
