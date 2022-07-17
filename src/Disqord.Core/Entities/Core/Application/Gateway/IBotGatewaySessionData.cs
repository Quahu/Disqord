using System;

namespace Disqord;

/// <summary>
///     Represents gateway session usage data.
/// </summary>
public interface IBotGatewaySessionData : IEntity
{
    /// <summary>
    ///     Gets the maximum session count available per-day.
    /// </summary>
    int MaxCount { get; }

    /// <summary>
    ///     Gets the remaining session count available.
    /// </summary>
    int RemainingCount { get; }

    /// <summary>
    ///     Gets the period after which the <see cref="RemainingCount"/> will reset to <see cref="MaxCount"/>.
    /// </summary>
    TimeSpan ResetAfter { get; }

    /// <summary>
    ///     Gets the maximum count of concurrent identifies allowed per 5 seconds.
    /// </summary>
    int MaxConcurrency { get; }
}