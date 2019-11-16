using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord
{
    public readonly partial struct ChannelPermissions : IEquatable<ulong>, IEquatable<Permission>, IEquatable<ChannelPermissions>,
        IEnumerable<Permission>
    {
        public static ChannelPermissions All => ALL_PERMISSIONS_VALUE;

        public static ChannelPermissions Text => TEXT_PERMISSIONS_VALUE;

        public static ChannelPermissions Voice => VOICE_PERMISSIONS_VALUE;

        public static ChannelPermissions Category => CATEGORY_PERMISSIONS_VALUE;

        public static ChannelPermissions None => 0;

        public bool CreateInstantInvite => Discord.Permissions.HasFlag(RawValue, Permission.CreateInstantInvite);

        public bool ManageChannels => Discord.Permissions.HasFlag(RawValue, Permission.ManageChannels);

        public bool AddReactions => Discord.Permissions.HasFlag(RawValue, Permission.AddReactions);

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

        public bool Connect => Discord.Permissions.HasFlag(RawValue, Permission.Connect);

        public bool Speak => Discord.Permissions.HasFlag(RawValue, Permission.Speak);

        public bool MuteMembers => Discord.Permissions.HasFlag(RawValue, Permission.MuteMembers);

        public bool DeafenMembers => Discord.Permissions.HasFlag(RawValue, Permission.DeafenMembers);

        public bool MoveMembers => Discord.Permissions.HasFlag(RawValue, Permission.MoveMembers);

        public bool UseVad => Discord.Permissions.HasFlag(RawValue, Permission.UseVad);

        public bool ManageRoles => Discord.Permissions.HasFlag(RawValue, Permission.ManageRoles);

        public bool ManageWebhooks => Discord.Permissions.HasFlag(RawValue, Permission.ManageWebhooks);

        public Permission Permissions => (Permission) RawValue;

        public ulong RawValue { get; }

        public ChannelPermissions(Permission permission) : this((ulong) permission)
        { }

        public ChannelPermissions(ulong rawValue)
        {
            RawValue = rawValue;
        }

        public static ChannelPermissions Mask(ulong rawValue, IGuildChannel channel)
        {
            var mask = channel switch
            {
                ITextChannel _ => TEXT_PERMISSIONS_VALUE,
                IVoiceChannel _ => VOICE_PERMISSIONS_VALUE,
                ICategoryChannel _ => CATEGORY_PERMISSIONS_VALUE,
                _ => ALL_PERMISSIONS_VALUE,
            };
            return new ChannelPermissions(rawValue & mask);
        }

        public bool Has(Permission permission)
            => Discord.Permissions.HasFlag(RawValue, permission);

        public bool Equals(ulong other)
            => RawValue == other;

        public bool Equals(Permission other)
            => RawValue == (ulong) other;

        public bool Equals(ChannelPermissions other)
            => RawValue == other.RawValue;

        public override bool Equals(object obj)
        {
            if (obj is ChannelPermissions channelPermissions)
                return Equals(channelPermissions);

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

        public static implicit operator ChannelPermissions(ulong value)
            => new ChannelPermissions(value);

        public static implicit operator ulong(ChannelPermissions value)
            => value.RawValue;

        public static implicit operator ChannelPermissions(Permission value)
            => (ulong) value;

        public static implicit operator Permission(ChannelPermissions value)
            => value.Permissions;

        public static ChannelPermissions operator +(ChannelPermissions left, Permission right)
        {
            var rawValue = left.RawValue;
            Discord.Permissions.SetFlag(ref rawValue, right);
            return rawValue;
        }

        public static ChannelPermissions operator -(ChannelPermissions left, Permission right)
        {
            var rawValue = left.RawValue;
            Discord.Permissions.UnsetFlag(ref rawValue, right);
            return rawValue;
        }

        public static bool operator ==(ChannelPermissions left, Permission right)
            => left.Equals(right);

        public static bool operator !=(ChannelPermissions left, Permission right)
            => !left.Equals(right);
    }
}
