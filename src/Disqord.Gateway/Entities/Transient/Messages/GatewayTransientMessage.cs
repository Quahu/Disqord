using Disqord.Api;
using Disqord.Gateway;
using Disqord.Models;

namespace Disqord
{
    public abstract class GatewayTransientMessage : TransientMessage, IGatewayMessage
    {
        public override IUser Author
        {
            get
            {
                if (!Model.GuildId.HasValue)
                    return base.Author;

                if (_author == null)
                {
                    // Following trick let's us not duplicate logic.
                    Model.Member.Value.User = Model.Author;
                    _author = new TransientMember(Client, GuildId.Value, Model.Member.Value);
                }

                return _author as IMember;
            }
        }

        public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

        protected GatewayTransientMessage(IClient client, MessageJsonModel model)
            : base(client, model)
        { }

        /// <summary>
        ///     Creates either a <see cref="GatewayTransientUserMessage"/> or a <see cref="TransientSystemMessage"/> based on the type.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static new IGatewayMessage Create(IClient client, MessageJsonModel model)
        {
            switch ((MessageType) model.Type)
            {
                case MessageType.Default:
                case MessageType.Reply:
                case MessageType.ApplicationCommand:
                    return new GatewayTransientUserMessage(client, model);

                default:
                    return new GatewayTransientSystemMessage(client, model);
            }
        }
    }
}
