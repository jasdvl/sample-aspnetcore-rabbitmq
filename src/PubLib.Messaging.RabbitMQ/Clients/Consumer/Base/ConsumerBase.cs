using Microsoft.Extensions.Options;
using PubLib.Messaging.RabbitMQ.Clients.Connection;
using PubLib.Messaging.RabbitMQ.Configuration;
using PubLib.Messaging.RabbitMQ.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PubLib.Messaging.RabbitMQ.Clients.Consumer.Base;

public abstract class ConsumerBase : DisposableObject
{
    protected readonly IConnection _connection;

    protected readonly IModel _channelModel;

    protected readonly List<QueueConfig> _queueConfigs;

    protected readonly List<BindingConfig> _bindingConfigs;

    protected List<string> _consumerTags = new List<string>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsumerBase"/> class.
    /// </summary>
    /// <param name="connectionFactory">The factory for creating RabbitMQ connections.</param>
    /// <param name="options">The options for configuring RabbitMQ.</param>
    /// <param name="consumerName">The name of the consumer.</param>
    /// <remarks>
    /// This constructor sets up the basic configuration for a RabbitMQ consumer.
    /// It uses the provided connection factory to establish a connection to RabbitMQ,
    /// applies the specified options, and sets the consumer name.
    /// </remarks>
    protected ConsumerBase(IRabbitMQConnectionFactory connectionFactory, IOptions<RabbitMQOptions> options, string consumerName)
    {
        _connection = connectionFactory.CreateConnection();
        _channelModel = _connection.CreateModel();

        // Get queue configurations for this consumer
        _queueConfigs = options.Value.Queues
            .Where(q => q.Consumers.Contains(consumerName))
            .ToList();

        if (!_queueConfigs.Any())
        {
            throw new ArgumentException($"No queue configuration found for consumer: {consumerName}");
        }

        _bindingConfigs = new List<BindingConfig>();

        foreach (var queueConfig in _queueConfigs)
        {
            // Find bindings for this queue
            var bindingsForQueue = options.Value.Bindings
                .Where(b => b.Queue == queueConfig.Name)
                .ToList();

            if (!bindingsForQueue.Any())
            {
                throw new ArgumentException($"No binding configuration found for queue: {queueConfig.Name}");
            }

            _bindingConfigs.AddRange(bindingsForQueue);
        }
    }

    /// <summary>
    /// Consumes messages from the configured queues asynchronously.
    /// This is the default implementation in the base class and can be overridden in derived classes.
    /// </summary>
    /// <param name="stoppingToken">A token that can be used to signal cancellation of the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public virtual async Task ConsumeAsync(CancellationToken stoppingToken)
    {
        InitializeQueuesAndBindings();

        foreach (var queueConfig in _queueConfigs)
        {
            var consumer = new EventingBasicConsumer(_channelModel);
            consumer.Received += async (model, ea) =>
            {
                await HandleReceivedAsync(ea, queueConfig.Name);
            };

            var consumerTag = _channelModel.BasicConsume(queue: queueConfig.Name, autoAck: true, consumer: consumer);
            _consumerTags.Add(consumerTag);
        }
    }

    /// <summary>
    /// Handles the received message for a specific queue asynchronously.
    /// </summary>
    /// <param name="ea">The event arguments containing delivery information and the message.</param>
    /// <param name="queueName">The name of the queue from which the message was received.</param>
    protected abstract Task HandleReceivedAsync(BasicDeliverEventArgs ea, string queueName);

    /// <summary>
    /// Releases the managed resources used by the object. This method is called by the Dispose method.
    /// Derived classes should override this method to release managed resources.
    /// </summary>
    protected override void DisposeManagedResources()
    {
        // Dispose of consumers and other resources
        foreach (var consumerTag in _consumerTags)
        {
            _channelModel.BasicCancel(consumerTag);

        }

        _channelModel.Close();
        _connection.Close();

        base.DisposeManagedResources();
    }

    protected void InitializeQueuesAndBindings()
    {
        foreach (var queueConfig in _queueConfigs)
        {
            _channelModel.QueueDeclare(
                queue: queueConfig.Name,
                durable: queueConfig.Durable,
                exclusive: queueConfig.Exclusive,
                autoDelete: queueConfig.AutoDelete,
                arguments: null);

            var bindingsForQueue = _bindingConfigs
                .Where(b => b.Queue == queueConfig.Name)
                .ToList();

            if (!bindingsForQueue.Any())
            {
                throw new ArgumentException($"No binding configuration found for queue: {queueConfig.Name}");
            }

            foreach (var binding in bindingsForQueue)
            {
                // Assuming exchange already exists
                _channelModel.QueueBind(
                    queue: queueConfig.Name,
                    exchange: binding.Exchange,
                    routingKey: binding.BindingKey
                );
            }
        }
    }
}
