using PubLib.Messaging.RabbitMQ.Clients.Consumer.Base;
using PubLib.Messaging.RabbitMQ.Clients.DTOs;

namespace PubLib.Messaging.RabbitMQ.Clients.Consumer
{
    public class BookProvisionReceivedEventArgs : MessageBaseReceivedEventArgs
    {
        public BookProvisionDto BookProvision { get; }

        public BookProvisionReceivedEventArgs(BookProvisionDto bookProvision, string queueName) : base(queueName)
        {
            BookProvision = bookProvision;
        }
    }
}
