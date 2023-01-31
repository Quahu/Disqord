using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Qommon;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalApplicationRoleConnectionMetadataExtensions
{
    public static TAction WithType<TAction>(this TAction action, ApplicationRoleConnectionMetadataType type)
        where TAction : LocalApplicationRoleConnectionMetadata
    {
        action.Type = type;
        return action;
    }

    public static TApplicationRoleConnectionMetadata WithKey<TApplicationRoleConnectionMetadata>(this TApplicationRoleConnectionMetadata metadata, string key)
        where TApplicationRoleConnectionMetadata : LocalApplicationRoleConnectionMetadata
    {
        metadata.Key = key;
        return metadata;
    }

    public static TApplicationRoleConnectionMetadata WithName<TApplicationRoleConnectionMetadata>(this TApplicationRoleConnectionMetadata metadata, string name)
        where TApplicationRoleConnectionMetadata : LocalApplicationRoleConnectionMetadata
    {
        metadata.Name = name;
        return metadata;
    }

    public static TApplicationRoleConnectionMetadata AddNameLocalization<TApplicationRoleConnectionMetadata>(this TApplicationRoleConnectionMetadata metadata, CultureInfo locale, string nameLocalization)
        where TApplicationRoleConnectionMetadata : LocalApplicationRoleConnectionMetadata
    {
        Guard.IsNotNull(locale);
        Guard.IsNotNull(nameLocalization);

        if (metadata.NameLocalizations.Add(locale, nameLocalization, out var dictionary))
            metadata.NameLocalizations = new(dictionary);

        return metadata;
    }

    public static TApplicationRoleConnectionMetadata WithNameLocalizations<TApplicationRoleConnectionMetadata>(this TApplicationRoleConnectionMetadata metadata, IEnumerable<KeyValuePair<CultureInfo, string>> nameLocalizations)
        where TApplicationRoleConnectionMetadata : LocalApplicationRoleConnectionMetadata
    {
        Guard.IsNotNull(nameLocalizations);

        if (metadata.NameLocalizations.With(nameLocalizations, out var dictionary))
            metadata.NameLocalizations = new(dictionary);

        return metadata;
    }

    public static TApplicationRoleConnectionMetadata WithDescription<TApplicationRoleConnectionMetadata>(this TApplicationRoleConnectionMetadata metadata, string description)
        where TApplicationRoleConnectionMetadata : LocalApplicationRoleConnectionMetadata
    {
        metadata.Description = description;
        return metadata;
    }

    public static TApplicationRoleConnectionMetadata AddDescriptionLocalization<TApplicationRoleConnectionMetadata>(this TApplicationRoleConnectionMetadata metadata, CultureInfo locale, string descriptionLocalization)
        where TApplicationRoleConnectionMetadata : LocalApplicationRoleConnectionMetadata
    {
        Guard.IsNotNull(locale);
        Guard.IsNotNull(descriptionLocalization);

        if (metadata.DescriptionLocalizations.Add(locale, descriptionLocalization, out var dictionary))
            metadata.DescriptionLocalizations = new(dictionary);

        return metadata;
    }

    public static TApplicationRoleConnectionMetadata WithDescriptionLocalizations<TApplicationRoleConnectionMetadata>(this TApplicationRoleConnectionMetadata metadata, IEnumerable<KeyValuePair<CultureInfo, string>> descriptionLocalizations)
        where TApplicationRoleConnectionMetadata : LocalApplicationRoleConnectionMetadata
    {
        Guard.IsNotNull(descriptionLocalizations);

        if (metadata.DescriptionLocalizations.With(descriptionLocalizations, out var dictionary))
            metadata.DescriptionLocalizations = new(dictionary);

        return metadata;
    }
}
