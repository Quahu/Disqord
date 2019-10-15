using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord
{
    public sealed class CachedOverwrite : CachedSnowflakeEntity, IOverwrite
    {
        public CachedGuildChannel Channel { get; }

        public Snowflake TargetId { get; }

        public OverwritePermissions Permissions { get; }

        public OverwriteTargetType TargetType { get; }

        Snowflake IOverwrite.ChannelId => Channel.Id;

        internal CachedOverwrite(DiscordClient client, OverwriteModel model, CachedGuildChannel channel) : base(client, model.Id)
        {
            Channel = channel;
            TargetId = model.Id;
            Permissions = (model.Allow, model.Deny);
            TargetType = model.Type;
        }

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.RestClient.DeleteOverwriteAsync(Channel.Id, TargetId, options);

        public override string ToString()
            => $"{TargetType} overwrite: {Permissions}";
    }
}
