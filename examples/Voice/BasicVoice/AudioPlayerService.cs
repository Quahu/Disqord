using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot.Hosting;
using Disqord.Extensions.Voice;
using Disqord.Gateway;

namespace BasicVoice;

/// <summary>
///     Represents a type responsible for maintaining active
///     audio players for guilds.
/// </summary>
public class AudioPlayerService : DiscordBotService
{
    private readonly Dictionary<Snowflake, BasicAudioPlayer> _players = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public AudioPlayerService()
    { }

    /// <summary>
    ///     Disposes of the audio players when the service is stopped,
    ///     i.e. on bot shutdown.
    /// </summary>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);

        var voiceExtension = Bot.GetRequiredExtension<VoiceExtension>();

        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            foreach (var (guildID, player) in _players)
            {
                player.Stop();
                await player.DisposeAsync();

                await voiceExtension.DisconnectAsync(guildID);
            }

            _players.Clear();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    ///     Gets the audio player for the guild with the specified ID.
    /// </summary>
    /// <param name="guildId"> The ID of the guild. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> with the result being the audio player
    ///     or <see langword="null"/> if no player exists.
    /// </returns>
    public async Task<BasicAudioPlayer?> GetPlayerAsync(Snowflake guildId, CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            return _players.GetValueOrDefault(guildId);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    ///     Connects the audio player for the guild with the specified ID.
    /// </summary>
    /// <param name="guildId"> The ID of the guild. </param>
    /// <param name="voiceChannelId"> The ID of the voice channel to connect to. </param>
    /// <param name="notificationsChannelId"> The ID of the text channel to send notifications to. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> with the result being the audio player.
    /// </returns>
    public async Task<BasicAudioPlayer> ConnectPlayerAsync(Snowflake guildId, Snowflake voiceChannelId, Snowflake notificationsChannelId, CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            if (!_players.TryGetValue(guildId, out var player))
            {
                var voiceExtension = Bot.GetRequiredExtension<VoiceExtension>();
                var voiceConnection = await voiceExtension.ConnectAsync(guildId, voiceChannelId, cancellationToken);
                _players[guildId] = player = new BasicAudioPlayer(Bot, notificationsChannelId, voiceConnection);
                player.Start();
            }

            return player;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    ///     Disposes of the audio player for the guild with the specified ID.
    /// </summary>
    /// <param name="guildId"> The ID of the guild. </param>
    public async Task DisposePlayerAsync(Snowflake guildId)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (_players.Remove(guildId, out var player))
            {
                player.Stop();
                await player.DisposeAsync();

                var voiceExtension = Bot.GetRequiredExtension<VoiceExtension>();
                await voiceExtension.DisconnectAsync(guildId);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    ///     Disposes of audio players when the bot is disconnected from voice channels.
    /// </summary>
    /// <remarks>
    ///     Note that our <see cref="BasicAudioPlayer"/>'s <see cref="AudioPlayer.OnStopped"/>
    ///     already handles disposing of the player, but this allows for a graceful stop of the player in the case
    ///     where the voice state update arrives before the websocket connection is killed (it's a race condition).
    /// </remarks>
    /// <param name="e"> The event data. </param>
    protected override async ValueTask OnVoiceStateUpdated(VoiceStateUpdatedEventArgs e)
    {
        // Checks if the bot was disconnected from a voice channel.
        if (e.MemberId == Bot.CurrentUser.Id && e.NewVoiceState.ChannelId == null)
        {
            await DisposePlayerAsync(e.GuildId);
        }
    }
}
