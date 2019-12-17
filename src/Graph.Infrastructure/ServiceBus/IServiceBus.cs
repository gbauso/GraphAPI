using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Infrastructure.ServiceBus
{
    public interface IServiceBus 
    {
        Task SendMessage(Message message);
    }
}
