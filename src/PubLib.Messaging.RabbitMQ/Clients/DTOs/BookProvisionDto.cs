using System.Text.Json.Serialization;

namespace PubLib.Messaging.RabbitMQ.Clients.DTOs;

public class BookProvisionDto
{
    [JsonPropertyName("book")]
    public required BookDto Book { get; set; }

    /// <summary>
    /// Unique identifier of the patron for whom the book is being provided.
    /// </summary>
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("provisionTimestamp")]
    public DateTime ProvisionTimestamp { get; set; }
}