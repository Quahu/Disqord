using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord
{
    public class TransientApplicationCommandOptionChoice : TransientEntity<ApplicationCommandOptionChoiceJsonModel>, IApplicationCommandOptionChoice
    {
        public object Value => Model.Value.Value;

        public string Name => Model.Name;

        public TransientApplicationCommandOptionChoice(IClient client, ApplicationCommandOptionChoiceJsonModel model)
            : base(client, model)
        { }
    }
}
