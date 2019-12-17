using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Common
{
    public class MutationResultType : ObjectGraphType 
    {
        public MutationResultType()
        {
            Field<bool>("result", (a) => true);
        }
    }
}
