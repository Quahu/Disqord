using System.ComponentModel.DataAnnotations;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Api.Default;

public class DefaultShardConfiguration
{
    [Required]
    public virtual GatewayIntents Intents { get; set; } = GatewayIntents.Unprivileged;

    [Range(50, 250)]
    public virtual int LargeGuildThreshold { get; set; } = 250;

    public virtual UpdatePresenceJsonModel? Presence { get; set; }

    public DefaultShardConfiguration Clone()
    {
        var @this = (DefaultShardConfiguration) MemberwiseClone();
        @this.Presence = Presence?.Clone();
        return @this;
    }
}
