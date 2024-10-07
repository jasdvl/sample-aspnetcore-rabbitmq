namespace PubLib.Messaging.RabbitMQ.Configuration;

public class RabbitMQOptions
{
    public string HostName { get; set; }

    public int Port { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public List<ExchangeConfig> Exchanges { get; set; }

    public List<QueueConfig> Queues { get; set; }

    public List<BindingConfig> Bindings { get; set; }
}

public class QueueConfig
{
    public string Name { get; set; }

    public string RoutingKey { get; set; }

    public bool Durable { get; set; }

    public bool Exclusive { get; set; }

    public bool AutoDelete { get; set; }

    public List<string> Consumers { get; set; }
}

public class BindingConfig
{
    public string Queue { get; set; }

    public string Exchange { get; set; }

    public string BindingKey { get; set; }
}
