using System.Collections.Generic;
using System.Globalization;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientSlashCommand : TransientApplicationCommand, ISlashCommand
{
    /// <inheritdoc/>
    public string Description => Model.Description;

    /// <inheritdoc />
    public IReadOnlyDictionary<CultureInfo, string> DescriptionLocalizations
    {
        get
        {
            var localizations = Model.DescriptionLocalizations.GetValueOrDefault();
            if (localizations == null)
                return ReadOnlyDictionary<CultureInfo, string>.Empty;

            return localizations.ToReadOnlyDictionary(x => CultureInfo.GetCultureInfo(x.Key), x => x.Value);
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<ISlashCommandOption> Options
    {
        get
        {
            if (!Model.Options.HasValue)
                return ReadOnlyList<ISlashCommandOption>.Empty;

            return _options ??= Model.Options.Value.ToReadOnlyList(Client, (model, client) => new TransientSlashCommandOption(client, model));
        }
    }
    private IReadOnlyList<ISlashCommandOption>? _options;

    public TransientSlashCommand(IClient client, ApplicationCommandJsonModel model)
        : base(client, model)
    { }
}
