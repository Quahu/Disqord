using Disqord.Models;

namespace Disqord
{
    public class TransientOverwrite : TransientClientEntity<OverwriteJsonModel>, IOverwrite
    {
        public Snowflake ChannelId { get; }

        public Snowflake TargetId => Model.Id;

        public OverwriteTargetType TargetType => Model.Type;

        public OverwritePermissions Permissions => new(Model.Allow, Model.Deny);

        public TransientOverwrite(IClient client, Snowflake channelId, OverwriteJsonModel model)
            : base(client, model)
        {
            ChannelId = channelId;
        }
    }
}
