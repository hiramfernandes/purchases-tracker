using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Purchases.Domain.Models;

public class Merchant
{
    [JsonPropertyName("legal_name")]
    public string? LegalName { get; set; }

    [JsonPropertyName("trade_name")]
    public string? TradeName { get; set; }

    [JsonPropertyName("id")]
    [BsonRepresentation(BsonType.String)]
    public string? Cnpj { get; set; }
    
    [JsonPropertyName("address")]
    public Address? Address { get; set; }
}

public class Address
{
    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("neighborhood")]
    public string? Neighborhood { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("zip")]
    public string? Zip { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }
}
