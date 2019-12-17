using System.Threading.Tasks;
using Disqord.Models;
using Disqord.Rest;

namespace Disqord
{
    public sealed class CachedOverwrite : CachedSnowflakeEntity, IOverwrite
    {
        public CachedGuildChannel Channel { get; }

        public Snowflake TargetId { get; }

        public OverwritePermissions Permissions { get; }

        public OverwriteTargetType TargetType { get; }

        Snowflake IOverwrite.ChannelId => Channel.Id;

        internal CachedOverwrite(CachedGuildChannel channel, OverwriteModel model) : base(channel.Client, model.Id)
        {
            Channel = channel;
            TargetId = model.Id;
            Permissions = (model.Allow, model.Deny);
            TargetType = model.Type;
        }

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteOverwriteAsync(Channel.Id, TargetId, options);

        public override string ToString()
            => $"{TargetType} overwrite: {Permissions}";
    }
}
