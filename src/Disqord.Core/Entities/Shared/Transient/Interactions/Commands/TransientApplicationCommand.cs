using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientApplicationCommand : TransientEntity<ApplicationCommandJsonModel>, IApplicationCommand
    {
        public ApplicationCommandType? Type => Model.Type.GetValueOrNullable();

        public Snowflake ApplicationId => Model.ApplicationId;

        public string Description => Model.Description;

        public virtual IReadOnlyList<IApplicationCommandOption> Options => _options ??= Model.Options.Value.ToReadOnlyList(this, (x, @this) => new TransientApplicationCommandOption(@this.Client, x));
        private IReadOnlyList<IApplicationCommandOption> _options;

        public bool? IsEnabledByDefault => Model.DefaultPermission.GetValueOrNullable();

        public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

        public Snowflake Id => Model.Id;

        public string Name => Model.Name;

        public Snowflake Version => Model.Version;

        public TransientApplicationCommand(IClient client, ApplicationCommandJsonModel model)
            : base(client, model)
        { }
    }
}
