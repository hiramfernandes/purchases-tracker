using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Purchases.Domain.Models;

public class Receipt
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    [JsonPropertyName("id")]
    public required string Url { get; set; }

    [JsonPropertyName("processed")]
    public bool Processed { get; set; }

    [JsonPropertyName("received-date")]
    public DateTime? ReceivedDate { get; set; }

    [JsonPropertyName("processed-date")]
    public DateTime? ProcessedDate { get; set; }
    
    [JsonPropertyName("processing-message")]
    public string? ProcessingMessage { get; set; }
}
