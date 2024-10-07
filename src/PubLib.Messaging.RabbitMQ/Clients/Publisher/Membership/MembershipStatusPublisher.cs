using Microsoft.Extensions.Options;
using PubLib.Messaging.RabbitMQ.Clients.Connection;
using PubLib.Messaging.RabbitMQ.Clients.DTOs;
using PubLib.Messaging.RabbitMQ.Clients.Publisher.Base;
using PubLib.Messaging.RabbitMQ.Configuration;
using System.Text.Json;

namespace PubLib.Messaging.RabbitMQ.Clients.Publisher.Membership
{
    public class MembershipStatusPublisher : PublisherBase
    {
        public MembershipStatusPublisher(IRabbitMQConnectionFactory connection, IOptions<RabbitMQOptions> options)
            : base(connection, options, nameof(MembershipStatusPublisher))
        {
        }

        public void PublishNewMembership(MembershipApplicationDto message)
        {
            var membershipMessage = JsonSerializer.Serialize(message);
            base.Publish("membership.status.applied", membershipMessage);
        }

        public void PublishCancelledMembership(MembershipApplicationDto message)
        {
            var membershipMessage = JsonSerializer.Serialize(message);
            base.Publish("membership.status.cancelled", membershipMessage);
        }
    }
}
