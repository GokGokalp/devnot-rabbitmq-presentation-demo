using GreenPipes;
using MassTransit;
using System;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
           {
               var host = cfg.Host(new Uri("rabbitmq://localhost"), hst =>
               {
                   hst.Username("guest");
                   hst.Password("guest");
               });

               //cfg.UseRetry(retryConfig =>
               //{
               //    retryConfig.Incremental(3, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1));
               //});

               cfg.ReceiveEndpoint(host, "order_queue", e =>
               {
                   e.Consumer<OrderConsumer>();
               });
           });

            bus.StartAsync().Wait();
        }
    }
}