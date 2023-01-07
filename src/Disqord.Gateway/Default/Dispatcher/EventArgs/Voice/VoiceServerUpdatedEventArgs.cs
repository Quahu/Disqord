using System;

namespace Disqord.Gateway;

public class VoiceServerUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the server was updated.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the voice server token of this update.
    /// </summary>
    public string Token { get; }

    /// <summary>
    ///     Gets the voice server endpoint of this update.
    /// </summary>
    /// <returns>
    ///     The endpoint or <see langword="null"/> if the voice server is currently being reallocated.
    /// </returns>
    public string? Endpoint { get; }

    public VoiceServerUpdatedEventArgs(
        Snowflake guildId,
        string token,
        string? endpoint)
    {
        GuildId = guildId;
        Token = token;
        Endpoint = endpoint;
    }
}
