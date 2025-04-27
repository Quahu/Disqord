using System.Collections.Generic;
using Qommon;

namespace Disqord;

public sealed class ModifyWebhookMessageActionProperties
{
    public Optional<string?> Content { internal get; set; }

    public Optional<IEnumerable<LocalEmbed>> Embeds { internal get; set; }

    public Optional<LocalAllowedMentions> AllowedMentions { internal get; set; }

    public Optional<IEnumerable<LocalPartialAttachment>> Attachments { internal get; set; }

    public Optional<IEnumerable<LocalComponent>> Components { internal get; set; }

    public Optional<MessageFlags> Flags { internal get; set; }

    internal ModifyWebhookMessageActionProperties()
    { }
}
