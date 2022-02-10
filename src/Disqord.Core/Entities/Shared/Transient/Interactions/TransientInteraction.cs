﻿using System.Globalization;
using Disqord.Models;

namespace Disqord.Interaction
{
    public class TransientInteraction : TransientClientEntity<InteractionJsonModel>, IInteraction
    {
        public Snowflake Id => Model.Id;

        public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

        public Snowflake ChannelId => Model.ChannelId.Value;

        public Snowflake ApplicationId => Model.ApplicationId;

        public int Version => Model.Version;

        public InteractionType Type => Model.Type;

        public string Token => Model.Token;

        public IUser Author => _author ??= Model.Member.HasValue
            ? new TransientMember(Client, GuildId.Value, Model.Member.Value)
            : new TransientUser(Client, Model.User.Value);

        public CultureInfo Locale => Model.Locale.HasValue
            ? Discord.Internal.GetLocale(Model.Locale.Value)
            : null;

        public CultureInfo GuildLocale => Model.GuildLocale.HasValue
            ? Discord.Internal.GetLocale(Model.GuildLocale.Value)
            : null;

        private IUser _author;

        public TransientInteraction(IClient client, InteractionJsonModel model)
            : base(client, model)
        { }

        public static IInteraction Create(IClient client, InteractionJsonModel model)
            => model.Type switch
            {
                InteractionType.ApplicationCommand => model.Data.Value.Type.Value switch
                {
                    ApplicationCommandType.Slash => new TransientSlashCommandInteraction(client, model),
                    ApplicationCommandType.User or ApplicationCommandType.Message => new TransientContextMenuInteraction(client, model),
                    _ => new TransientApplicationCommandInteraction(client, model)
                },
                InteractionType.MessageComponent => new TransientComponentInteraction(client, model),
                InteractionType.ApplicationCommandAutoComplete => new TransientSlashCommandAutoCompleteInteraction(client, model),
                _ => new TransientInteraction(client, model)
            };
    }
}
