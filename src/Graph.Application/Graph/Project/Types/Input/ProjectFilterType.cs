using Graph.Application.Graph.Common;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Project.Types.Input
{
    public class ProjectFilterType : InputObjectGraphType
    {
        public ProjectFilterType()
        {
            Field<FilterType>("description");
            Field<FilterType>("longDescription");
        }
    }
}
