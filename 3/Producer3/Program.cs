using RabbitMQ.Client;
using System;
using System.Text;

namespace Producer3
{
	class Program
	{
		public static void Main(string[] args)
		{
			var message = GetMessage(args);
			for (var i = 0; i < 10; i++)
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
				channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

				var body = Encoding.UTF8.GetBytes(message);
				channel.BasicPublish(exchange: "logs", routingKey: "", basicProperties: null, body: body);
				Console.WriteLine(" [x] Sent {0}", message);
			}

			//Console.WriteLine(" Press [enter] to exit.");
			//Console.ReadLine();
		}

		private static string GetMessage(string[] args)
		{
			return ((args.Length > 0) ? string.Join(" ", args) : "info: Hello World!");
		}
	}
}
