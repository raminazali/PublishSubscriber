using System.Text.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ
{
    public class RabbitListener : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;

        public RabbitListener(RabitMqSettings settings)
        {
            _hostname = settings.HostName;
            _queueName = settings.QueueName;
            _username = settings.UserName;
            _password = settings.Password;

            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            
            _channel = _connection.CreateModel();
        }

        // Consuming the Queue
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string ExchangeName = "Publish-Subscrib";

            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct, durable:true );


            _channel.QueueBind(queue: _queueName,
                          exchange: ExchangeName,
                          routingKey: "Hello");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, DeliveryEvent) =>
            {
                var content = Encoding.UTF8.GetString(DeliveryEvent.Body.ToArray());
                ConsumerMessages(content);
                _channel.BasicAck(DeliveryEvent.DeliveryTag, false);
            };
            

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }
        /// <summary>
        ///  After Listening the Queue This Method Will be Work
        /// </summary>
        /// <param name="content"></param>
        public virtual void ConsumerMessages(string content)
        {

        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }

    public class RabitMqSettings
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
    }
}
