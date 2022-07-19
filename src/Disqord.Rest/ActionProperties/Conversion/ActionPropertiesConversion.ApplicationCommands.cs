using System;
using System.Linq;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

internal static partial class ActionPropertiesConversion
{
    public static ModifyApplicationCommandJsonRestRequestContent ToContent(this Action<ModifyApplicationCommandActionProperties> action, IJsonSerializer serializer)
    {
        Guard.IsNotNull(action);

        var properties = new ModifyApplicationCommandActionProperties();
        action(properties);

        var content = new ModifyApplicationCommandJsonRestRequestContent
        {
            Name = properties.Name,
            NameLocalizations = Optional.Convert(properties.NameLocalizations, localizations => localizations.ToDictionary(x => x.Key.Name, x => x.Value)),
            Description = properties.Description,
            DescriptionLocalizations = Optional.Convert(properties.DescriptionLocalizations, localizations => localizations.ToDictionary(x => x.Key.Name, x => x.Value)),
            Options = Optional.Convert(properties.Options, options => options?.Select(option => option.ToModel(serializer)).ToArray())!,
            DefaultMemberPermissions = Optional.Convert(properties.DefaultRequiredMemberPermissions, defaultMemberPermissions => (Permissions?) defaultMemberPermissions),
            DmPermission = properties.IsEnabledInPrivateChannels,
            DefaultPermission = properties.IsEnabledByDefault,
        };

        return content;
    }
}
