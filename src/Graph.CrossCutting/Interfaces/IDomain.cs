using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.CrossCutting.Interfaces
{
    public interface IDomain
    {
        Guid Id { get; }
        void Validate();
    }
}
