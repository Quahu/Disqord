using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Bot
{
    public class DiscordTemporaryResponseCommandResult : DiscordCommandResult
    {
        public DiscordResponseCommandResult Result { get; protected set; }

        public TimeSpan Delay { get; protected set; }

        public DiscordTemporaryResponseCommandResult(DiscordResponseCommandResult result, TimeSpan delay)
            : base(result.Context)
        {
            Result = result;
            Delay = delay;
        }

        public override async Task ExecuteAsync()
        {
            var message = await Result.ExecuteAsync().ConfigureAwait(false);
            await Task.Delay(Delay).ConfigureAwait(false);
            await message.DeleteAsync().ConfigureAwait(false);
        }
    }
}
