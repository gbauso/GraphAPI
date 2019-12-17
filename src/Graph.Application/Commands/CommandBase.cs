using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Commands
{
    public abstract class CommandBase<T> : IRequest<T>
    {
    }
}
