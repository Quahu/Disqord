using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot.Commands.Application;
using Disqord.Gateway;
using Qmmands;

namespace BasicVoice;

/// <summary>
///     Contains the audio slash commands.
/// </summary>
public class AudioModule(AudioPlayerService playerService) : DiscordApplicationGuildModuleBase
{
    [SlashCommand("play")]
    [Description("Plays the audio track matching the specified query.")]
    public async Task<IResult> Play(
        [Description("The audio track query.")] string query,
        [Description("The channel to initially connect to."), ChannelTypes(ChannelType.Voice)] IInteractionChannel? voiceChannel = null)
    {
        await Deferral();

        var player = await playerService.GetPlayerAsync(Context.GuildId);
        if (player == null)
        {
            var voiceChannelId = voiceChannel?.Id ?? Context.Author.GetVoiceState()?.ChannelId;
            if (voiceChannelId == null)
            {
                return Response("Please provide a voice channel to connect to.");
            }

            player = await playerService.ConnectPlayerAsync(Context.GuildId, voiceChannelId.Value, Context.ChannelId);
        }

        // Look for files matching the query in the Music library folder.
        var files = Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "*", SearchOption.AllDirectories);
        var file = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file).Contains(query, StringComparison.OrdinalIgnoreCase));
        if (file == null)
        {
            return Response("No file found.");
        }

        var source = new FFmpegAudioSource(File.OpenRead(file));
        var title = Path.GetFileNameWithoutExtension(file);

        var queued = player.Enqueue(new Song(title, source));
        return Response($"{(queued ? "Queued" : "Playing")} {Markdown.Bold(title)}.");
    }

    [SlashCommand("pause")]
    [Description("Pauses the audio playback.")]
    public async Task<IResult> Pause()
    {
        var player = await playerService.GetPlayerAsync(Context.GuildId);
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
        var player = await playerService.GetPlayerAsync(Context.GuildId);
        if (player == null)
        {
            return Response("Not playing.");
        }

        if (!player.Resume())
        {
            return Response("Already playing.");
        }

        return Response("Resumed.");
    }

    [SlashCommand("skip")]
    [Description("Skips the currently playing audio track.")]
    public async Task<IResult> Skip()
    {
        var player = await playerService.GetPlayerAsync(Context.GuildId);
        if (player?.Source == null)
        {
            return Response("Not playing.");
        }

        // Setting the source to null stops it and starts the next queued source.
        player.Source = null;

        return Response("Skipped.");
    }

    [SlashCommand("stop")]
    [Description("Stops the playback and disconnects the bot from the voice channel.")]
    public async Task<IResult> Stop()
    {
        var player = await playerService.GetPlayerAsync(Context.GuildId);
        if (player == null)
        {
            return Response("Not playing.");
        }

        await playerService.DisposePlayerAsync(Context.GuildId);
        return Response("Disconnected.");
    }
}
