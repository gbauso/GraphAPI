using System.Security.Claims;

namespace Graph.API
{
    public class GraphQLUserContext
    {
        public ClaimsPrincipal User { get; set; }
    }
}
