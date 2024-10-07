using PubLib.Messaging.RabbitMQ.Clients.Consumer.Base;
using PubLib.Messaging.RabbitMQ.Clients.DTOs;

namespace PubLib.Messaging.RabbitMQ.Clients.Consumer
{
    public class MembershipApplicationReceivedEventArgs : MessageBaseReceivedEventArgs
    {
        public MembershipApplicationDto Message { get; }

        public MembershipApplicationReceivedEventArgs(MembershipApplicationDto message, string queueName) : base(queueName)
        {
            Message = message;
        }
    }
}
