using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public class Requester: IRequestClass
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _username;
        private IConnection _connection;

        public Requester(RabitMqSettings settings)
        {
            _hostname = settings.HostName;
            _queueName = settings.QueueName;
            _username = settings.UserName;
            _password = settings.Password;
            CreateConnection();
        }

        public void Sender(string request)
        {
            if (ConnectionExists())
            {
                // Publisher to RabbitMq Server
                using (var channel = _connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "Publish-Subscrib", type: ExchangeType.Direct, true);

                    channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    var body = Encoding.UTF8.GetBytes(request);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = false;

                    channel.BasicPublish(exchange: "Publish-Subscrib", routingKey: "Hello", basicProperties: properties, body: body);
                }
            }
        }

        // RabbitMq Create Connection 
        private void CreateConnection()
        {
            try
            {
                
                var factory = new ConnectionFactory
                    {
                        HostName = _hostname,
                        UserName = _username,
                        Password = _password,
                        Port = Protocols.DefaultProtocol.DefaultPort,
                        VirtualHost = "/",
                        ContinuationTimeout = new TimeSpan(10, 0, 0, 0),
                    };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        // Close the RabbitMq Connection
        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}
