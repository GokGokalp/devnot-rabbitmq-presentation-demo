using Newtonsoft.Json;
using Producer.Models;
using RabbitMQ.Client;
using System.Text;
using System.Web.Mvc;

namespace Producer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult CreateOrder(int orderId, string userName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "order_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                Order order = new Order() { OrderId = orderId, Username = userName };

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(order));

                channel.BasicPublish(exchange: "",
                                     routingKey: "order_queue",
                                     basicProperties: null,
                                     body: body);

                TempData["OrderCreated"] = true;
                TempData["OrderId"] = orderId;

                return RedirectToAction("Index");
            }
        }
    }
}