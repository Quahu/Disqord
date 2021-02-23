using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordCommandResult : CommandResult
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool IsSuccessful => true;

        public DiscordCommandContext Context { get; set; }

        public abstract Task ExecuteAsync();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public TaskAwaiter GetAwaiter()
            => ExecuteAsync().GetAwaiter();
    }
}
