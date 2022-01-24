using System;
using System.Collections;
using System.Collections.Generic;
using Disqord.Utilities;
using Qommon;

namespace Disqord
{
    public readonly partial struct GuildPermissions : IEquatable<ulong>, IEquatable<Permission>, IEquatable<GuildPermissions>,
        IEnumerable<Permission>
    {
        public static GuildPermissions All => AllPermissionsValue;

        public static GuildPermissions None => 0;

        public bool CreateInvites => Has(Permission.CreateInvites);

        public bool KickMembers => Has(Permission.KickMembers);

        public bool BanMembers => Has(Permission.BanMembers);

        public bool Administrator => Has(Permission.Administrator);

        public bool ManageChannels => Has(Permission.ManageChannels);

        public bool ManageGuild => Has(Permission.ManageGuild);

        public bool AddReactions => Has(Permission.AddReactions);

        public bool ViewAuditLog => Has(Permission.ViewAuditLog);

        public bool PrioritySpeaker => Has(Permission.PrioritySpeaker);

        public bool Stream => Has(Permission.Stream);

        public bool ViewChannels => Has(Permission.ViewChannels);

        public bool SendMessages => Has(Permission.SendMessages);

        public bool UseTextToSpeech => Has(Permission.UseTextToSpeech);

        public bool ManageMessages => Has(Permission.ManageMessages);

        public bool SendEmbeds => Has(Permission.SendEmbeds);

        public bool SendAttachments => Has(Permission.SendAttachments);

        public bool ReadMessageHistory => Has(Permission.ReadMessageHistory);

        public bool MentionEveryone => Has(Permission.MentionEveryone);

        public bool UseExternalEmojis => Has(Permission.UseExternalEmojis);

        public bool ViewGuildInsights => Has(Permission.ViewGuildInsights);

        public bool Connect => Has(Permission.Connect);

        public bool Speak => Has(Permission.Speak);

        public bool MuteMembers => Has(Permission.MuteMembers);

        public bool DeafenMembers => Has(Permission.DeafenMembers);

        public bool MoveMembers => Has(Permission.MoveMembers);

        public bool UseVoiceActivity => Has(Permission.UseVoiceActivity);

        public bool SetNick => Has(Permission.SetNick);

        public bool ManageNicks => Has(Permission.ManageNicks);

        public bool ManageRoles => Has(Permission.ManageRoles);

        public bool ManageWebhooks => Has(Permission.ManageWebhooks);

        public bool ManageEmojisAndStickers => Has(Permission.ManageEmojisAndStickers);

        public bool UseApplicationCommands => Has(Permission.UseApplicationCommands);

        public bool RequestToSpeak => Has(Permission.RequestToSpeak);

        public bool ManageEvents => Has(Permission.ManageEvents);

        public bool ManageThreads => Has(Permission.ManageThreads);

        public bool CreatePublicThreads => Has(Permission.CreatePublicThreads);

        public bool CreatePrivateThreads => Has(Permission.CreatePrivateThreads);

        public bool UseExternalStickers => Has(Permission.UseExternalStickers);

        public bool SendMessagesInThreads => Has(Permission.SendMessagesInThreads);

        public bool StartActivities => Has(Permission.StartActivities);

        public bool ModerateMembers => Has(Permission.ModerateMembers);

        public Permission Flags => (Permission) RawValue;

        public ulong RawValue { get; }

        public GuildPermissions(Permission permissions)
            : this((ulong) permissions)
        { }

        public GuildPermissions(ulong rawValue)
        {
            RawValue = rawValue;
        }

        public bool Has(Permission permissions)
            => Flags.HasFlag(permissions);

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
        {
            if (Flags.HasFlag(Permission.Administrator))
                return nameof(Permission.Administrator);

            return Flags.ToString();
        }

        public IEnumerator<Permission> GetEnumerator()
            => FlagUtilities.GetFlags(Flags).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public static implicit operator GuildPermissions(ulong value)
            => new(value);

        public static implicit operator GuildPermissions(Permission value)
            => new(value);

        public static implicit operator Permission(GuildPermissions value)
            => value.Flags;

        public static bool operator ==(GuildPermissions left, GuildPermissions right)
            => left.RawValue == right.RawValue;

        public static bool operator !=(GuildPermissions left, GuildPermissions right)
            => left.RawValue != right.RawValue;

        public static GuildPermissions operator ~(GuildPermissions value)
            => ~value.RawValue;

        public static GuildPermissions operator &(GuildPermissions left, GuildPermissions right)
            => left.RawValue & right.RawValue;

        public static GuildPermissions operator ^(GuildPermissions left, GuildPermissions right)
            => left.RawValue ^ right.RawValue;

        public static GuildPermissions operator |(GuildPermissions left, GuildPermissions right)
            => left.RawValue | right.RawValue;

        [Obsolete("The '+' and '-' operators have been removed, use '|' and '& ~' respectively.", true)]
        public static GuildPermissions operator +(GuildPermissions left, GuildPermissions right)
            => Throw.InvalidOperationException<GuildPermissions>();

        [Obsolete("The '+' and '-' operators have been removed, use '|' and '& ~' respectively.", true)]
        public static GuildPermissions operator -(GuildPermissions left, GuildPermissions right)
            => Throw.InvalidOperationException<GuildPermissions>();
    }
}
