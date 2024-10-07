namespace PubLib.Messaging.RabbitMQ.Clients.Consumer.Base
{
    public class MessageReceivedEventArgs : MessageBaseReceivedEventArgs
    {
        public string Message { get; }

        public MessageReceivedEventArgs(string message, string queueName) : base(queueName)
        {
            Message = message;
        }
    }

    public abstract class MessageBaseReceivedEventArgs : EventArgs
    {
        public string QueueName { get; }

        public MessageBaseReceivedEventArgs(string queueName)
        {
            QueueName = queueName;
        }
    }
}
