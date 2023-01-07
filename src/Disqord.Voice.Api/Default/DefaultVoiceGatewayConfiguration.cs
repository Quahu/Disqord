namespace Disqord.Voice.Api.Default;

public class DefaultVoiceGatewayConfiguration
{
    public virtual int Version { get; set; } = Library.Constants.VoiceVersion;

    public virtual bool LogsPayloads { get; set; }
}