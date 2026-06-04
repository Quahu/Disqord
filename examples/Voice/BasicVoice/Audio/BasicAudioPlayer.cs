using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot;
using Disqord.Extensions.Voice;
using Disqord.Rest;
using Disqord.Voice;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BasicVoice;

// Basic player implementation that supports queueing and sends notifications to a text channel.
public class BasicAudioPlayer(
    DiscordBotBase bot,
    Snowflake notificationsChannelId,
    IVoiceConnection connection) : AudioPlayer(connection)
{
    public DiscordBotBase Bot { get; } = bot;

    public Snowflake NotificationsChannelId { get; } = notificationsChannelId;

    public Song? CurrentSong { get; private set; }

    private readonly object _queueLock = new();
    private readonly Queue<Song> _queue = new();

    public bool Enqueue(Song track)
    {
        lock (_queueLock)
        {
            if (!TrySetSource(track.Source))
            {
                _queue.Enqueue(track);
                return true;
            }

            CurrentSong = track;
        }

        return false;
    }

    private async Task SendNotificationAsync(string content)
    {
        // Yield to not delay playing the next audio track.
        await Task.Yield();

        try
        {
            await Bot.SendMessageAsync(NotificationsChannelId,
                new LocalMessage().WithContent(content));
        }
        catch (Exception ex)
        {
            Bot.Logger.LogWarning(ex, "Failed to send notification for guild ID {GuildId}.", GuildId);
        }
    }

    protected override ValueTask OnSourceStarted(IAudioSource source)
    {
        var song = CurrentSong;
        if (song != null)
        {
            _ = SendNotificationAsync($"Now playing {Markdown.Bold(song.Title)}.");
        }

        return default;
    }

    protected override ValueTask OnSourceFinished(IAudioSource source, bool wasReplaced)
    {
        var song = CurrentSong;
        if (song != null && !wasReplaced)
        {
            _ = SendNotificationAsync($"Finished playing {Markdown.Bold(song.Title)}.");
        }

        PlayNextTrack();
        return default;
    }

    protected override ValueTask OnSourceErrored(IAudioSource source, Exception exception)
    {
        var displayName = CurrentSong?.Title ?? source.GetType().Name;
        Bot.Logger.LogError(exception, "An exception occurred in audio source '{Title}' for guild ID {GuildId}.", displayName, GuildId);

        _ = SendNotificationAsync($"An error occurred while playing {Markdown.Bold(displayName)}.");

        PlayNextTrack();
        return default;
    }

    protected override async ValueTask OnStopped(Exception? exception)
    {
        if (exception != null)
        {
            Bot.Logger.LogError(exception, "An exception occurred in the audio player for guild ID {GuildId}.", GuildId);

            if (exception is VoiceConnectionException)
            {
                var playerService = Bot.Services.GetRequiredService<AudioPlayerService>();
                await playerService.DisposePlayerAsync(GuildId);
            }
        }
    }

    private void PlayNextTrack()
    {
        lock (_queueLock)
        {
            if (_queue.TryDequeue(out var next))
            {
                CurrentSong = next;
                TrySetSource(next.Source);
            }
            else
            {
                CurrentSong = null;
            }
        }
    }
}
