using Microsoft.Extensions.Options;
using PubLib.Messaging.RabbitMQ.Clients.Connection;
using PubLib.Messaging.RabbitMQ.Clients.DTOs;
using PubLib.Messaging.RabbitMQ.Clients.Publisher.Base;
using PubLib.Messaging.RabbitMQ.Configuration;
using System.Text.Json;

namespace PubLib.Messaging.RabbitMQ.Clients.Publisher.BookOrder
{
    public class BookOrderPublisher : PublisherBase
    {
        public BookOrderPublisher(IRabbitMQConnectionFactory connection, IOptions<RabbitMQOptions> options)
            : base(connection, options, nameof(BookOrderPublisher))
        {
        }

        public void PublishNewReservation(BookReservationDto item)
        {
            var reservationMessage = JsonSerializer.Serialize(item);
            base.Publish("book.reservation.created", reservationMessage);
        }

        public void PublishNewProvision(BookProvisionDto bookProvisionDto)
        {
            var bookProvision = JsonSerializer.Serialize(bookProvisionDto);
            base.Publish("book.provision.created", bookProvision);
        }

        public override void Publish(string routingKey, string message)
        {
            base.Publish(routingKey, message);
        }
    }
}
