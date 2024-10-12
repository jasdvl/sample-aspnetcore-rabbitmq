using System.Threading.Channels;

namespace PubLib.Messaging.RabbitMQ.Clients.Consumer.Channels;

public interface IMembershipApplicationChannelFactory
{
    Channel<MembershipApplicationReceivedEventArgs> GetChannel();
}

public class MembershipApplicationChannelFactory : IMembershipApplicationChannelFactory
{
    private readonly Channel<MembershipApplicationReceivedEventArgs> _channel;

    public MembershipApplicationChannelFactory()
    {
        _channel = Channel.CreateUnbounded<MembershipApplicationReceivedEventArgs>();
    }

    public Channel<MembershipApplicationReceivedEventArgs> GetChannel()
    {
        return _channel;
    }
}
