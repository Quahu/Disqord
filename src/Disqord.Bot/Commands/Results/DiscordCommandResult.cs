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

        public DiscordCommandContext Context { get; protected set; }

        protected DiscordCommandResult(DiscordCommandContext context)
        {
            Context = context;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual TaskAwaiter GetAwaiter()
            => ExecuteAsync().GetAwaiter();

        public abstract Task ExecuteAsync();
    }
}
