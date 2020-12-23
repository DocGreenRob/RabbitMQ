using RabbitMQ.Client;
using System;
using System.Text;

namespace Producer
{
	class Program
	{
		static void Main(string[] args)
		{
			SendMessageToQueue(args);
		}

		private static void SendMessageToQueue(string[] args)
		{
			var message = GetMessage(args);
			var factory = new ConnectionFactory() { HostName = "localhost" };

			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: "hello",
								 durable: false,
								 exclusive: false,
								 autoDelete: false,
								 arguments: null);

				var body = Encoding.UTF8.GetBytes(message);

				channel.BasicPublish(exchange: "",
									 routingKey: "hello",
									 basicProperties: null,
									 body: body);
				Console.WriteLine(" [x] Sent {0}", message);

				Console.WriteLine(" Press [enter] to exit.");
				Console.ReadLine();
			}
		}
		static string GetMessage(string[] args)
		{
			return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!22");
		}
	}
}
