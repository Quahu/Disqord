using System.Linq;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class UpdatePresenceJsonModel : JsonModel
{
    [JsonProperty("since")]
    public int? Since;

    [JsonProperty("activities")]
    public ActivityJsonModel[] Activities = null!;

    [JsonProperty("status")]
    public UserStatus Status;

    [JsonProperty("afk")]
    public bool Afk;

    public UpdatePresenceJsonModel Clone()
    {
        var @this = (UpdatePresenceJsonModel) MemberwiseClone();
        @this.Activities = Activities?.ToArray()!;
        return @this;
    }
}