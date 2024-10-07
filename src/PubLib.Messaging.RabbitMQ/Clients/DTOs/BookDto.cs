using System.Text.Json.Serialization;

namespace PubLib.Messaging.RabbitMQ.Clients.DTOs;

public class BookDto
{
    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("year")]
    public required int Year { get; set; }
}
