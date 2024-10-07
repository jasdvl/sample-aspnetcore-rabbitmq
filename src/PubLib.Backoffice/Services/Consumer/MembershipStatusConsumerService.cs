using PubLib.Messaging.RabbitMQ.Clients.Consumer;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Membership;

namespace PubLib.Backoffice.WebApp.Services.Consumer
{
    public class MembershipStatusConsumerService : BackgroundService
    {
        private readonly MembershipStatusConsumer _consumer;

        private readonly ILogger<MembershipStatusConsumerService> _logger;

        private readonly MessageService _messageService;

        public MembershipStatusConsumerService(
                                        MembershipStatusConsumer consumer,
                                        ILogger<MembershipStatusConsumerService> logger,
                                        MessageService messageService)
        {
            _consumer = consumer;
            _logger = logger;
            _messageService = messageService;

            _consumer.MembershipApplied += OnMembershipApplied;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting {nameof(MembershipStatusConsumerService)}.");
            await _consumer.ConsumeAsync(stoppingToken);
        }

        private void OnMembershipApplied(object? sender, MembershipApplicationReceivedEventArgs e)
        {
            string message = $"Membership applied: {e.Message.Person.FirstName} {e.Message.Person.LastName}.";
            _logger.LogInformation(message);

            _messageService.AddMessage(message, e.QueueName);
        }
    }
}
