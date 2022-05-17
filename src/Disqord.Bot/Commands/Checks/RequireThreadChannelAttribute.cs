using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Specifies that the module or command can only be executed in thread channel.
    /// </summary>
    public class RequireThreadChannelAttribute : DiscordGuildCheckAttribute
    {
        public RequireThreadChannelAttribute()
        { }

        public override ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context)
        {
            if (context.Channel is IThreadChannel)
                return Success();

            return Failure($"This can only be executed in thread channel.");
        }
    }
}