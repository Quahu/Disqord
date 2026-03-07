using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot.Hosting;
using Disqord.Extensions.Voice;
using Disqord.Gateway;

namespace BasicVoice;

public class AudioPlayerService : DiscordBotService
{
    private readonly Dictionary<Snowflake, BasicAudioPlayer> _players = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);

        var voiceExtension = Bot.GetRequiredExtension<VoiceExtension>();

        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            foreach (var (guildId, player) in _players)
            {
                player.Stop();
                await player.DisposeAsync();

                await voiceExtension.DisconnectAsync(guildId);
            }

            _players.Clear();
        }
        finally
        {
            _semaphore.Release();
        }
    }

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

    // Disposes of the audio player when the bot is disconnected from a voice channel.
    // BasicAudioPlayer.OnStopped already handles disposing,
    // but this allows for a graceful stop when the voice state update arrives before the connection is killed.
    protected override async ValueTask OnVoiceStateUpdated(VoiceStateUpdatedEventArgs e)
    {
        if (e.MemberId == Bot.CurrentUser.Id && e.NewVoiceState.ChannelId == null)
        {
            await DisposePlayerAsync(e.GuildId);
        }
    }
}
