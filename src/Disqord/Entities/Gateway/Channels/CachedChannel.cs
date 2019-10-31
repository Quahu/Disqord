using Disqord.Models;

namespace Disqord
{
    public abstract class CachedChannel : CachedSnowflakeEntity, IChannel
    {
        public string Name { get; private set; }

        internal CachedChannel(DiscordClientBase client, ChannelModel model) : base(client, model.Id)
        { }

        internal virtual void Update(ChannelModel model)
        {
            if (model.Name.HasValue)
                Name = model.Name.Value;
        }

        internal CachedChannel Clone()
            => (CachedChannel) MemberwiseClone();

        public override string ToString()
            => Name;
    }
}
