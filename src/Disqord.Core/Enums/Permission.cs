using System;

namespace Disqord
{
    [Flags]
    public enum Permission : ulong
    {
        None = 0,

        CreateInstantInvite = 1ul << 0,

        KickMembers = 1ul << 1,

        BanMembers = 1ul << 2,

        Administrator = 1ul << 3,

        ManageChannels = 1ul << 4,

        ManageGuild = 1ul << 5,

        AddReactions = 1ul << 6,

        ViewAuditLog = 1ul << 7,

        PrioritySpeaker = 1ul << 8,

        Stream = 1ul << 9,

        ViewChannel = 1ul << 10,

        SendMessages = 1ul << 11,

        UseTextToSpeech = 1ul << 12,

        ManageMessages = 1ul << 13,

        EmbedLinks = 1ul << 14,

        AttachFiles = 1ul << 15,

        ReadMessageHistory = 1ul << 16,

        MentionEveryone = 1ul << 17,

        UseExternalEmojis = 1ul << 18,

        ViewGuildInsights = 1ul << 19,

        Connect = 1ul << 20,

        Speak = 1ul << 21,

        MuteMembers = 1ul << 22,

        DeafenMembers = 1ul << 23,

        MoveMembers = 1ul << 24,

        UseVad = 1ul << 25,

        ChangeNickname = 1ul << 26,

        ManageNicknames = 1ul << 27,

        ManageRoles = 1ul << 28,

        ManageWebhooks = 1ul << 29,

        ManageEmojisAndStickers = 1ul << 30,

        UseSlashCommands = 1ul << 31,

        RequestToSpeak = 1ul << 32,

        ManageThreads = 1ul << 34,

        UsePublicThreads = 1ul << 35,

        UsePrivateThreads = 1ul << 36,

        UseExternalStickers = 1ul << 37
    }
}