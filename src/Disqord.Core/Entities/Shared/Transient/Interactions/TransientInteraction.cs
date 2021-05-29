using System;
using Disqord.Models;

namespace Disqord.Interaction
{
    public class TransientInteraction : TransientEntity<InteractionJsonModel>, IInteraction
    {
        public Snowflake Id => Model.Id;

        public DateTimeOffset CreatedAt => Id.CreatedAt;

        public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

        public Snowflake ChannelId => Model.ChannelId.Value;

        public Snowflake ApplicationId => Model.ApplicationId;

        public int Version => Model.Version;

        public InteractionType Type => Model.Type;

        public string Token => Model.Token;

        public IUser Author => _author ??= Model.Member.HasValue
            ? new TransientMember(Client, GuildId.Value, Model.Member.Value)
            : new TransientUser(Client, Model.User.Value);
        private IUser _author;

        public TransientInteraction(IClient client, InteractionJsonModel model)
            : base(client, model)
        { }

        public static IInteraction Create(IClient client, InteractionJsonModel model)
            => model.Type switch
            {
                InteractionType.MessageComponent => new TransientComponentInteraction(client, model),
                _ => new TransientInteraction(client, model)
            };
    }
}
