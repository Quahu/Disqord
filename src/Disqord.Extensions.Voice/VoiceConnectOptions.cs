namespace Disqord.Extensions.Voice;

/// <summary>
///     Represents voice-state behavior options used when connecting a voice session.
/// </summary>
public class VoiceConnectOptions
{
    /// <summary>
    ///     Gets or sets whether the current user should be self-muted while connected.
    /// </summary>
    public bool SelfMute { get; set; }

    /// <summary>
    ///     Gets or sets whether the current user should be self-deafened while connected.
    ///     This is visual-only for bots; bots can receive audio regardless of this setting.
    /// </summary>
    public bool SelfDeafen { get; set; } = true;
}
