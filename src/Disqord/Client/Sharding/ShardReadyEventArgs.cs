using Disqord.Events;

namespace Disqord.Sharding
{
    public sealed class ShardReadyEventArgs : ReadyEventArgs
    {
        public Shard Shard { get; }

        internal ShardReadyEventArgs(DiscordClientBase client, string sessionId, string[] trace, Shard shard)
            : base(client, sessionId, trace)
        {
            Shard = shard;
        }
    }
}
