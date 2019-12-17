using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.Extensions.Options;
using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Infrastructure.ServiceBus
{
    public class MassTransitSB : IServiceBus
    {
        private readonly IBusControl _Bus;
        private readonly ISubscribe _Subscriber;

        public MassTransitSB(IOptions<BusConfiguration> configuration, ISubscribe subscribe)
        {
            _Subscriber = subscribe;
            _Bus = GetBusInstance(configuration);
            _Bus.StartAsync().Wait();
        }

        private IBusControl GetBusInstance(IOptions<BusConfiguration> configuration)
        {
            var config = configuration.Value;

            IBusControl bus = null;

            switch (config.Transport)
            {
                case BusTransport.RABBITMQ:
                    bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        var host = cfg.Host(new Uri(config.Url), c =>
                        {
                            c.Username(config.Credentials.Username);
                            c.Password(config.Credentials.Password);
                        });

                        cfg.ReceiveEndpoint(host, config.Queue, e =>
                        {
                            e.Handler<Message>(ctx => _Subscriber.HandleMessage(ctx.Message));

                            EndpointConvention.Map<Message>(e.InputAddress);
                        });

                    });
                    break;
                case BusTransport.AZURE:
                    bus = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
                    {
                        var host = cfg.Host(config.Url, c =>
                        {
                            c.SharedAccessSignature(s =>
                            {
                                s.KeyName = config.Credentials.KeyName;
                                s.SharedAccessKey = config.Credentials.SharedAccessKey;
                                s.TokenTimeToLive = TimeSpan.FromDays(config.Credentials.TokenTimeToLive);
                                s.TokenScope = TokenScope.Namespace;
                            });
                        });

                        cfg.ReceiveEndpoint(host, config.Queue, e =>
                        {
                            e.Handler<Message>(ctx => _Subscriber.HandleMessage(ctx.Message));

                            EndpointConvention.Map<Message>(e.InputAddress);
                        });
                    });
                    break;
                default:
                    bus = Bus.Factory.CreateUsingInMemory(cfg =>
                    {
                        cfg.ReceiveEndpoint(config.Queue, e =>
                        {
                            e.Handler<Message>(ctx => _Subscriber.HandleMessage(ctx.Message));

                            EndpointConvention.Map<Message>(e.InputAddress);
                        });
                    });
                    break;
            }

            return bus;
        }

        public Task SendMessage(Message message)
        {
            return _Bus.Send(message);
        }


    }
}
