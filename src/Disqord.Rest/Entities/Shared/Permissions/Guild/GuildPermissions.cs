using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord
{
    public readonly partial struct GuildPermissions : IEquatable<ulong>, IEquatable<Permission>, IEquatable<GuildPermissions>,
        IEnumerable<Permission>
    {
        public static GuildPermissions All => ALL_PERMISSIONS_VALUE;

        public static GuildPermissions None => 0;

        public bool CreateInstantInvite => Discord.Permissions.HasFlag(RawValue, Permission.CreateInstantInvite);

        public bool KickMembers => Discord.Permissions.HasFlag(RawValue, Permission.KickMembers);

        public bool BanMembers => Discord.Permissions.HasFlag(RawValue, Permission.BanMembers);

        public bool Administrator => Discord.Permissions.HasFlag(RawValue, Permission.Administrator);

        public bool ManageChannels => Discord.Permissions.HasFlag(RawValue, Permission.ManageChannels);

        public bool ManageGuild => Discord.Permissions.HasFlag(RawValue, Permission.ManageGuild);

        public bool AddReactions => Discord.Permissions.HasFlag(RawValue, Permission.AddReactions);

        public bool ViewAuditLog => Discord.Permissions.HasFlag(RawValue, Permission.ViewAuditLog);

        public bool PrioritySpeaker => Discord.Permissions.HasFlag(RawValue, Permission.PrioritySpeaker);

        public bool Stream => Discord.Permissions.HasFlag(RawValue, Permission.Stream);

        public bool ViewChannel => Discord.Permissions.HasFlag(RawValue, Permission.ViewChannel);

        public bool SendMessages => Discord.Permissions.HasFlag(RawValue, Permission.SendMessages);

        public bool SendTtsMessages => Discord.Permissions.HasFlag(RawValue, Permission.SendTtsMessages);

        public bool ManageMessages => Discord.Permissions.HasFlag(RawValue, Permission.ManageMessages);

        public bool EmbedLinks => Discord.Permissions.HasFlag(RawValue, Permission.EmbedLinks);

        public bool AttachFiles => Discord.Permissions.HasFlag(RawValue, Permission.AttachFiles);

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

        public bool ChangeNickname => Discord.Permissions.HasFlag(RawValue, Permission.ChangeNickname);

        public bool ManageNicknames => Discord.Permissions.HasFlag(RawValue, Permission.ManageNicknames);

        public bool ManageRoles => Discord.Permissions.HasFlag(RawValue, Permission.ManageRoles);

        public bool ManageWebhooks => Discord.Permissions.HasFlag(RawValue, Permission.ManageWebhooks);

        public bool ManageEmojis => Discord.Permissions.HasFlag(RawValue, Permission.ManageEmojis);

        public Permission Permissions => (Permission) RawValue;

        public ulong RawValue { get; }

        public GuildPermissions(Permission permission) : this((ulong) permission)
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
            => Discord.Permissions.GetFlags(Permissions).GetEnumerator();

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
