using System.Threading.Channels;

namespace PubLib.Messaging.RabbitMQ.Clients.Consumer.Channels;

public interface IBookProvisionChannelFactory
{
    Channel<BookProvisionReceivedEventArgs> GetChannel();
}

public class BookProvisionChannelFactory : IBookProvisionChannelFactory
{
    private readonly Channel<BookProvisionReceivedEventArgs> _channel;

    public BookProvisionChannelFactory()
    {
        _channel = Channel.CreateUnbounded<BookProvisionReceivedEventArgs>();
    }

    public Channel<BookProvisionReceivedEventArgs> GetChannel()
    {
        return _channel;
    }
}
