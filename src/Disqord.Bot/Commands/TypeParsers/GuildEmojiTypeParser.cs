using System;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Parsers;

/// <summary>
///     Represents type parsing for the <see cref="ICustomEmoji"/> type
///     via <see cref="LocalCustomEmoji.TryParse(string, out LocalCustomEmoji)"/>.
/// </summary>
public class GuildEmojiTypeParser : DiscordGuildTypeParser<IGuildEmoji>
{
    /// <inheritdoc/>
    public override ValueTask<ITypeParserResult<IGuildEmoji>> ParseAsync(IDiscordGuildCommandContext context, IParameter parameter, ReadOnlyMemory<char> value)
    {
        var guild = context.Bot.GetGuild(context.GuildId);
        if (guild == null)
            Throw.InvalidOperationException($"{nameof(GuildEmojiTypeParser)} requires the context guild to be cached.");

        if (LocalCustomEmoji.TryParse(value.Span, out var emoji))
        {
            if (guild.Emojis.TryGetValue(emoji.Id.Value, out var guildEmoji))
                return Success(new(guildEmoji));

            return Failure("The provided custom emoji is not from this guild.");
        }

        return Failure("Invalid custom emoji.");
    }
}
