using System.Collections.Generic;
using System.Globalization;
using Qommon;

namespace Disqord.Rest;

public sealed class ModifyApplicationCommandActionProperties
{
    public Optional<string> Name { internal get; set; }

    public Optional<IEnumerable<KeyValuePair<CultureInfo, string>>> NameLocalizations { internal get; set; }

    public Optional<string> Description { internal get; set; }

    public Optional<IEnumerable<KeyValuePair<CultureInfo, string>>> DescriptionLocalizations { internal get; set; }

    public Optional<IEnumerable<LocalSlashCommandOption>> Options { internal get; set; }

    public Optional<Permissions> DefaultRequiredMemberPermissions { internal get; set; }

    public Optional<bool> IsEnabledInPrivateChannels { internal get; set; }

    public Optional<bool> IsEnabledByDefault { internal get; set; }
}