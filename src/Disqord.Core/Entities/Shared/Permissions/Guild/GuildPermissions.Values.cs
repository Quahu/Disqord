﻿namespace Disqord
{
    public readonly partial struct GuildPermissions
    {
        public const ulong ALL_PERMISSIONS_VALUE = (ulong) (
            Permission.CreateInvites
            | Permission.KickMembers
            | Permission.BanMembers
            | Permission.Administrator
            | Permission.ManageChannels
            | Permission.ManageGuild
            | Permission.AddReactions
            | Permission.ViewAuditLog
            | Permission.PrioritySpeaker
            | Permission.Stream
            | Permission.ViewChannels
            | Permission.SendMessages
            | Permission.UseTextToSpeech
            | Permission.ManageMessages
            | Permission.SendEmbeds
            | Permission.SendAttachments
            | Permission.ReadMessageHistory
            | Permission.MentionEveryone
            | Permission.UseExternalEmojis
            | Permission.ViewGuildInsights
            | Permission.Connect
            | Permission.Speak
            | Permission.MuteMembers
            | Permission.DeafenMembers
            | Permission.MoveMembers
            | Permission.UseVad
            | Permission.SetNick
            | Permission.ManageNicks
            | Permission.ManageRoles
            | Permission.ManageWebhooks
            | Permission.ManageEmojisAndStickers
            | Permission.UseApplicationCommands
            | Permission.RequestToSpeak
            | Permission.ManageThreads
            | Permission.CreatePublicThreads
            | Permission.CreatePrivateThreads
            | Permission.UseExternalStickers
            | Permission.SendMessagesInThreads
            | Permission.StartActivities);
    }
}
