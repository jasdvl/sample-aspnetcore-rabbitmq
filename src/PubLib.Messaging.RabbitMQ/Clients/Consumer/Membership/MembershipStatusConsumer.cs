using Microsoft.Extensions.Options;
using PubLib.Messaging.RabbitMQ.Clients.Connection;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Base;
using PubLib.Messaging.RabbitMQ.Clients.DTOs;
using PubLib.Messaging.RabbitMQ.Configuration;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace PubLib.Messaging.RabbitMQ.Clients.Consumer.Membership
{
    public class MembershipStatusConsumer : ConsumerBase
    {
        public event EventHandler<MembershipApplicationReceivedEventArgs>? MembershipApplied;

        public event EventHandler<MembershipApplicationReceivedEventArgs>? MembershipCancelled;

        public MembershipStatusConsumer(
                                IRabbitMQConnectionFactory connectionFactory,
                                IOptions<RabbitMQOptions> options)
            : base(connectionFactory, options, nameof(MembershipStatusConsumer))
        {
        }

        public override async Task ConsumeAsync(CancellationToken stoppingToken)
        {
            await base.ConsumeAsync(stoppingToken);

            // Add membership specific tasks here
        }

        protected override async Task HandleReceived(object? model, BasicDeliverEventArgs ea, string queueName)
        {
            var body = ea.Body.ToArray();
            var membershipMessage = JsonSerializer.Deserialize<MembershipApplicationDto>(body);

            if (ea.RoutingKey == "membership.status.applied")
            {
                OnMembershipApplied(new MembershipApplicationReceivedEventArgs(membershipMessage!, queueName));
            }
            else if (ea.RoutingKey == "membership.status.cancelled")
            {
                OnMembershipCancelled(new MembershipApplicationReceivedEventArgs(membershipMessage!, queueName));
            }
        }

        private void OnMembershipApplied(MembershipApplicationReceivedEventArgs e)
        {
            MembershipApplied?.Invoke(this, e);
        }

        private void OnMembershipCancelled(MembershipApplicationReceivedEventArgs e)
        {
            MembershipCancelled?.Invoke(this, e);
        }
    }
}
