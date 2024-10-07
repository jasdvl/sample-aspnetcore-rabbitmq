using Microsoft.Extensions.Options;
using PubLib.Messaging.RabbitMQ.Clients.Connection;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Base;
using PubLib.Messaging.RabbitMQ.Clients.DTOs;
using PubLib.Messaging.RabbitMQ.Configuration;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace PubLib.Messaging.RabbitMQ.Clients.Consumer.BookOrder
{
    public class BookReservationConsumer : ConsumerBase
    {
        public event EventHandler<BookReservationReceivedEventArgs>? BookReserved;

        public BookReservationConsumer(IRabbitMQConnectionFactory connectionFactory, IOptions<RabbitMQOptions> options)
            : base(connectionFactory, options, nameof(BookReservationConsumer))
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
            var bookOrderDto = JsonSerializer.Deserialize<BookReservationDto>(body);

            if (ea.RoutingKey == "book.reservation.created")
            {
                OnBookReserved(new BookReservationReceivedEventArgs(bookOrderDto!, queueName));
            }
            else if (ea.RoutingKey == "book.reservation.cancelled")
            {

            }
        }

        private void OnBookReserved(BookReservationReceivedEventArgs e)
        {
            BookReserved?.Invoke(this, e);
        }
    }

}
