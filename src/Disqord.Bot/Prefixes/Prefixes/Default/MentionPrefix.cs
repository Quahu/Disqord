using System;
using Disqord.Gateway;

namespace Disqord.Bot;

/// <summary>
///     Represents a mention prefix for the given <see cref="UserId"/>.
/// </summary>
public sealed class MentionPrefix : IPrefix
{
    /// <summary>
    ///     Gets the user ID to check for in the message content.
    /// </summary>
    public Snowflake UserId { get; }

    /// <summary>
    ///     Instantiates a new <see cref="MentionPrefix"/> with the specified user ID.
    /// </summary>
    /// <param name="userId"> The user ID. </param>
    public MentionPrefix(Snowflake userId)
    {
        UserId = userId;
    }

    /// <inheritdoc/>
    public bool TryFind(IGatewayUserMessage message, out ReadOnlyMemory<char> output)
    {
        var content = message.Content.AsMemory();
        var contentSpan = content.Span;
        if (contentSpan.Length > 17 && contentSpan[0] == '<' && contentSpan[1] == '@')
        {
            var closingBracketIndex = contentSpan.IndexOf('>');
            if (closingBracketIndex != -1)
            {
                var offset = contentSpan[2] == '!' ? 3 : 2;
                if (Snowflake.TryParse(contentSpan.Slice(offset, closingBracketIndex - offset), out var id) && id == UserId)
                {
                    output = content.Slice(closingBracketIndex + 1);
                    return true;
                }
            }
        }

        output = null;
        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
        => UserId.GetHashCode();

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is MentionPrefix prefix)
            return UserId == prefix.UserId;

        if (obj is Snowflake id)
            return UserId == id;

        if (obj is ulong rawId)
            return UserId == rawId;

        return false;
    }

    /// <inheritdoc/>
    public override string ToString()
        => Mention.User(UserId);
}
