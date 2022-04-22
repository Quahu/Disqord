using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientMessageReference : TransientEntity<MessageReferenceJsonModel>, IMessageReference
    {
        /// <inheritdoc/>
        public Snowflake? MessageId => Model.MessageId.GetValueOrNullable();

        /// <inheritdoc/>
        public Snowflake ChannelId => Model.ChannelId.Value;

        /// <inheritdoc/>
        public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

        public TransientMessageReference(MessageReferenceJsonModel model)
            : base(model)
        { }

        public override string ToString()
            => this.GetString();
    }
}
