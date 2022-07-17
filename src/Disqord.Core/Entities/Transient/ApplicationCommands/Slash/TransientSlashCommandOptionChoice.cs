using System.Collections.Generic;
using System.Globalization;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientSlashCommandOptionChoice : TransientClientEntity<ApplicationCommandOptionChoiceJsonModel>, ISlashCommandOptionChoice
{
    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc />
    public IReadOnlyDictionary<CultureInfo, string> NameLocalizations
    {
        get
        {
            var localizations = Model.NameLocalizations.GetValueOrDefault();
            if (localizations == null)
                return ReadOnlyDictionary<CultureInfo, string>.Empty;

            return localizations.ToReadOnlyDictionary(x => CultureInfo.GetCultureInfo(x.Key), x => x.Value);
        }
    }

    /// <inheritdoc/>
    public object Value => Model.Value.Value!;

    public TransientSlashCommandOptionChoice(IClient client, ApplicationCommandOptionChoiceJsonModel model)
        : base(client, model)
    { }
}
