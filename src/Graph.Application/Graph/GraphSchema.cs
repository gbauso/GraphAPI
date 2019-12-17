using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Graph.CrossCutting.Extensions.GraphQL;
using Graph.Application.Graph.User.Types.Query;
using Graph.Application.Graph.Query;
using Graph.Application.Graph.Mutation;

namespace Graph.Application.Graph
{
    public class GraphSchema : Schema
    {
        public GraphSchema(IDependencyResolver resolver) : base(resolver)
        {
            this.AddQuery<UserQuery>();
            this.AddQuery<ProjectQuery>();
            this.AddQuery<TaskQuery>();

            this.AddMutation<UserMutation>();
            this.AddMutation<ProjectMutation>();
            this.AddMutation<TaskMutation>();
        }
    }
}
