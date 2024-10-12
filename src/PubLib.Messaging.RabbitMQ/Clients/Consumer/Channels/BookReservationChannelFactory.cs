using System.Threading.Channels;

namespace PubLib.Messaging.RabbitMQ.Clients.Consumer.Channels;

/// <summary>
/// Interface for creating a channel for book reservation events.
/// </summary>
public interface IBookReservationChannelFactory
{
    /// <summary>
    /// Gets the channel for book reservation received events.
    /// </summary>
    /// <returns>A channel that handles <see cref="BookReservationReceivedEventArgs"/>.</returns>
    Channel<BookReservationReceivedEventArgs> GetChannel();
}

/// <summary>
/// Factory class for creating and managing a channel for book reservation events.
/// </summary>
public class BookReservationChannelFactory : IBookReservationChannelFactory
{
    private readonly Channel<BookReservationReceivedEventArgs> _channel;

    /// <summary>
    /// Initializes a new instance of the <see cref="BookReservationChannelFactory"/> class,
    /// creating an unbounded channel for book reservation events.
    /// </summary>
    public BookReservationChannelFactory()
    {
        _channel = Channel.CreateUnbounded<BookReservationReceivedEventArgs>();
    }

    /// <summary>
    /// Gets the channel for book reservation received events.
    /// </summary>
    /// <returns>A channel that handles <see cref="BookReservationReceivedEventArgs"/>.</returns>
    public Channel<BookReservationReceivedEventArgs> GetChannel()
    {
        return _channel;
    }
}
