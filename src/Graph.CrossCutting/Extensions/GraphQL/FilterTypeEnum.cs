using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.CrossCutting.Extensions.GraphQL
{
    public enum FilterTypeEnum
    {
        GREATER = 'g', // >
        LESS = 'l', // <
        NOT = 'n', // !
        EQUAL = 'e', // =
        CONTAIN = 'c', // Contains(@0)

        // GREATER and EQUAL - >=
        // LESS and EQUAL - <=
        // NOT and EQUAL - !=
    }
}
