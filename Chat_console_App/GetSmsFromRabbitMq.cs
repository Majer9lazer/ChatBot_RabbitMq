using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chat_console_App
{
    class GetSmsFromRabbitMq
    {
        public string HostName = "192.168.111.199";
        public string UserNameOrPassword = "shag";
        public GetSmsFromRabbitMq()
        {
            ConnectionFactory = new ConnectionFactory()
            {
                HostName = HostName,
                UserName = UserNameOrPassword,
                Password = UserNameOrPassword,
                VirtualHost = "/"
            };
        }
        public static IConnectionFactory ConnectionFactory;
       
      
        public void RunWorkerProcessForSmss(string queueName = "smss_to_send")
        {

            using (var connection = ConnectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                Console.WriteLine(" [*] Opened Channel");

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                Console.WriteLine($" [*] Waiting for messages from queue {queueName}");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var messageDeserialized = JsonConvert.DeserializeObject(message);



                   

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($" [x] Operation Completed {messageDeserialized}");
                    Console.ForegroundColor = ConsoleColor.Green;

                    // ReSharper disable once AccessToDisposedClosure
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };
                channel.BasicConsume(queue: queueName,
                    autoAck: false,
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

    }
}
