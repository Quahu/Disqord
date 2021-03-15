using Disqord.Gateway;
using Disqord.Models;

namespace Disqord
{
    public class GatewayTransientUserMessage : TransientUserMessage, IGatewayUserMessage
    {
        public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

        public override IUser Author
        {
            get
            {
                if (!Model.GuildId.HasValue)
                    return base.Author;

                if (_author == null)
                {
                    // Following trick lets us not duplicate logic.
                    Model.Member.Value.User = Model.Author;
                    _author = new TransientMember(Client, GuildId.Value, Model.Member.Value);
                }

                return _author as IMember;
            }
        }

        public GatewayTransientUserMessage(IClient client, MessageJsonModel model)
            : base(client, model)
        { }
    }
}
