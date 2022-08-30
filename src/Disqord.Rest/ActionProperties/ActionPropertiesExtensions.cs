using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Qommon;

namespace Disqord.Rest;

internal static class ActionPropertiesExtensions
{
    public static bool TryGetFullAttachments(this Optional<IEnumerable<LocalPartialAttachment>> optional, [MaybeNullWhen(false)] out LocalAttachment[] attachments)
    {
        if (!optional.HasValue)
        {
            attachments = null;
            return false;
        }

        attachments = optional.Value.OfType<LocalAttachment>().ToArray();
        return attachments.Length != 0;
    }
}
