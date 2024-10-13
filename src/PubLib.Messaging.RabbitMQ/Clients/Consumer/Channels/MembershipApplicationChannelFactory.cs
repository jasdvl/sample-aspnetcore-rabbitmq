namespace PubLib.Messaging.RabbitMQ.Clients.Consumer.Channels;


public interface IMembershipApplicationChannelFactory : IMessageChannelFactory<MembershipApplicationReceivedEventArgs>
{
}

public class MembershipApplicationChannelFactory : MessageChannelFactory<MembershipApplicationReceivedEventArgs>, IMembershipApplicationChannelFactory
{
}
