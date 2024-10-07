using Microsoft.Extensions.Options;
using PubLib.Messaging.RabbitMQ.Clients.Connection;
using PubLib.Messaging.RabbitMQ.Configuration;
using RabbitMQ.Client;
using System.Text;

namespace PubLib.Messaging.RabbitMQ.Clients.Publisher.Base
{
    public abstract class PublisherBase
    {
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly ExchangeConfig _exchangeConfig;

        protected PublisherBase(IRabbitMQConnectionFactory connectionFactory, IOptions<RabbitMQOptions> options, string publisherName)
        {
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            // Extrahiere die Exchange-Konfiguration
            var exchangeConfig = options.Value.Exchanges
                .FirstOrDefault(e => e.Publishers.Contains(publisherName));

            if (exchangeConfig == null)
            {
                throw new ArgumentException($"Exchange configuration for publisher {publisherName} not found.");
            }

            _exchangeConfig = exchangeConfig;

            // Deklariere die Exchange
            _channel.ExchangeDeclare(
                exchange: _exchangeConfig.Name,
                type: _exchangeConfig.Type,
                durable: _exchangeConfig.Durable,
                autoDelete: _exchangeConfig.AutoDelete
            );
        }

        public virtual void Publish(string routingKey, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            // Sende die Nachricht an die Exchange mit dem angegebenen Routing-Key
            _channel.BasicPublish(
                exchange: _exchangeConfig.Name,
                routingKey: routingKey,
                basicProperties: null,
                body: body);

            Console.WriteLine($"Published message: {message}, exchange: {_exchangeConfig.Name}, routing key: {routingKey}.");
        }
    }
}
