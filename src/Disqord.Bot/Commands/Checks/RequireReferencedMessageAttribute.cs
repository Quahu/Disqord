using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Specifies that the module or command can only be executed in reply.
    /// </summary>
    public class RequireReferencedMessageAttribute : DiscordGuildCheckAttribute
    {
        public RequireReferencedMessageAttribute()
        { }

        public override ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context)
        {
            if (context.Message.ReferencedMessage.HasValue)
                return Success();

            return Failure($"This can only be executed in reply to another message.");
        }
    }
}