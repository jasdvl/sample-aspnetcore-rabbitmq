using Microsoft.Extensions.Options;
using PubLib.Messaging.RabbitMQ.Clients.Connection;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Base;
using PubLib.Messaging.RabbitMQ.Clients.DTOs;
using PubLib.Messaging.RabbitMQ.Configuration;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace PubLib.Messaging.RabbitMQ.Clients.Consumer.BookOrder
{
    public class BookProvisionConsumer : ConsumerBase
    {
        public event EventHandler<BookProvisionReceivedEventArgs>? BookReadyForPickup;

        public BookProvisionConsumer(IRabbitMQConnectionFactory connectionFactory, IOptions<RabbitMQOptions> options)
            : base(connectionFactory, options, nameof(BookProvisionConsumer))
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
            var bookHoldMessage = JsonSerializer.Deserialize<BookProvisionDto>(body);

            if (ea.RoutingKey == "book.provision.created")
            {
                OnBookProvisionReady(new BookProvisionReceivedEventArgs(bookHoldMessage!, queueName));
            }
            else if (ea.RoutingKey == "book.provision.failed")
            {

            }
        }

        private void OnBookProvisionReady(BookProvisionReceivedEventArgs e)
        {
            BookReadyForPickup?.Invoke(this, e);
        }
    }
}
