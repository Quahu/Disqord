using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientApplicationCommandOption : TransientEntity<ApplicationCommandOptionJsonModel>, IApplicationCommandOption
    {
        public ApplicationCommandOptionType Type => Model.Type;

        public string Description => Model.Description;

        public bool? Required => Model.Required.GetValueOrNullable();

        public IReadOnlyList<IApplicationCommandOptionChoice> Choices => _choices ??= Model.Choices.Value.ToReadOnlyList(this, (x, @this) => new TransientApplicationCommandOptionChoice(@this.Client, x));
        private IReadOnlyList<IApplicationCommandOptionChoice> _choices;

        public virtual IReadOnlyList<IApplicationCommandOption> Options => _options ??= Model.Options.Value.ToReadOnlyList(this, (x, @this) => new TransientApplicationCommandOption(@this.Client, x));
        private IReadOnlyList<IApplicationCommandOption> _options;

        public string Name => Model.Name;

        public TransientApplicationCommandOption(IClient client, ApplicationCommandOptionJsonModel model)
            : base(client, model)
        { }
    }
}
