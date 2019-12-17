using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Common
{
    public class PaginationType : InputObjectGraphType<Pagination>
    {
        public PaginationType()
        {
            Field(i => i.Skip);
            Field(i => i.Take);
        }
    }
}
