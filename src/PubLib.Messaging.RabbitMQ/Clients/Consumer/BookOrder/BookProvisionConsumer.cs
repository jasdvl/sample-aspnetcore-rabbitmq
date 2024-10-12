using Microsoft.Extensions.Options;
using PubLib.Messaging.RabbitMQ.Clients.Connection;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Base;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Channels;
using PubLib.Messaging.RabbitMQ.Clients.DTOs;
using PubLib.Messaging.RabbitMQ.Configuration;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Threading.Channels;

namespace PubLib.Messaging.RabbitMQ.Clients.Consumer.BookOrder
{
    public class BookProvisionConsumer : ConsumerBase
    {
        private readonly Channel<BookProvisionReceivedEventArgs> _bookProvisionChannel;

        public BookProvisionConsumer(
                                    IRabbitMQConnectionFactory connectionFactory,
                                    IBookProvisionChannelFactory bookProvisionChannelFactory,
                                    IOptions<RabbitMQOptions> options)
            : base(connectionFactory, options, nameof(BookProvisionConsumer))
        {
            _bookProvisionChannel = bookProvisionChannelFactory.GetChannel();
        }

        public override async Task ConsumeAsync(CancellationToken stoppingToken)
        {
            await base.ConsumeAsync(stoppingToken);

            // Add book provision tasks here
        }

        protected override async Task HandleReceivedAsync(BasicDeliverEventArgs ea, string queueName)
        {
            var body = ea.Body.ToArray();
            var bookProvisionDto = JsonSerializer.Deserialize<BookProvisionDto>(body);

            if (ea.RoutingKey == "book.provision.created")
            {
                var messageReceivedEventArgs = new BookProvisionReceivedEventArgs(bookProvisionDto!, queueName);

                await _bookProvisionChannel.Writer.WriteAsync(messageReceivedEventArgs);
            }
            else if (ea.RoutingKey == "book.provision.failed")
            {
                throw new NotImplementedException();
            }
        }
    }
}
