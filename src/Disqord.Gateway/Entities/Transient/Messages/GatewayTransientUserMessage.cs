using Disqord.Gateway;
using Disqord.Models;

namespace Disqord
{
    public class GatewayTransientUserMessage : TransientUserMessage, IGatewayUserMessage
    {
        public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

        public GatewayTransientUserMessage(IClient client, MessageJsonModel model)
            : base(client, model)
        { }
    }
}
