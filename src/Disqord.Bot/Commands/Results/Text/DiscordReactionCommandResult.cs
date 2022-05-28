﻿using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Bot.Commands.Text;

public class DiscordReactionCommandResult : DiscordCommandResult<IDiscordTextCommandContext>
{
    public LocalEmoji Emoji { get; protected set; }

    public DiscordReactionCommandResult(IDiscordTextCommandContext context, LocalEmoji emoji)
        : base(context)
    {
        Emoji = emoji;
    }

    public override Task ExecuteAsync()
    {
        return Context.Message.AddReactionAsync(Emoji);
    }
}
