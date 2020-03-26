using System;
using System.Globalization;
using Disqord.Rest;

namespace Disqord
{
    public static partial class Discord
    {
        internal static class Internal
        {
            internal static string GetAvatarUrl(RestWebhook webhook, ImageFormat format = default, int size = 2048)
                => webhook.AvatarHash != null
                    ? Cdn.GetUserAvatarUrl(webhook.Id, webhook.AvatarHash, format, size)
                    : Cdn.GetDefaultUserAvatarUrl(DefaultAvatarColor.Blurple);

            internal static string GetAvatarUrl(IUser user, ImageFormat format = default, int size = 2048)
                => user.AvatarHash != null
                    ? Cdn.GetUserAvatarUrl(user.Id, user.AvatarHash, format, size)
                    : Cdn.GetDefaultUserAvatarUrl(user.Discriminator);

            internal static CultureInfo CreateLocale(string locale)
                => CultureInfo.ReadOnly(locale != null
                    ? new CultureInfo(locale)
                    : new CultureInfo("en-US"));

            internal static string GetSystemMessageContent(ISystemMessage message, IGuild guild) => message.Type switch
            {
                SystemMessageType.RecipientAdded => $"{message.Author.Name} added {message.MentionedUsers[0].Name} to the group.",
                SystemMessageType.RecipientRemoved => $"{message.Author.Name} removed {message.MentionedUsers[0].Name} from the group.",
                SystemMessageType.Call => throw new NotImplementedException(), // TODO
                SystemMessageType.ChannelNameChanged => $"{message.Author.Name} changed the channel name: {message.RawContent}",
                SystemMessageType.ChannelIconChanged => $"{message.Author.Name} changed the channel icon.",
                SystemMessageType.ChannelMessagePinned => $"{message.Author.Name} pinned a message to this channel.",
                SystemMessageType.MemberJoined => string.Format(MemberJoinFormats[message.Id.CreatedAt.ToUnixTimeMilliseconds() % MemberJoinFormats.Length], message.Author.Name),
                SystemMessageType.GuildBoosted => $"{message.Author.Name} just boosted the server!",
                SystemMessageType.GuildBoostedFirstTier => $"{message.Author.Name} just boosted the server! {guild?.Name ?? "The server"} has achieved **Level 1!**",
                SystemMessageType.GuildBoostedSecondTier => $"{message.Author.Name} just boosted the server! {guild?.Name ?? "The server"} has achieved **Level 2!**",
                SystemMessageType.GuildBoostedThirdTier => $"{message.Author.Name} just boosted the server! {guild?.Name ?? "The server"} has achieved **Level 3!**",
                SystemMessageType.ChannelFollowed => $"{message.Author.Name} has added {message.RawContent} to this channel.",
                _ => string.Empty,
            };

            internal static readonly string[] MemberJoinFormats =
            {
                "{0} just joined the server - glhf!",
                "{0} just joined. Everyone, look busy!",
                "{0} just joined. Can I get a heal?",
                "{0} joined your party.",
                "{0} joined. You must construct additional pylons.",
                "Ermagherd. {0} is here.",
                "Welcome, {0}. Stay awhile and listen.",
                "Welcome, {0}. We were expecting you ( ͡° ͜ʖ ͡°)",
                "Welcome, {0}. We hope you brought pizza.",
                "Welcome {0}. Leave your weapons by the door.",
                "A wild {0} appeared.",
                "Swoooosh. {0} just landed.",
                "Brace yourselves. {0} just joined the server.",
                "{0} just joined... or did they?",
                "{0} just arrived. Seems OP - please nerf.",
                "{0} just slid into the server.",
                "A {0} has spawned in the server.",
                "Big {0} showed up!",
                "Where’s {0}? In the server!",
                "{0} hopped into the server. Kangaroo!!",
                "{0} just showed up. Hold my beer.",
                "Challenger approaching - {0} has appeared!",
                "It's a bird! It's a plane! Nevermind, it's just {0}.",
                "It's {0}! Praise the sun! \\\\[T]/",
                "Never gonna give {0} up. Never gonna let {0} down.",
                "{0} has joined the battle bus.",
                "Cheers, love! {0}'s here!",
                "Hey! Listen! {0} has joined!",
                "We've been expecting you {0}",
                "It's dangerous to go alone, take {0}!",
                "{0} has joined the server! It's super effective!",
                "Cheers, love! {0} is here!",
                "{0} is here, as the prophecy foretold.",
                "{0} has arrived. Party's over.",
                "Ready player {0}",
                "{0} is here to kick butt and chew bubblegum. And {0} is all out of gum.",
                "Hello. Is it {0} you're looking for?",
                "{0} has joined. Stay a while and listen!",
                "Roses are red, violets are blue, {0} joined this server with you"
            };
        }
    }
}