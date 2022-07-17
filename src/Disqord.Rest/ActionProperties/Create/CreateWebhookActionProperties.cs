using System.IO;
using Qommon;

namespace Disqord;

public sealed class CreateWebhookActionProperties
{
    public Optional<Stream> Avatar { internal get; set; }
}