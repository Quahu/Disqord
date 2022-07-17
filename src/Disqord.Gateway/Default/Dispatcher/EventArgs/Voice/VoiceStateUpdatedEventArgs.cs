using System;

namespace Disqord.Gateway;

public class VoiceStateUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the update occurred.
    /// </summary>
    public Snowflake GuildId => NewVoiceState.GuildId;

    /// <summary>
    ///     Gets the ID of the member for which the update occurred.
    /// </summary>
    public Snowflake MemberId => NewVoiceState.MemberId;

    /// <summary>
    ///     Gets the member for which the update occurred.
    /// </summary>
    public IMember Member { get; }

    /// <summary>
    ///     Gets whether the <see cref="Member"/> is a lurker, i.e. is missing data like <see cref="IMember.JoinedAt"/>.
    /// </summary>
    public bool IsLurker { get; }

    /// <summary>
    ///     Gets the voice state in the state before the update occurred.
    ///     Returns <see langword="null"/> if the voice state was not cached.
    /// </summary>
    public CachedVoiceState? OldVoiceState { get; }

    /// <summary>
    ///     Gets the updated voice state.
    /// </summary>
    public IVoiceState NewVoiceState { get; }

    public VoiceStateUpdatedEventArgs(
        IMember member,
        bool isLurker,
        CachedVoiceState? oldVoiceState,
        IVoiceState newVoiceState)
    {
        Member = member;
        IsLurker = isLurker;
        OldVoiceState = oldVoiceState;
        NewVoiceState = newVoiceState;
    }
}
