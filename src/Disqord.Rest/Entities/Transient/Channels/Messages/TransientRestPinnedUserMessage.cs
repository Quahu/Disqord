using System;
using Disqord.Models;

namespace Disqord.Rest;

public class TransientRestPinnedUserMessage(IClient client, MessageJsonModel model, DateTimeOffset pinnedAt)
    : TransientUserMessage(client, model), IRestPinnedUserMessage
{
    public DateTimeOffset PinnedAt { get; } = pinnedAt;
}
