using Disqord.Extensions.Voice;

namespace BasicVoice;

/// <summary>
///     Represents a song.
/// </summary>
/// <param name="Title"> The display title of the track. </param>
/// <param name="Source"> The audio source to play. </param>
public record Song(string Title, IAudioSource Source);
