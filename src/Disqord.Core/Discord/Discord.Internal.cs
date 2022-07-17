using System.Globalization;
using Qommon;

namespace Disqord;

public static partial class Discord
{
    internal static class Internal
    {
        internal static CultureInfo GetLocale(string? locale)
            => CultureInfo.GetCultureInfo(locale ?? "en-US");

        internal static string GetSystemMessageContent(ISystemMessage message, IGuild? guild)
        {
            return message.Type switch
            {
                SystemMessageType.RecipientAdded => $"{message.Author.Name} added {message.MentionedUsers[0].Name} to the group.",
                SystemMessageType.RecipientRemoved => $"{message.Author.Name} removed {message.MentionedUsers[0].Name} from the group.",
                SystemMessageType.Call => Throw.NotSupportedException<string>(),
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
                SystemMessageType.GuildInviteReminder => "**Wondering who to invite?**\nStart by inviting anyone who can help you build the server!",
                _ => string.Empty
            };
        }

        private static readonly string[] MemberJoinFormats =
        {
            "{0} joined the party.",
            "{0} is here.",
            "Welcome, {0}. We hope you brought pizza.",
            "A wild {0} appeared.",
            "{0} just landed.",
            "{0} just slid into the server.",
            "{0} just showed up!",
            "Welcome {0}. Say hi!",
            "{0} hopped into the server.",
            "Everyone welcome {0}!",
            "Glad you're here, {0}.",
            "Good to see you, {0}.",
            "Yay you made it, {0}!"
        };
    }
}
