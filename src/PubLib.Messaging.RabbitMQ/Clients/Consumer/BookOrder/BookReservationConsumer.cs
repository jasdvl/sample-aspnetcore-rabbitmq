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
    public class BookReservationConsumer : ConsumerBase
    {
        private readonly Channel<BookReservationReceivedEventArgs> _bookReservationChannel;

        public BookReservationConsumer(
                                    IRabbitMQConnectionFactory connectionFactory,
                                    IOptions<RabbitMQOptions> options,
                                    IBookReservationChannelFactory bookReservationChannelFactory)
            : base(connectionFactory, options, nameof(BookReservationConsumer))
        {
            _bookReservationChannel = bookReservationChannelFactory.GetChannel();
        }

        public override async Task ConsumeAsync(CancellationToken stoppingToken)
        {
            await base.ConsumeAsync(stoppingToken);

            // Add book reservation specific tasks here
        }

        /// <summary>
        /// Handles the received message for a specific queue asynchronously.
        /// </summary>
        /// <param name="ea">The event arguments containing delivery information and the message.</param>
        /// <param name="queueName">The name of the queue from which the message was received.</param>
        protected override async Task HandleReceivedAsync(BasicDeliverEventArgs ea, string queueName)
        {
            var body = ea.Body.ToArray();
            var bookOrderDto = JsonSerializer.Deserialize<BookReservationDto>(body);

            if (ea.RoutingKey == "book.reservation.created")
            {
                var messageReceivedEventArgs = new BookReservationReceivedEventArgs(bookOrderDto!, queueName);

                await _bookReservationChannel.Writer.WriteAsync(messageReceivedEventArgs);
            }
            else if (ea.RoutingKey == "book.reservation.cancelled")
            {
                throw new NotImplementedException();
            }
        }
    }
}
