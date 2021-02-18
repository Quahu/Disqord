using System.ComponentModel;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordCommandResult : CommandResult
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool IsSuccessful => true;

        public abstract Task ExecuteAsync(DiscordCommandContext context);
    }
}
