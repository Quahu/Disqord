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
using Qommon.Metadata;

namespace BasicVoice;

/// <summary>
///     Represents a basic <see cref="AudioPlayer"/> implementation
///     that supports queueing audio sources and sends notifications
///     to a text channel.
/// </summary>
public class BasicAudioPlayer : AudioPlayer
{
    /// <summary>
    ///     Gets the bot of this audio player.
    /// </summary>
    public DiscordBotBase Bot { get; }

    /// <summary>
    ///     Gets the ID of the channel this audio player will send notifications to.
    /// </summary>
    public Snowflake NotificationsChannelId { get; }

    // The lock is necessary to prevent the race condition
    // between Enqueue() and OnSourceFinished().
    private readonly object _queueLock = new();
    private readonly Queue<AudioSource> _queue;

    public BasicAudioPlayer(
        DiscordBotBase bot,
        Snowflake notificationsChannelId,
        IVoiceConnection connection)
        : base(connection)
    {
        Bot = bot;
        NotificationsChannelId = notificationsChannelId;

        _queue = new();
    }

    /// <summary>
    ///     Invoked when this audio player is stopped.
    /// </summary>
    /// <param name="exception"> The exception that caused the stop or <see langword="null"/> if no exception occurred. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work.
    /// </returns>
    protected override async ValueTask OnStopped(Exception? exception)
    {
        if (exception == null)
        {
            // If an exception occurred, we'll log it.
            Bot.Logger.LogError(exception, "An exception occurred in the audio player for guild ID {GuildId}.", GuildId);

            // Here you can add different handling for different exceptions that might occur
            // but for VoiceConnectionException the logic should always be basically the same.
            // VoiceConnectionException indicates that the connection object was rendered unusable
            // because, for example, the bot was disconnected from the voice channel.
            // We dispose of this audio player allowing for a new one to be created.
            if (exception is VoiceConnectionException)
            {
                var playerService = Bot.Services.GetRequiredService<AudioPlayerService>();
                await playerService.DisposePlayerAsync(GuildId);
            }
        }
    }

    /// <summary>
    ///     Invoked when an audio source has finished playing.
    ///     Starts playing the next audio source if one is queued
    ///     and sends notifications to the channel with ID <see cref="NotificationsChannelId"/>.
    /// </summary>
    /// <param name="source"> The audio source that finished playing. </param>
    /// <param name="wasReplaced"> <see langword="true"/> if the previous audio source was replaced with a new one. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work.
    /// </returns>
    protected override ValueTask OnSourceFinished(AudioSource source, bool wasReplaced)
    {
        var nextSource = PlayNextSource();

        // Not awaited so that we don't block the audio playback.
        _ = SendNotificationAsync(source, wasReplaced, exception: null, nextSource);
        return default;
    }

    /// <summary>
    ///     Invoked when an <see cref="AudioSource"/> has errored,
    ///     i.e. thrown an exception.
    /// </summary>
    /// <param name="source"> The <see cref="AudioSource"/> that has errored. </param>
    /// <param name="exception"> The exception that occurred. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work.
    /// </returns>
    protected override ValueTask OnSourceErrored(AudioSource source, Exception exception)
    {
        // An error occurred in the audio source.
        // We'll simply log it.
        var title = source.GetMetadataOrDefault<string>(AudioMetadataKeys.Title, null);
        Bot.Logger.LogError(exception, "An exception occurred in the audio source '{Title}' ({AudioSourceType}) "
            + "in the audio player for guild ID {GuildId}.", title ?? "unknown", source.GetType().Name, GuildId);

        var nextSource = PlayNextSource();

        // Not awaited so that we don't block the audio playback.
        _ = SendNotificationAsync(source, wasReplaced: false, exception, nextSource);
        return default;
    }

    private AudioSource? PlayNextSource()
    {
        lock (_queueLock)
        {
            if (_queue.TryDequeue(out var queuedSource))
            {
                // If there's a queued source we start playing it.
                if (TrySetSource(queuedSource))
                {
                    return queuedSource;
                }
            }
        }

        return null;
    }

    private async Task SendNotificationAsync(AudioSource finishedSource, bool wasReplaced, Exception? exception, AudioSource? nextSource)
    {
        // Yield, so the code below runs in background.
        await Task.Yield();

        // Below we access metadata on the audio sources.
        // This metadata is set in AudioModule.
        string? notification = null;
        if (finishedSource.TryGetMetadata<string>(AudioMetadataKeys.Title, out var title))
        {
            if (exception != null)
            {
                notification += $"An error occurred while playing {Markdown.Code(title)}.\n";
            }
            else
            {
                notification += $"{(wasReplaced ? "Skipped" : "Finished")} playing {Markdown.Code(title)}.\n";
            }
        }

        if (nextSource != null && nextSource.TryGetMetadata(AudioMetadataKeys.Title, out title))
        {
            notification += $"Now playing {Markdown.Code(title)}.";
        }

        if (notification != null)
        {
            try
            {
                var message = new LocalMessage().WithContent(notification.TrimStart());
                await Bot.SendMessageAsync(NotificationsChannelId, message);
            }
            catch (Exception ex)
            {
                // If an exception occurred, we'll log it.
                Bot.Logger.LogError(ex, "An exception occurred while sending the notification in the audio player for guild ID {GuildId}.", GuildId);
            }
        }
    }

    /// <summary>
    ///     Enqueues the specified audio source.
    /// </summary>
    /// <param name="source"> The audio source to enqueue. </param>
    /// <returns>
    ///     <see langword="true"/> if the source was enqueued;
    ///     <see langword="false"/> if the source is being played immediately.
    /// </returns>
    public bool Enqueue(AudioSource source)
    {
        lock (_queueLock)
        {
            if (!TrySetSource(source))
            {
                // If a source is already playing we enqueue the source.
                _queue.Enqueue(source);
                return true;
            }
        }

        return false;
    }
}
