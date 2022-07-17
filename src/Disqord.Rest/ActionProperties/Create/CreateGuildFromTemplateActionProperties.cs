using System.IO;
using Qommon;

namespace Disqord;

public sealed class CreateGuildFromTemplateActionProperties
{
    public Optional<Stream> Icon { internal get; set; }
}