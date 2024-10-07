using System.Text.Json.Serialization;

namespace PubLib.Messaging.RabbitMQ.Clients.DTOs;

public class PersonDto
{
    [JsonPropertyName("firstName")]
    public required string FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public required string LastName { get; set; }

    [JsonPropertyName("city")]
    public required string City { get; set; }
}

public class MembershipApplicationDto
{
    [JsonPropertyName("person")]
    public required PersonDto Person { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}