using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Domain.Model
{
    public enum DomainState
    {
        NEW = 1,
        FROM_DB,
        ADD_RELATION,
        REMOVE_RELATION
    }
}
