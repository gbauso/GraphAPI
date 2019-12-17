using AutoMapper;
using Command = Graph.Infrastructure.Database.Command.Model;
using Query = Graph.Infrastructure.Database.Query.UserSchema;
using System.Linq;

namespace Graph.Domain.Service.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, Command.User>();

            CreateMap<Command.User, User>()
                .ForMember(i => i.Projects, j => j.MapFrom(m => m.UserProjects.Select(s => s.Project)));

            CreateMap<Command.User, Query.User>()
                .ForMember(i => i.Projects, j => j.MapFrom(m => m.UserProjects.Select(s => s.Project)));

            CreateMap<User, Query.User>()
                .ForMember(i => i.Projects, j => j.MapFrom(m => m.Projects));

            CreateMap<Project, Query.UserProject>();
        }
    }
}
