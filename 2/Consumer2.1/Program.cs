﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Consumer2._1
{
	class Program
	{
		private readonly static string queueName = "task_queue-2";
		static void Main(string[] args)
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };

			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: queueName,
									 durable: true,
									 exclusive: false,
									 autoDelete: false,
									 arguments: null);

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) =>
				{
					var body = ea.Body.ToArray();
					var message = Encoding.UTF8.GetString(body);
					Console.WriteLine(" [x] Received {0}", message);

					Thread.Sleep(5000);
					channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
					Console.WriteLine(" [x] Done");
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
