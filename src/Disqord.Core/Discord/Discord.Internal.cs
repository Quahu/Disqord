using System;
using System.Globalization;

namespace Disqord
{
    public static partial class Discord
    {
        internal static class Internal
        {
            internal static CultureInfo GetLocale(string locale)
                => CultureInfo.GetCultureInfo(locale ?? "en-US");

            internal static string GetSystemMessageContent(ISystemMessage message, IGuild guild) => message.Type switch
            {
                SystemMessageType.RecipientAdded => $"{message.Author.Name} added {message.MentionedUsers[0].Name} to the group.",
                SystemMessageType.RecipientRemoved => $"{message.Author.Name} removed {message.MentionedUsers[0].Name} from the group.",
                SystemMessageType.Call => throw new NotSupportedException(),
                SystemMessageType.ChannelNameChanged => $"{message.Author.Name} changed the channel name: {message.RawContent}",
                SystemMessageType.ChannelIconChanged => $"{message.Author.Name} changed the channel icon.",
                SystemMessageType.ChannelMessagePinned => $"{message.Author.Name} pinned a message to this channel.",
                SystemMessageType.MemberJoined => string.Format(MemberJoinFormats[message.Id.CreatedAt.ToUnixTimeMilliseconds() % MemberJoinFormats.Length], message.Author.Name),
                SystemMessageType.GuildBoosted => $"{message.Author.Name} just boosted the server!",
                SystemMessageType.GuildBoostedFirstTier => $"{message.Author.Name} just boosted the server! {guild?.Name ?? "The server"} has achieved **Level 1!**",
                SystemMessageType.GuildBoostedSecondTier => $"{message.Author.Name} just boosted the server! {guild?.Name ?? "The server"} has achieved **Level 2!**",
                SystemMessageType.GuildBoostedThirdTier => $"{message.Author.Name} just boosted the server! {guild?.Name ?? "The server"} has achieved **Level 3!**",
                SystemMessageType.ChannelFollowed => $"{message.Author.Name} has added {message.RawContent} to this channel.",
                SystemMessageType.GuildStream => $"{message.Author.Name} is live!",
                SystemMessageType.GuildDiscoveryDisqualified => "This server has been removed from Server Discovery because it no longer passes all the requirements. Check Server Settings for more details.",
                SystemMessageType.GuildDiscoveryRequalified => "This server is eligible for Server Discovery again and has been automatically relisted!",
                SystemMessageType.GuildDiscoveryGracePeriodInitialWarning => "This server has failed Discovery activity requirements for 1 week. If this server fails for 4 weeks in a row, it will be automatically removed from Discovery.",
                SystemMessageType.GuildDiscoveryGracePeriodFinalWarning => "This server has failed Discovery activity requirements for 3 weeks in a row. If this server fails for 1 more week, it will be removed from Discovery.",
                SystemMessageType.ThreadCreated => $"{message.Author.Name} started a thread.",
                _ => string.Empty
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
