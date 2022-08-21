using System.Collections.Generic;
using System.Globalization;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientSlashCommandOption : TransientClientEntity<ApplicationCommandOptionJsonModel>, ISlashCommandOption
{
    /// <inheritdoc/>
    public SlashCommandOptionType Type => Model.Type;

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
    public bool IsRequired => Model.Required.GetValueOrDefault();

    /// <inheritdoc/>
    public IReadOnlyList<ISlashCommandOptionChoice> Choices
    {
        get
        {
            if (!Model.Choices.HasValue)
                return ReadOnlyList<ISlashCommandOptionChoice>.Empty;

            return _choices ??= Model.Choices.Value.ToReadOnlyList(Client, (model, client) => new TransientSlashCommandOptionChoice(client, model));
        }
    }
    private IReadOnlyList<ISlashCommandOptionChoice>? _choices;

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

    /// <inheritdoc/>
    public IReadOnlyList<ChannelType> ChannelTypes
    {
        get
        {
            if (!Model.ChannelTypes.HasValue)
                return ReadOnlyList<ChannelType>.Empty;

            return Model.ChannelTypes.Value.ReadOnly();
        }
    }

    /// <inheritdoc/>
    public double? MinimumValue => Model.MinValue.GetValueOrNullable();

    /// <inheritdoc/>
    public double? MaximumValue => Model.MaxValue.GetValueOrNullable();

    /// <inheritdoc/>
    public int? MinimumLength => Model.MinLength.GetValueOrNullable();

    /// <inheritdoc/>
    public int? MaximumLength => Model.MaxLength.GetValueOrNullable();

    /// <inheritdoc/>
    public bool HasAutoComplete => Model.AutoComplete.GetValueOrDefault();

    public TransientSlashCommandOption(IClient client, ApplicationCommandOptionJsonModel model)
        : base(client, model)
    { }
}
