using System;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireNsfwAttribute : DiscordGuildCheckAttribute
    {
        public override ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context)
        {
            if (context.Channel is null)
                throw new InvalidOperationException($"{nameof(RequireNsfwAttribute)} requires the context channel.");

            var isNsfw = context.Channel switch
            {
                CachedTextChannel textChannel => textChannel.IsNsfw,
                CachedThreadChannel threadChannel => threadChannel.GetChannel()?.IsNsfw ?? false,
                _ => false
            };

            if (isNsfw)
                return Success();

            return Failure("This can only be executed in a NSFW channel.");
        }
    }
}
