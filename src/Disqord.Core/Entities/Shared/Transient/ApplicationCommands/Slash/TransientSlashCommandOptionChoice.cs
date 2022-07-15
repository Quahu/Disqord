using System.Collections.Generic;
using System.Globalization;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
    public class TransientSlashCommandOptionChoice : TransientClientEntity<ApplicationCommandOptionChoiceJsonModel>, ISlashCommandOptionChoice
    {
        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc />
        public IReadOnlyDictionary<CultureInfo, string> NameLocalizations
        {
            get
            {
                if (!Model.NameLocalizations.HasValue)
                    return ReadOnlyDictionary<CultureInfo, string>.Empty;

                return Model.NameLocalizations.Value.ToReadOnlyDictionary(x => CultureInfo.GetCultureInfo(x.Key), x => x.Value);
            }
        }

        /// <inheritdoc/>
        public object Value => Model.Value.Value;

        public TransientSlashCommandOptionChoice(IClient client, ApplicationCommandOptionChoiceJsonModel model)
            : base(client, model)
        { }
    }
}
