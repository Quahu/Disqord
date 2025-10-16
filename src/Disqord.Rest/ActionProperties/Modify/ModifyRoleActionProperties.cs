using System;
using System.IO;
using Qommon;

namespace Disqord;

public sealed class ModifyRoleActionProperties
{
    public Optional<string> Name { internal get; set; }

    public Optional<Permissions> Permissions { internal get; set; }

    [Obsolete("The legacy role \"Color\" is being replaced with the newer role \"Colors\" object, and may no longer be supported in a future API version.")]
    public Optional<Color?> Color { internal get; set; }
    
    public Optional<RoleColors?> Colors { internal get; set; }

    public Optional<bool> IsHoisted { internal get; set; }

    public Optional<Stream> Icon { internal get; set; }

    public Optional<bool> IsMentionable { internal get; set; }

    public Optional<LocalEmoji> UnicodeEmoji { internal get; set; }

    public Optional<int> Position { internal get; set; }

    internal bool HasValues
        => Name.HasValue || Permissions.HasValue || Color.HasValue || Colors.HasValue || IsHoisted.HasValue || Icon.HasValue || IsMentionable.HasValue || UnicodeEmoji.HasValue || Position.HasValue;
}
