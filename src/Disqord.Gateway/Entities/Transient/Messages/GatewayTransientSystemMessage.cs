using Disqord.Gateway;
using Disqord.Models;

namespace Disqord
{
    public class GatewayTransientSystemMessage : TransientSystemMessage, IGatewayMessage
    {
        public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

        public GatewayTransientSystemMessage(IClient client, MessageJsonModel model)
            : base(client, model)
        { }
    }
}
