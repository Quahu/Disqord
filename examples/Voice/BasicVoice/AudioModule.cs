using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot.Commands.Application;
using Disqord.Gateway;
using Qmmands;
using Qommon.Metadata;

namespace BasicVoice;

/// <summary>
///     Contains the audio slash commands.
/// </summary>
public class AudioModule : DiscordApplicationGuildModuleBase
{
    private readonly AudioPlayerService _playerService;

    public AudioModule(AudioPlayerService playerService)
    {
        _playerService = playerService;
    }

    [SlashCommand("play")]
    [Description("Plays the audio track matching the specified query.")]
    public async Task<IResult> Play(
        [Description("The audio track query.")] string query,
        [Description("The channel to initially connect to."), ChannelTypes(ChannelType.Voice)] IInteractionChannel? voiceChannel = null)
    {
        var player = await _playerService.GetPlayerAsync(Context.GuildId);
        if (player == null)
        {
            // Defer the response as we need time to connect.
            await Deferral();

            var voiceChannelId = voiceChannel?.Id ?? Context.Author.GetVoiceState()?.ChannelId;
            if (voiceChannelId == null)
                return Response("Please provide a voice channel to connect to.");

            player = await _playerService.ConnectPlayerAsync(Context.GuildId, voiceChannelId.Value, Context.ChannelId);
        }

        // Do your audio querying here.
        // For example purposes, we'll only look for files
        // in the 'music' folder in the bot's working directory
        // using the most basic string contains check.
        var files = Directory.GetFiles("./music/");
        var file = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file).Contains(query, StringComparison.OrdinalIgnoreCase));
        if (file == null)
            return Response("No file found.");

        var source = new FFmpegAudioSource(File.OpenRead(file));
        var title = Path.GetFileNameWithoutExtension(file);

        // This sets a metadata value with the key 'Title'.
        // It's a quick and easy way to attach metadata to AudioSource instances.
        // A performant alternative would be to create AudioSource implementations
        // that store data such as the title in actual fields.
        // The audio player would then cast to those types and access the fields efficiently.
        source.SetMetadata(AudioMetadataKeys.Title, title);

        var queued = player.Enqueue(source);
        return Response($"{(queued ? "Queued" : "Playing")} {Markdown.Code(title)}.");
    }

    [SlashCommand("pause")]
    [Description("Pauses the audio playback.")]
    public async Task<IResult> Pause()
    {
        var player = await _playerService.GetPlayerAsync(Context.GuildId);
        if (player == null)
        {
            return Response("Not playing.");
        }

        if (!player.Pause())
        {
            return Response("Already paused.");
        }

        return Response("Paused.");
    }

    [SlashCommand("resume")]
    [Description("Resumes the audio playback if it's been paused.")]
    public async Task<IResult> Resume()
    {
        var player = await _playerService.GetPlayerAsync(Context.GuildId);
        if (player == null)
        {
            return Response("Not playing.");
        }

        if (!player.Resume())
        {
            return Response("Already resumed.");
        }

        return Response("Resumed.");
    }

    [SlashCommand("skip")]
    [Description("Skips the currently playing audio track.")]
    public async Task<IResult> Skip()
    {
        var player = await _playerService.GetPlayerAsync(Context.GuildId);
        if (player == null || player.Source == null)
        {
            return Response("Not playing.");
        }

        // Setting the source to null stops the currently playing source
        // and our audio player will start playing the next source in queue.
        player.Source = null;

        return Response(new LocalInteractionMessageResponse().WithContent("Skipped.").WithIsEphemeral());
    }

    [SlashCommand("stop")]
    [Description("Stops the playback and disconnects the bot from the voice channel.")]
    public async Task<IResult> Stop()
    {
        var player = await _playerService.GetPlayerAsync(Context.GuildId);
        if (player == null)
        {
            return Response("Not playing.");
        }

        await _playerService.DisposePlayerAsync(Context.GuildId);
        return Response("Disconnected.");
    }
}
