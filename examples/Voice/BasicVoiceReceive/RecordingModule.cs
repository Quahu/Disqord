using System.Linq;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot.Commands.Application;
using Disqord.Gateway;
using Qmmands;

namespace BasicVoiceReceive;

public class RecordingModule(RecordingService recordingService) : DiscordApplicationGuildModuleBase
{
    [SlashCommand("record")]
    [Description("Starts recording voice in a channel.")]
    public async Task<IResult> Record(
        [Description("A specific user to record. Leave empty for the entire channel.")] IMember? user = null,
        [Description("The channel to record in."), ChannelTypes(ChannelType.Voice)] IInteractionChannel? channel = null)
    {
        await Deferral();

        var channelId = channel?.Id ?? Context.Author.GetVoiceState()?.ChannelId;
        if (channelId == null)
        {
            return Response("Please provide a voice channel to record in.");
        }

        var started = await recordingService.StartRecordingAsync(Context.GuildId, channelId.Value, user?.Id);
        if (!started)
        {
            return Response("Already recording in this guild.");
        }

        var responseText = user != null
            ? $"Recording {Markdown.Bold(user.Nick ?? user.Name)}."
            : "Recording the channel.";

        return Response($"{responseText} Use `/stop` to finish.");
    }

    [SlashCommand("stop")]
    [Description("Stops the active recording and uploads the result.")]
    public async Task<IResult> Stop()
    {
        await Deferral();

        var streams = await recordingService.StopRecordingAsync(Context.GuildId);
        if (streams == null)
        {
            return Response("Not recording.");
        }

        if (streams.Count == 0)
        {
            return Response("Stopped recording, but no audio was captured.");
        }

        var streamsToUpload = streams.Take(10).ToArray();
        var omittedCount = streams.Count - streamsToUpload.Length;
        var content = $"Recording complete for {streamsToUpload.Length} user(s).";
        if (omittedCount > 0)
        {
            content += $" Omitted {omittedCount} recording(s) due to attachment limits.";
        }

        var response = new LocalInteractionMessageResponse()
            .WithContent(content);

        foreach (var (userId, stream) in streamsToUpload)
        {
            response.AddAttachment(new LocalAttachment(stream, $"recording-{userId}.ogg"));
        }

        return Response(response);
    }
}
