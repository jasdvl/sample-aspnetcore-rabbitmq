using RabbitMQ.Client;

namespace PubLib.Messaging.RabbitMQ.Clients.Connection
{
    public interface IRabbitMQConnectionFactory
    {
        IConnection CreateConnection();
    }
}