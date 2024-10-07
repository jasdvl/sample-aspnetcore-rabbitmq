
namespace PubLib.Messaging.RabbitMQ.Configuration;

public class ExchangeConfig
{
    public string Name { get; set; }

    public string Type { get; set; }

    public bool Durable { get; set; }

    public bool AutoDelete { get; set; }

    public List<string> Publishers { get; set; } // Liste von Publisher-Klassen
}