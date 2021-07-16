using Disqord.Models;

namespace Disqord
{
    public class TransientPartialChannel : TransientEntity<ChannelJsonModel>, IPartialChannel
    {
        public Snowflake Id => Model.Id;

        public string Name => Model.Name.Value;

        public ChannelType Type => Model.Type;

        public ChannelPermissions Permissions => Model.Permissions.Value;

        public TransientPartialChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}