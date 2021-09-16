﻿using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientApplicationCommand : TransientEntity<ApplicationCommandJsonModel>, IApplicationCommand
    {
        public Snowflake Id => Model.Id;

        public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

        public string Name => Model.Name;

        public ApplicationCommandType Type => Model.Type.GetValueOrDefault(ApplicationCommandType.Text);

        public Snowflake ApplicationId => Model.ApplicationId;

        public string Description => Model.Description;

        public virtual IReadOnlyList<IApplicationCommandOption> Options => _options ??= Model.Options.Value.ToReadOnlyList(Client, (model, client) => new TransientApplicationCommandOption(client, model));
        private IReadOnlyList<IApplicationCommandOption> _options;

        public bool IsEnabledByDefault => Model.DefaultPermission.GetValueOrDefault();

        public Snowflake Version => Model.Version;

        public TransientApplicationCommand(IClient client, ApplicationCommandJsonModel model)
            : base(client, model)
        { }
    }
}
