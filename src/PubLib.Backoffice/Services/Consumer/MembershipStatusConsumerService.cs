using PubLib.Messaging.RabbitMQ.Clients.Consumer;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Channels;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Membership;
using System.Threading.Channels;

namespace PubLib.Backoffice.WebApp.Services.Consumer
{
    public class MembershipStatusConsumerService : BackgroundService
    {
        private readonly MembershipStatusConsumer _consumer;

        private readonly ILogger<MembershipStatusConsumerService> _logger;

        private readonly MessageService _messageService;

        private readonly IMembershipApplicationChannelFactory _membershipApplicationChannelFactory;

        private readonly Channel<MembershipApplicationReceivedEventArgs> _membershipApplicationChannel;

        public MembershipStatusConsumerService(
                                        ILogger<MembershipStatusConsumerService> logger,
                                        IMembershipApplicationChannelFactory membershipApplicationChannelFactory,
                                        MembershipStatusConsumer consumer,
                                        MessageService messageService)
        {
            _logger = logger;
            _consumer = consumer;
            _membershipApplicationChannelFactory = membershipApplicationChannelFactory;
            _membershipApplicationChannel = membershipApplicationChannelFactory.GetChannel();
            _messageService = messageService;
        }

        public override void Dispose()
        {
            _membershipApplicationChannelFactory.Dispose();
            _consumer.Dispose();
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting {nameof(MembershipStatusConsumerService)}.");
            await Task.WhenAll(_consumer.ConsumeAsync(stoppingToken), ProcessMembershipApplicationsAsync(stoppingToken));
        }

        private async Task ProcessMembershipApplicationsAsync(CancellationToken cancellationToken)
        {
            try
            {
                await foreach (var MembershipApplication in _membershipApplicationChannel.Reader.ReadAllAsync(cancellationToken))
                {
                    HandleMembershipApplicationAsync(MembershipApplication);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Processing membership application was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing membership application.");
            }
        }

        private void HandleMembershipApplicationAsync(MembershipApplicationReceivedEventArgs membershipApplication)
        {
            try
            {
                string message = $"Membership applied: {membershipApplication.Message.Person.LastName}.";
                _logger.LogInformation(message);
                _messageService.AddMessage(message, membershipApplication.QueueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while handling a membership application.");
                throw;
            }
        }
    }
}
