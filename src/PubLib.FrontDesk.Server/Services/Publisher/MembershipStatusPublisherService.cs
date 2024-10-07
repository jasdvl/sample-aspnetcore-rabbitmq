using PubLib.Messaging.RabbitMQ.Clients.DTOs;
using PubLib.Messaging.RabbitMQ.Clients.Publisher.Membership;

namespace PubLib.Server.Services.Publisher
{
    public class MembershipStatusPublisherService
    {
        private readonly MembershipStatusPublisher _publisher;
        private readonly ILogger<MembershipStatusPublisherService> _logger;

        public MembershipStatusPublisherService(MembershipStatusPublisher publisher, ILogger<MembershipStatusPublisherService> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public void PublishNewMembership(MembershipApplicationDto message)
        {
            _logger.LogInformation($"Publishing new membership message with {nameof(MembershipStatusPublisher)}.");
            _publisher.PublishNewMembership(message);
        }

        public void PublishCancelMembership(MembershipApplicationDto message)
        {
            _logger.LogInformation($"Publishing new membership message with {nameof(MembershipStatusPublisher)}.");
            _publisher.PublishCancelledMembership(message);
        }
    }
}
