using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Chat_console_App
{
    public class RabbitMqMiddlewareBusService
    {
        public static string HostName = "192.168.111.199";
        public static string UserNameOrPassword = "shag";
        private readonly IConnectionFactory _connectionFactory;
        public RabbitMqMiddlewareBusService()
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = HostName,
                UserName = UserNameOrPassword,
                Password = UserNameOrPassword,
                VirtualHost = "/"
            };
        }
        public void PublishMessage<T>(T message, string queueName) where T : class
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                    routingKey: queueName,
                    basicProperties: properties,
                    body: body);
            }
        }
    }
}
