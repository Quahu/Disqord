using System.IO;
using Qommon;

namespace Disqord.Rest;

public sealed class ModifyCurrentMemberActionProperties
{
    public Optional<string?> Nick { internal get; set; }

    public Optional<Stream?> Avatar { internal get; set; }

    public Optional<Stream?> Banner { internal get; set; }

    public Optional<string?> Bio { internal get; set; }
}
