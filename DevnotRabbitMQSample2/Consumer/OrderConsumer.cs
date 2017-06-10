using Contracts;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Consumer
{
    public class OrderConsumer : IConsumer<Order>
    {
        public async Task Consume(ConsumeContext<Order> context)
        {
            var command = context.Message;

            //throw new Exception("Error!");

            await Console.Out.WriteLineAsync($"[x] Order received. Order Id: {command.OrderId}, Username: {command.Username}");
        }
    }
}