using System;
using System.Collections;
using System.Collections.Generic;
using Disqord.Utilities;

namespace Disqord
{
    public readonly partial struct GuildPermissions : IEquatable<ulong>, IEquatable<Permission>, IEquatable<GuildPermissions>,
        IEnumerable<Permission>
    {
        public static GuildPermissions All => ALL_PERMISSIONS_VALUE;

        public static GuildPermissions None => 0;

        public bool CreateInvites => Discord.Permissions.HasFlag(RawValue, Permission.CreateInvites);

        public bool KickMembers => Discord.Permissions.HasFlag(RawValue, Permission.KickMembers);

        public bool BanMembers => Discord.Permissions.HasFlag(RawValue, Permission.BanMembers);

        public bool Administrator => Discord.Permissions.HasFlag(RawValue, Permission.Administrator);

        public bool ManageChannels => Discord.Permissions.HasFlag(RawValue, Permission.ManageChannels);

        public bool ManageGuild => Discord.Permissions.HasFlag(RawValue, Permission.ManageGuild);

        public bool AddReactions => Discord.Permissions.HasFlag(RawValue, Permission.AddReactions);

        public bool ViewAuditLog => Discord.Permissions.HasFlag(RawValue, Permission.ViewAuditLog);

        public bool PrioritySpeaker => Discord.Permissions.HasFlag(RawValue, Permission.PrioritySpeaker);

        public bool Stream => Discord.Permissions.HasFlag(RawValue, Permission.Stream);

        public bool ViewChannels => Discord.Permissions.HasFlag(RawValue, Permission.ViewChannels);

        public bool SendMessages => Discord.Permissions.HasFlag(RawValue, Permission.SendMessages);

        public bool UseTextToSpeech => Discord.Permissions.HasFlag(RawValue, Permission.UseTextToSpeech);

        public bool ManageMessages => Discord.Permissions.HasFlag(RawValue, Permission.ManageMessages);

        public bool SendEmbeds => Discord.Permissions.HasFlag(RawValue, Permission.SendEmbeds);

        public bool SendAttachments => Discord.Permissions.HasFlag(RawValue, Permission.SendAttachments);

        public bool ReadMessageHistory => Discord.Permissions.HasFlag(RawValue, Permission.ReadMessageHistory);

        public bool MentionEveryone => Discord.Permissions.HasFlag(RawValue, Permission.MentionEveryone);

        public bool UseExternalEmojis => Discord.Permissions.HasFlag(RawValue, Permission.UseExternalEmojis);

        public bool ViewGuildInsights => Discord.Permissions.HasFlag(RawValue, Permission.ViewGuildInsights);

        public bool Connect => Discord.Permissions.HasFlag(RawValue, Permission.Connect);

        public bool Speak => Discord.Permissions.HasFlag(RawValue, Permission.Speak);

        public bool MuteMembers => Discord.Permissions.HasFlag(RawValue, Permission.MuteMembers);

        public bool DeafenMembers => Discord.Permissions.HasFlag(RawValue, Permission.DeafenMembers);

        public bool MoveMembers => Discord.Permissions.HasFlag(RawValue, Permission.MoveMembers);

        public bool UseVad => Discord.Permissions.HasFlag(RawValue, Permission.UseVad);

        public bool SetNick => Discord.Permissions.HasFlag(RawValue, Permission.SetNick);

        public bool ManageNicks => Discord.Permissions.HasFlag(RawValue, Permission.ManageNicks);

        public bool ManageRoles => Discord.Permissions.HasFlag(RawValue, Permission.ManageRoles);

        public bool ManageWebhooks => Discord.Permissions.HasFlag(RawValue, Permission.ManageWebhooks);

        public bool ManageEmojisAndStickers => Discord.Permissions.HasFlag(RawValue, Permission.ManageEmojisAndStickers);

        public bool UseApplicationCommands => Discord.Permissions.HasFlag(RawValue, Permission.UseApplicationCommands);

        public bool RequestToSpeak => Discord.Permissions.HasFlag(RawValue, Permission.RequestToSpeak);

        public bool ManageThreads => Discord.Permissions.HasFlag(RawValue, Permission.ManageThreads);

        public bool CreatePublicThreads => Discord.Permissions.HasFlag(RawValue, Permission.CreatePublicThreads);

        public bool CreatePrivateThreads => Discord.Permissions.HasFlag(RawValue, Permission.CreatePrivateThreads);

        public bool UseExternalStickers => Discord.Permissions.HasFlag(RawValue, Permission.UseExternalStickers);

        public bool SendMessagesInThreads => Discord.Permissions.HasFlag(RawValue, Permission.SendMessagesInThreads);

        public bool StartActivities => Discord.Permissions.HasFlag(RawValue, Permission.StartActivities);

        public Permission Permissions => (Permission) RawValue;

        public ulong RawValue { get; }

        public GuildPermissions(Permission permission)
            : this((ulong) permission)
        { }

        public GuildPermissions(ulong rawValue)
        {
            RawValue = rawValue;
        }

        public bool Has(Permission permission)
            => Discord.Permissions.HasFlag(RawValue, permission);

        public bool Equals(ulong other)
            => RawValue == other;

        public bool Equals(Permission other)
            => RawValue == (ulong) other;

        public bool Equals(GuildPermissions other)
            => RawValue == other.RawValue;

        public override bool Equals(object obj)
        {
            if (obj is GuildPermissions guildPermissions)
                return Equals(guildPermissions);

            if (obj is Permission permission)
                return Equals(permission);

            if (obj is ulong rawValue)
                return Equals(rawValue);

            return false;
        }

        public override int GetHashCode()
            => RawValue.GetHashCode();

        public override string ToString()
            => Permissions.ToString();

        public IEnumerator<Permission> GetEnumerator()
            => FlagUtilities.GetFlags(Permissions).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public static implicit operator GuildPermissions(ulong value)
            => new GuildPermissions(value);

        public static implicit operator ulong(GuildPermissions value)
            => value.RawValue;

        public static implicit operator GuildPermissions(Permission value)
            => (ulong) value;

        public static implicit operator Permission(GuildPermissions value)
            => value.Permissions;

        public static GuildPermissions operator +(GuildPermissions left, Permission right)
        {
            var rawValue = left.RawValue;
            Discord.Permissions.SetFlag(ref rawValue, right);
            return rawValue;
        }

        public static GuildPermissions operator -(GuildPermissions left, Permission right)
        {
            var rawValue = left.RawValue;
            Discord.Permissions.UnsetFlag(ref rawValue, right);
            return rawValue;
        }

        public static bool operator ==(GuildPermissions left, Permission right)
            => left.Equals(right);

        public static bool operator !=(GuildPermissions left, Permission right)
            => !left.Equals(right);
    }
}
