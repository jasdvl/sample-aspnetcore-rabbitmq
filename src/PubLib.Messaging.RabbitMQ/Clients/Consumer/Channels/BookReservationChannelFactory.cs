namespace PubLib.Messaging.RabbitMQ.Clients.Consumer.Channels;

/// <summary>
/// Interface for a factory that provides channels for relaying book reservation messages
/// from RabbitMQ consumers.
/// </summary>
public interface IBookReservationChannelFactory : IMessageChannelFactory<BookReservationReceivedEventArgs>
{
}

/// <summary>
/// A factory that creates and manages a channel for relaying book reservation messages
/// from RabbitMQ consumers.
/// </summary>
public class BookReservationChannelFactory : MessageChannelFactory<BookReservationReceivedEventArgs>, IBookReservationChannelFactory
{
}
