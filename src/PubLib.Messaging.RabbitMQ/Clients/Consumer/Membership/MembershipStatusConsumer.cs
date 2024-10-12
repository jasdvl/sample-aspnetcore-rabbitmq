using Microsoft.Extensions.Options;
using PubLib.Messaging.RabbitMQ.Clients.Connection;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Base;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Channels;
using PubLib.Messaging.RabbitMQ.Clients.DTOs;
using PubLib.Messaging.RabbitMQ.Configuration;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Threading.Channels;

namespace PubLib.Messaging.RabbitMQ.Clients.Consumer.Membership
{
    public class MembershipStatusConsumer : ConsumerBase
    {
        private readonly Channel<MembershipApplicationReceivedEventArgs> _membershipApplicationChannel;

        //private readonly Channel<MembershipApplicationReceivedEventArgs> _membershipCancelMessageChannel;

        public MembershipStatusConsumer(
                                IRabbitMQConnectionFactory connectionFactory,
                                IMembershipApplicationChannelFactory membershipApplicationChannelFactory,
                                IOptions<RabbitMQOptions> options)
            : base(connectionFactory, options, nameof(MembershipStatusConsumer))
        {
            _membershipApplicationChannel = membershipApplicationChannelFactory.GetChannel();
        }

        public override async Task ConsumeAsync(CancellationToken stoppingToken)
        {
            await base.ConsumeAsync(stoppingToken);

            // Add membership specific tasks here
        }

        protected override async Task HandleReceivedAsync(BasicDeliverEventArgs ea, string queueName)
        {
            var body = ea.Body.ToArray();
            var membershipApplication = JsonSerializer.Deserialize<MembershipApplicationDto>(body);
            var membershipApplicationReceivedEventArgs = new MembershipApplicationReceivedEventArgs(membershipApplication!, queueName);

            if (ea.RoutingKey == "membership.status.applied")
            {
                await _membershipApplicationChannel.Writer.WriteAsync(membershipApplicationReceivedEventArgs);
            }
            else if (ea.RoutingKey == "membership.status.cancelled")
            {
                throw new NotImplementedException();
                //await _membershipCancelMessageChannel.Writer.WriteAsync(messageReceivedEventArgs);
            }
        }
    }
}
