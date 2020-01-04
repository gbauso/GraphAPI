using Graph.Application.Graph.Common;
using Graph.CrossCutting.Extensions;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Project.Types.Input
{
    public class TaskFilterType : InputObjectGraphType
    {
        public TaskFilterType()
        {
            Field<FilterType>("description");
            Field<DateFilterType>("deadLine");
            Field<FilterType>("assignee");
            Field<FilterType>("reporter");
            Field<EnumFilterType>("status");
        }
    }
}
