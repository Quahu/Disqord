using System.ComponentModel;
using System.Globalization;
using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientInteraction : TransientClientEntity<InteractionJsonModel>, IInteraction
    {
        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public long ReceivedAt { get; }

        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

        /// <inheritdoc/>
        public Snowflake ChannelId => Model.ChannelId.Value;

        /// <inheritdoc/>
        public Snowflake ApplicationId => Model.ApplicationId;

        /// <inheritdoc/>
        public int Version => Model.Version;

        /// <inheritdoc/>
        public InteractionType Type => Model.Type;

        /// <inheritdoc/>
        public string Token => Model.Token;

        /// <inheritdoc/>
        public IUser Author
        {
            get
            {
                return _author ??= Model.Member.HasValue
                    ? new TransientMember(Client, GuildId.Value, Model.Member.Value)
                    : new TransientUser(Client, Model.User.Value);
            }
        }
        private IUser _author;

        /// <inheritdoc/>
        public Permission AuthorPermissions
        {
            get
            {
                if (!Model.Member.HasValue || !Model.Member.Value.Permissions.HasValue)
                    return Permission.None;

                return (Permission) Model.Member.Value.Permissions.Value;
            }
        }

        /// <inheritdoc/>
        public CultureInfo Locale => Model.Locale.HasValue
            ? Discord.Internal.GetLocale(Model.Locale.Value)
            : null;

        /// <inheritdoc/>
        public CultureInfo GuildLocale => Model.GuildLocale.HasValue
            ? Discord.Internal.GetLocale(Model.GuildLocale.Value)
            : null;

        public TransientInteraction(IClient client, long receivedAt, InteractionJsonModel model)
            : base(client, model)
        {
            ReceivedAt = receivedAt;
        }

        public static IInteraction Create(IClient client, long receivedAt, InteractionJsonModel model)
            => model.Type switch
            {
                InteractionType.ApplicationCommand => model.Data.Value.Type.Value switch
                {
                    ApplicationCommandType.Slash => new TransientSlashCommandInteraction(client, receivedAt, model),
                    ApplicationCommandType.User or ApplicationCommandType.Message => new TransientContextMenuInteraction(client, receivedAt, model),
                    _ => new TransientApplicationCommandInteraction(client, receivedAt, model)
                },
                InteractionType.MessageComponent => new TransientComponentInteraction(client, receivedAt, model),
                InteractionType.ApplicationCommandAutoComplete => new TransientAutoCompleteInteraction(client, receivedAt, model),
                InteractionType.ModalSubmit => new TransientModalSubmitInteraction(client, receivedAt, model),
                _ => new TransientInteraction(client, receivedAt, model)
            };
    }
}
