using System.Text.Json.Serialization;

namespace PubLib.Messaging.RabbitMQ.Clients.DTOs;

public class BookReservationDto
{
    [JsonPropertyName("book")]
    public required BookDto Book { get; set; }

    [JsonPropertyName("userId")]
    public int UserId { get; set; }
}