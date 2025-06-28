// using Microsoft.EntityFrameworkCore.Metadata;
// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
// using System;
// using System.Text;

// namespace ConfigWebApi.Helpers
// {
//     public class RabbitMQHelper : IDisposable
//     {
//         private readonly IConnection _connection;
//         private readonly IModel _channel;
//         private readonly string _queueName;

//         public RabbitMQHelper(string hostname, string queueName)
//         {
//             _queueName = queueName;
//             var factory = new ConnectionFactory() { HostName = hostname };
//             _connection = factory.CreateConnection(); // Sync connection açılıyor
//             _channel = _connection.CreateModel();     // Sync kanal açılıyor

//             _channel.QueueDeclare(queue: _queueName,
//                                  durable: false,
//                                  exclusive: false,
//                                  autoDelete: false,
//                                  arguments: null);
//         }

//         public void Publish(string message)
//         {
//             var body = Encoding.UTF8.GetBytes(message);
//             _channel.BasicPublish(exchange: "",
//                                  routingKey: _queueName,
//                                  basicProperties: null,
//                                  body: body);
//         }

//         public void Consume(Action<string> onMessageReceived)
//         {
//             var consumer = new EventingBasicConsumer(_channel);
//             consumer.Received += (model, ea) =>
//             {
//                 var body = ea.Body.ToArray();
//                 var message = Encoding.UTF8.GetString(body);
//                 onMessageReceived(message);
//             };

//             _channel.BasicConsume(queue: _queueName,
//                                  autoAck: true,
//                                  consumer: consumer);
//         }

//         public void Dispose()
//         {
//             _channel?.Close();
//             _connection?.Close();
//         }
//     }
// }
