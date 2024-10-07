using PubLib.Messaging.RabbitMQ.Setup;

namespace PubLib.Server.Services
{
    public class RabbitMQExchangeSetupService : IHostedService
    {
        private readonly ExchangeSetupService _exchangeSetupService;
        private readonly ILogger<RabbitMQExchangeSetupService> _logger;

        public RabbitMQExchangeSetupService(ExchangeSetupService exchangeSetupService,
            ILogger<RabbitMQExchangeSetupService> logger)
        {
            _exchangeSetupService = exchangeSetupService ?? throw new ArgumentNullException(nameof(exchangeSetupService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting RabbitMQ exchange setup...");
            _exchangeSetupService.SetupExchanges();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RabbitMQ exchange setup service is stopping");
            return Task.CompletedTask;
        }
    }
}
