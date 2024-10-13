using PubLib.Messaging.RabbitMQ.Infrastructure;
using System.Threading.Channels;

namespace PubLib.Messaging.RabbitMQ.Clients.Consumer.Channels;

/// <summary>
/// Interface for a factory that provides channels for relaying messages from RabbitMQ consumers.
/// </summary>
/// <typeparam name="T">The type of messages relayed through the channel.</typeparam>
public interface IMessageChannelFactory<T> : IDisposable
{
    /// <summary>
    /// Gets a channel for asynchronously relaying messages from RabbitMQ consumers.
    /// </summary>
    /// <returns>A <see cref="Channel{T}"/> instance for handling messages of type <typeparamref name="T"/>.</returns>
    Channel<T> GetChannel();
}

/// <summary>
/// A generic implementation of <see cref="IMessageChannelFactory{T}"/> that provides unbounded channels 
/// for relaying messages from RabbitMQ consumers.
/// </summary>
/// <typeparam name="T">The type of messages relayed through the channel.</typeparam>
public class MessageChannelFactory<T> : DisposableObject, IMessageChannelFactory<T>
{
    private readonly Channel<T> _channel;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageChannelFactory{T}"/> class with an unbounded channel
    /// for relaying messages from RabbitMQ consumers.
    /// </summary>
    public MessageChannelFactory()
    {
        _channel = Channel.CreateUnbounded<T>();
    }

    /// <summary>
    /// Gets the unbounded channel used for relaying messages from RabbitMQ consumers.
    /// </summary>
    /// <returns>A <see cref="Channel{T}"/> instance for relaying messages of type <typeparamref name="T"/>.</returns>
    public Channel<T> GetChannel()
    {
        return _channel;
    }

    /// <summary>
    /// Releases the managed resources used by the instance of the <see cref="MessageChannelFactory{T}"/> class.
    /// This method is called by the Dispose method.
    /// Derived classes should override this method to release managed resources.
    /// </summary>
    protected override void DisposeManagedResources()
    {
        // Complete the writer to release the channel resources.
        _channel.Writer.Complete();
        base.DisposeManagedResources();
    }
}
