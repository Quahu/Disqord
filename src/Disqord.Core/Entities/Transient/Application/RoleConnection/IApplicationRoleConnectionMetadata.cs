using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientApplicationRoleConnectionMetadata : TransientEntity<ApplicationRoleConnectionMetadataJsonModel>, IApplicationRoleConnectionMetadata
{
    /// <inheritdoc/>
    public ApplicationRoleConnectionMetadataType Type => Model.Type;

    /// <inheritdoc/>
    public string Key => Model.Key;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, string> NameLocalizations
    {
        get
        {
            if (!Model.NameLocalizations.HasValue)
                return ReadOnlyDictionary<string, string>.Empty;

            return Model.NameLocalizations.Value;
        }
    }

    /// <inheritdoc/>
    public string Description => Model.Description;

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, string> DescriptionLocalizations
    {
        get
        {
            if (!Model.DescriptionLocalizations.HasValue)
                return ReadOnlyDictionary<string, string>.Empty;

            return Model.DescriptionLocalizations.Value;
        }
    }

    public TransientApplicationRoleConnectionMetadata(ApplicationRoleConnectionMetadataJsonModel model)
        : base(model)
    { }
}
