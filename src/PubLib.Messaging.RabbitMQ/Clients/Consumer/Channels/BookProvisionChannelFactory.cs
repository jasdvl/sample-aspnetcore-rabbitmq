namespace PubLib.Messaging.RabbitMQ.Clients.Consumer.Channels;

public interface IBookProvisionChannelFactory : IMessageChannelFactory<BookProvisionReceivedEventArgs>
{
}

public class BookProvisionChannelFactory : MessageChannelFactory<BookProvisionReceivedEventArgs>, IBookProvisionChannelFactory
{
}
