using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public sealed class RequireNsfwAttribute : CheckAttribute
    {
        public bool AllowPrivateChannels { get; set; } = true;

        public RequireNsfwAttribute()
        { }

        public override ValueTask<CheckResult> CheckAsync(CommandContext _)
        {
            var context = _ as DiscordCommandContext;
            return context.Channel is CachedTextChannel textChannel && textChannel.IsNsfw || AllowPrivateChannels && context.Channel is CachedPrivateChannel
                ? CheckResult.Successful
                : CheckResult.Unsuccessful($"This can only be executed in NSFW{(AllowPrivateChannels ? " or private" : "")} channels.");
        }
    }
}
