using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PubLib.Messaging.RabbitMQ.Clients.Connection;
using PubLib.Messaging.RabbitMQ.Configuration;
using RabbitMQ.Client;

namespace PubLib.Messaging.RabbitMQ.Setup;

public class ExchangeSetupService
{
    private readonly IRabbitMQConnectionFactory _connectionFactory;
    private readonly RabbitMQOptions _rabbitMQOptions;
    private readonly ILogger<ExchangeSetupService> _logger;

    public ExchangeSetupService(IRabbitMQConnectionFactory connectionFactory,
                                IOptions<RabbitMQOptions> options,
                                ILogger<ExchangeSetupService> logger)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _rabbitMQOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void SetupExchanges()
    {
        using (var connection = _connectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            foreach (var exchange in _rabbitMQOptions.Exchanges)
            {
                try
                {
                    channel.ExchangeDeclare(
                        exchange: exchange.Name,
                        type: exchange.Type,
                        durable: exchange.Durable,
                        autoDelete: exchange.AutoDelete
                    );

                    _logger.LogInformation("Declared exchange: {ExchangeName}", exchange.Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to declare exchange: {ExchangeName}", exchange.Name);
                    throw;
                }
            }
        }

        _logger.LogInformation("RabbitMQ exchanges setup completed successfully.");
    }
}
