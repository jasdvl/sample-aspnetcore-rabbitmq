using PubLib.Messaging.RabbitMQ.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace PubLib.Messaging.RabbitMQ.Clients.Connection
{
    public class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
    {
        private readonly RabbitMQOptions _rabbitMQOptions;
        private readonly ILogger<RabbitMQConnectionFactory> _logger;

        public RabbitMQConnectionFactory(IOptions<RabbitMQOptions> options, ILogger<RabbitMQConnectionFactory> logger)
        {
            _rabbitMQOptions = options.Value;
            _logger = logger;
        }

        public IConnection CreateConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQOptions.HostName,
                Port = _rabbitMQOptions.Port,
                UserName = _rabbitMQOptions.UserName,
                Password = _rabbitMQOptions.Password
            };

            return factory.CreateConnection();
        }
    }
}
