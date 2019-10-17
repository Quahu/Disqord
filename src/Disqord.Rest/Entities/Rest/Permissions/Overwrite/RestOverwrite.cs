using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestOverwrite : RestDiscordEntity, IOverwrite
    {
        public Snowflake ChannelId { get; }

        public RestDownloadable<RestGuildChannel> Channel { get; }

        public Snowflake TargetId { get; }

        public OverwritePermissions Permissions { get; }

        public OverwriteTargetType TargetType { get; }

        internal RestOverwrite(RestDiscordClient client, OverwriteModel model, Snowflake channelId) : base(client)
        {
            ChannelId = channelId;
            Channel = new RestDownloadable<RestGuildChannel>(options => Client.GetChannelAsync<RestGuildChannel>(channelId, options));
            TargetId = model.Id;
            Permissions = (model.Allow, model.Deny);
            TargetType = model.Type;
        }

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteOverwriteAsync(ChannelId, TargetId, options);

        public override string ToString()
            => $"{TargetType} overwrite: {Permissions}";
    }
}
