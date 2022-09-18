namespace Disqord.Rest.Api;

/// <summary>
///     Defines the names of known REST API headers.
/// </summary>
public static class RestApiHeaderNames
{
    /// <summary>
    ///     The <c>X-Audit-Log-Reason</c> header; used for sending audit log reasons.
    /// </summary>
    public const string AuditLogReason = "X-Audit-Log-Reason";

    /// <summary>
    ///     The <c>Retry-After</c> header; returns the time after which it is safe to retry the request.
    /// </summary>
    public const string RetryAfter = "Retry-After";

    /// <summary>
    ///     The <c>X-RateLimit-Global</c> header; determines whether the request hit the global rate-limit.
    /// </summary>
    public const string RateLimitGlobal = "X-RateLimit-Global";

    /// <summary>
    ///     The <c>X-RateLimit-Limit</c> header; returns amount of requests allowed in the bucket.
    /// </summary>
    public const string RateLimitLimit = "X-RateLimit-Limit";

    /// <summary>
    ///     The <c>X-RateLimit-Remaining</c> header; returns the amount of requests remaining in the bucket.
    /// </summary>
    public const string RateLimitRemaining = "X-RateLimit-Remaining";

    /// <summary>
    ///     The <c>X-RateLimit-Reset</c> header; returns the date at which the bucket will reset.
    /// </summary>
    public const string RateLimitReset = "X-RateLimit-Reset";

    /// <summary>
    ///     The <c>X-RateLimit-Reset-After</c> header; returns the time after which the bucket resets.
    /// </summary>
    public const string RateLimitResetAfter = "X-RateLimit-Reset-After";

    /// <summary>
    ///     The <c>X-RateLimit-Bucket</c> header; returns the hash of the bucket.
    /// </summary>
    public const string RateLimitBucket = "X-RateLimit-Bucket";

    /// <summary>
    ///     The <c>X-RateLimit-Scope</c> header; returns the scope of the bucket.
    /// </summary>
    public const string RateLimitScope = "X-RateLimit-Scope";

    /// <summary>
    ///     The <c>Date</c> header; returns the server date.
    /// </summary>
    public const string Date = "Date";
}
