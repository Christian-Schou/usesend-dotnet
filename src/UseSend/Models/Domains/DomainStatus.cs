using System.Text.Json.Serialization;

namespace UseSend;

/// <summary>
///     Domain verification status.
/// </summary>
public enum DomainStatus
{
    /// <summary />
    [JsonStringEnumMemberName("NOT_STARTED")]
    NotStarted,

    /// <summary />
    [JsonStringEnumMemberName("PENDING")] Pending,

    /// <summary />
    [JsonStringEnumMemberName("SUCCESS")] Success,

    /// <summary />
    [JsonStringEnumMemberName("FAILED")] Failed,

    /// <summary />
    [JsonStringEnumMemberName("TEMPORARY_FAILURE")]
    TemporaryFailure
}