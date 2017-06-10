using Contracts;
using MassTransit;
using System;
using System.Web.Mvc;

namespace Producer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISendEndpoint _sendEndpoint;

        public HomeController()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost"), hst =>
                {
                    hst.Username("guest");
                    hst.Password("guest");
                });
            });

            var sendToUri = new Uri("rabbitmq://localhost/order_queue");
            _sendEndpoint = bus.GetSendEndpoint(sendToUri).Result;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateOrder(int orderId, string userName)
        {
            _sendEndpoint.Send<Order>(new Order
            {
                OrderId = orderId,
                Username = userName
            }).Wait();

            TempData["OrderCreated"] = true;
            TempData["OrderId"] = orderId;

            return RedirectToAction("Index");
        }
    }
}