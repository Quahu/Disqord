namespace Disqord.Gateway.Api;

/// <summary>
///     Represents the close status for a Discord gateway connection.
/// </summary>
public enum GatewayCloseCode
{
    /// <summary>
    ///     An unknown error occured.
    /// </summary>
    UnknownError = 4000,

    /// <summary>
    ///     The <see cref="GatewayPayloadOperation"/> specified was invalid.
    /// </summary>
    UnknownOperation = 4001,

    /// <summary>
    ///     The payload could not be decoded.
    /// </summary>
    DecodeError = 4002,

    /// <summary>
    ///     A payload was sent without prior authentication.
    /// </summary>
    NotAuthenticated = 4003,

    /// <summary>
    ///     The token provided was invalid.
    /// </summary>
    AuthenticationFailed = 4004,

    /// <summary>
    ///     A <see cref="GatewayPayloadOperation.Identify"/> was sent while already identified.
    /// </summary>
    AlreadyAuthenticated = 4005,

    /// <summary>
    ///     The sequence number of the payload was invalid.
    /// </summary>
    InvalidSequence = 4007,

    /// <summary>
    ///     Sent too many requests.
    /// </summary>
    RateLimited = 4008,

    /// <summary>
    ///     The gateway session timed out.
    /// </summary>
    SessionTimedOut = 4009,

    /// <summary>
    ///     The shard configuration specified was invalid.
    /// </summary>
    InvalidShard = 4010,

    /// <summary>
    ///     The bot has reached <c>2500</c> guilds and sharding of the connection is required
    ///     or the shard would have handled too many guilds upon connection and more shards are required.
    /// </summary>
    ShardingRequired = 4011,

    /// <summary>
    ///     The API version specified was invalid.
    /// </summary>
    InvalidApiVersion = 4012,

    /// <summary>
    ///     The intents specified were invalid.
    /// </summary>
    InvalidIntents = 4013,

    /// <summary>
    ///     The intents specified were disallowed.
    /// </summary>
    DisallowedIntents = 4014
}
