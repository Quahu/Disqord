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

        public virtual DiscordCommandContext Context { get; }

        protected DiscordCommandResult(DiscordCommandContext context)
        {
            Context = context;
        }

        public abstract Task ExecuteAsync();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual TaskAwaiter GetAwaiter()
            => ExecuteAsync().GetAwaiter();
    }
}
