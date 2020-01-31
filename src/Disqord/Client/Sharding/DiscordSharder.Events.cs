using Qommon.Events;

namespace Disqord.Sharding
{
    public partial class DiscordSharder : DiscordClientBase, IDiscordSharder
    {
        public event AsynchronousEventHandler<ShardReadyEventArgs> ShardReady
        {
            add => (this as IDiscordSharder)._shardReady.Hook(value);
            remove => (this as IDiscordSharder)._shardReady.Unhook(value);
        }
        AsynchronousEvent<ShardReadyEventArgs> IDiscordSharder._shardReady { get; set; }
    }
}
