using System.Text.Json.Serialization;

namespace UseSend;

/// <summary>
///     Domain verification status.
/// </summary>
[JsonConverter(typeof(DomainStatusConverter))]
public enum DomainStatus
{
    /// <summary />
    NotStarted,
    /// <summary />
    Pending,
    /// <summary />
    Success,
    /// <summary />
    Failed,
    /// <summary />
    TemporaryFailure
}