using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalApplicationRoleConnectionMetadata : ILocalConstruct<LocalApplicationRoleConnectionMetadata>, IJsonConvertible<ApplicationRoleConnectionMetadataJsonModel>
{
    /// <summary>
    ///     Gets or sets the type of this metadata.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<ApplicationRoleConnectionMetadataType> Type { get; set; }

    /// <summary>
    ///     Gets or sets the key of this metadata.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<string> Key { get; set; }

    /// <summary>
    ///     Gets or sets the name of this metadata.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<string> Name { get; set; }

    /// <summary>
    ///     Gets or sets the localizations of the name of this metadata.
    /// </summary>
    public Optional<IDictionary<CultureInfo, string>> NameLocalizations { get; set; }

    /// <summary>
    ///     Gets or sets the description of this metadata.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<string> Description { get; set; }

    /// <summary>
    ///     Gets or sets the localizations of the description of this metadata.
    /// </summary>
    public Optional<IDictionary<CultureInfo, string>> DescriptionLocalizations { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalApplicationRoleConnectionMetadata"/>.
    /// </summary>
    public LocalApplicationRoleConnectionMetadata()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalApplicationRoleConnectionMetadata"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalApplicationRoleConnectionMetadata(LocalApplicationRoleConnectionMetadata other)
    {
        Guard.IsNotNull(other);

        Type = other.Type;
        Key = other.Key;
        Name = other.Name;
        NameLocalizations = Optional.Convert(other.NameLocalizations, localizations => localizations.ToDictionary() as IDictionary<CultureInfo, string>);
        Description = other.Description;
        DescriptionLocalizations = Optional.Convert(other.DescriptionLocalizations, localizations => localizations.ToDictionary() as IDictionary<CultureInfo, string>);
    }

    /// <inheritdoc/>
    public LocalApplicationRoleConnectionMetadata Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public ApplicationRoleConnectionMetadataJsonModel ToModel()
    {
        OptionalGuard.HasValue(Type);
        OptionalGuard.HasValue(Key);
        OptionalGuard.HasValue(Name);
        OptionalGuard.HasValue(Description);

        return new ApplicationRoleConnectionMetadataJsonModel()
        {
            Type = Type.Value,
            Key = Key.Value,
            Name = Name.Value,
            NameLocalizations = Optional.Convert(NameLocalizations, localizations => localizations.ToDictionary(x => x.Key.Name, x => x.Value)),
            Description = Description.Value,
            DescriptionLocalizations = Optional.Convert(DescriptionLocalizations, localizations => localizations.ToDictionary(x => x.Key.Name, x => x.Value))
        };
    }
}
