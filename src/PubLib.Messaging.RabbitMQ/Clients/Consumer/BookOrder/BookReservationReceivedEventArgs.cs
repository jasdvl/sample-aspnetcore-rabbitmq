using PubLib.Messaging.RabbitMQ.Clients.Consumer.Base;
using PubLib.Messaging.RabbitMQ.Clients.DTOs;

namespace PubLib.Messaging.RabbitMQ.Clients.Consumer
{
    public class BookReservationReceivedEventArgs : MessageBaseReceivedEventArgs
    {
        public BookReservationDto BookOrder { get; }

        public BookReservationReceivedEventArgs(BookReservationDto bookOrder, string queueName) : base(queueName)
        {
            BookOrder = bookOrder;
        }
    }
}
