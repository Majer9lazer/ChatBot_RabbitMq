using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_console_App
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMqMiddlewareBusService busService= new RabbitMqMiddlewareBusService(); 
            GetSmsFromRabbitMq getSms   = new GetSmsFromRabbitMq();
            Console.Write("Your name here : ");
            string From = Console.ReadLine();
            Console.Write("Write to : ");
            string WriteTo=Console.ReadLine();
            getSms.RunWorkerProcessForSmss(WriteTo);

            while (true)
            {
                Console.WriteLine("Your Message");
                string message = Console.ReadLine();  
                busService.PublishMessage(message,From);
            }
           
        }
    }
}
