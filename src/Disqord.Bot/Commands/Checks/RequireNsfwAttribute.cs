﻿using System;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed in NSFW guild channels.
/// </summary>
public class RequireNsfwAttribute : DiscordGuildCheckAttribute
{
    public override ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context)
    {
        if (context.Bot.GetChannel(context.GuildId, context.ChannelId) is not IGuildChannel channel)
            throw new InvalidOperationException($"{nameof(RequireNsfwAttribute)} requires the context channel.");

        var isNsfw = channel switch
        {
            CachedTextChannel textChannel => textChannel.IsAgeRestricted,
            CachedVoiceChannel voiceChannel => voiceChannel.IsAgeRestricted,
            CachedForumChannel forumChannel => forumChannel.IsAgeRestricted,
            CachedThreadChannel threadChannel => threadChannel.GetChannel()?.IsAgeRestricted ?? false,
            _ => false
        };

        if (isNsfw)
            return Results.Success;

        return Results.Failure("This can only be executed in a NSFW channel.");
    }
}
