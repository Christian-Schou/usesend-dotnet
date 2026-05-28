using System.Text.Json;
using System.Text.Json.Serialization;

namespace UseSend.Webhooks;

/// <summary>
///     Data payload for all contact webhook events
///     (<c>contact.created</c>, <c>contact.updated</c>, <c>contact.deleted</c>).
/// </summary>
public sealed class ContactEventData
{
    /// <summary>Contact ID.</summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>Contact email address.</summary>
    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    /// <summary>Contact book this contact belongs to.</summary>
    [JsonPropertyName("contactBookId")]
    public string ContactBookId { get; init; } = string.Empty;

    /// <summary>Whether the contact is subscribed.</summary>
    [JsonPropertyName("subscribed")]
    public bool Subscribed { get; init; }

    /// <summary>Custom properties attached to the contact.</summary>
    [JsonPropertyName("properties")]
    public JsonElement? Properties { get; init; }

    /// <summary>First name.</summary>
    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }

    /// <summary>Last name.</summary>
    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }

    /// <summary>When the contact was created.</summary>
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>When the contact was last updated.</summary>
    [JsonPropertyName("updatedAt")]
    public DateTimeOffset UpdatedAt { get; init; }
}