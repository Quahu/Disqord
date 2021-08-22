using System;

namespace Disqord
{
    [Flags]
    public enum Permission : ulong
    {
        /// <summary>
        ///     Represents no permission.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Allows creating invites.
        /// </summary>
        CreateInvites = 1ul << 0,

        /// <summary>
        ///     Allows kicking members.
        /// </summary>
        KickMembers = 1ul << 1,

        /// <summary>
        ///     Allows banning members.
        /// </summary>
        BanMembers = 1ul << 2,

        /// <summary>
        ///     Allows everything, bypasses overwrites.
        /// </summary>
        Administrator = 1ul << 3,

        /// <summary>
        ///     Allows management of channels.
        /// </summary>
        ManageChannels = 1ul << 4,

        /// <summary>
        ///     Allows management of the guild.
        /// </summary>
        ManageGuild = 1ul << 5,

        /// <summary>
        ///     Allows adding new reactions to messages.
        /// </summary>
        AddReactions = 1ul << 6,

        /// <summary>
        ///     Allows viewing the audit log.
        /// </summary>
        ViewAuditLog = 1ul << 7,

        /// <summary>
        ///     Allows using the priority speaker functionality.
        /// </summary>
        PrioritySpeaker = 1ul << 8,

        /// <summary>
        ///     Allows streaming.
        /// </summary>
        Stream = 1ul << 9,

        /// <summary>
        ///     Allows viewing guild channels or a specific channel.
        /// </summary>
        ViewChannels = 1ul << 10,

        /// <summary>
        ///     Allows sending messages.
        /// </summary>
        /// <remarks>
        ///     This permission does not allow sending messages in threads.
        /// </remarks>
        SendMessages = 1ul << 11,

        /// <summary>
        ///     Allows using text to speech.
        /// </summary>
        UseTextToSpeech = 1ul << 12,

        /// <summary>
        ///     Allows management of messages.
        /// </summary>
        ManageMessages = 1ul << 13,

        /// <summary>
        ///     Allows embedding links and sending embeds.
        /// </summary>
        SendEmbeds = 1ul << 14,

        /// <summary>
        ///     Allows sending attachments.
        /// </summary>
        SendAttachments = 1ul << 15,

        /// <summary>
        ///     Allows reading message history, i.e. looking up already sent messages.
        /// </summary>
        ReadMessageHistory = 1ul << 16,

        /// <summary>
        ///     Allows mentioning <see cref="Mention.Here"/> and <see cref="Mention.Everyone"/>.
        /// </summary>
        MentionEveryone = 1ul << 17,

        /// <summary>
        ///     Allows using emojis from other guilds.
        /// </summary>
        UseExternalEmojis = 1ul << 18,

        /// <summary>
        ///     Allows viewing guild insights.
        /// </summary>
        ViewGuildInsights = 1ul << 19,

        /// <summary>
        ///     Allows connecting to vocal channels.
        /// </summary>
        Connect = 1ul << 20,

        /// <summary>
        ///     Allows speaking in vocal channels.
        /// </summary>
        Speak = 1ul << 21,

        /// <summary>
        ///     Allows muting members in vocal channels.
        /// </summary>
        MuteMembers = 1ul << 22,

        /// <summary>
        ///     Allows deafening members in vocal channels.
        /// </summary>
        DeafenMembers = 1ul << 23,

        /// <summary>
        ///     Allows moving members between vocal channels.
        /// </summary>
        MoveMembers = 1ul << 24,

        /// <summary>
        ///     Allows using voice activity detection over just push-to-talk.
        /// </summary>
        UseVoiceActivity = 1ul << 25,

        /// <summary>
        ///     Allows setting the own nick.
        /// </summary>
        SetNick = 1ul << 26,

        /// <summary>
        ///     Allows management of others' nicks.
        /// </summary>
        ManageNicks = 1ul << 27,

        /// <summary>
        ///     Allows management of roles.
        /// </summary>
        ManageRoles = 1ul << 28,

        /// <summary>
        ///     Allows management of webhooks.
        /// </summary>
        ManageWebhooks = 1ul << 29,

        /// <summary>
        ///     Allows management of emojis and stickers.
        /// </summary>
        ManageEmojisAndStickers = 1ul << 30,

        /// <summary>
        ///     Allows using application commands, including slash commands and context menu commands.
        /// </summary>
        UseApplicationCommands = 1ul << 31,

        /// <summary>
        ///     Allows requesting to speak in stage channels.
        /// </summary>
        RequestToSpeak = 1ul << 32,

        /// <summary>
        ///     Allows management of guild events.
        /// </summary>
        ManageEvents = 1ul << 33,

        /// <summary>
        ///     Allows management of threads.
        /// </summary>
        ManageThreads = 1ul << 34,

        /// <summary>
        ///     Allows creating public threads.
        /// </summary>
        CreatePublicThreads = 1ul << 35,

        /// <summary>
        ///     Allows creating private threads.
        /// </summary>
        CreatePrivateThreads = 1ul << 36,

        /// <summary>
        ///     Allows using stickers from other guilds.
        /// </summary>
        UseExternalStickers = 1ul << 37,

        /// <summary>
        ///     Allows sending messages in threads.
        /// </summary>
        SendMessagesInThreads = 1ul << 38,

        /// <summary>
        ///     Allows starting activities in voice channels.
        /// </summary>
        StartActivities = 1ul << 39,

        /// <summary>
        ///     Allows performing moderation actions on members.
        /// </summary>
        /// <remarks>
        ///     Note that this permission allows you to, for example, time out a member,
        ///     but does not allow actions that have their own permission like, for example, <see cref="Permission.KickMembers"/>.
        /// </remarks>
        ModerateMembers = 1ul << 40
    }
}
