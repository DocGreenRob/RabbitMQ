using RabbitMQ.Client;
using System;
using System.Text;

namespace Producer2
{
	class Program
	{
		private readonly static string queueName = "task_queue-2";
		static void Main(string[] args)
		{
			var message = GetMessage(args);
			for(var i=0; i<10; i++)
			{
				PublishMessage($"[{i + 1}] :: {message}");
			}
		}

		private static void PublishMessage(string message)
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };

			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				var properties = channel.CreateBasicProperties();
				properties.Persistent = true;

				var body = Encoding.UTF8.GetBytes(message);

				channel.BasicPublish(exchange: "",
									 routingKey: queueName,
									 basicProperties: properties,
									 body: body);
				Console.WriteLine(" [x] Sent {0}", message);

				//Console.WriteLine(" Press [enter] to exit.");
				//Console.ReadLine();
			}
		}
		static string GetMessage(string[] args)
		{
			return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!!!");
		}
	}
}
