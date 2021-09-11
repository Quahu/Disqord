using System;
using System.Collections;
using System.Collections.Generic;
using Disqord.Utilities;

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

        public bool CreateInvites => Discord.Permissions.HasFlag(RawValue, Permission.CreateInvites);

        public bool ManageChannels => Discord.Permissions.HasFlag(RawValue, Permission.ManageChannels);

        public bool AddReactions => Discord.Permissions.HasFlag(RawValue, Permission.AddReactions);

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

        public bool Connect => Discord.Permissions.HasFlag(RawValue, Permission.Connect);

        public bool Speak => Discord.Permissions.HasFlag(RawValue, Permission.Speak);

        public bool MuteMembers => Discord.Permissions.HasFlag(RawValue, Permission.MuteMembers);

        public bool DeafenMembers => Discord.Permissions.HasFlag(RawValue, Permission.DeafenMembers);

        public bool MoveMembers => Discord.Permissions.HasFlag(RawValue, Permission.MoveMembers);

        public bool UseVad => Discord.Permissions.HasFlag(RawValue, Permission.UseVad);

        public bool ManageRoles => Discord.Permissions.HasFlag(RawValue, Permission.ManageRoles);

        public bool ManageWebhooks => Discord.Permissions.HasFlag(RawValue, Permission.ManageWebhooks);

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

        public ChannelPermissions(Permission permission)
            : this((ulong) permission)
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
            => FlagUtilities.GetFlags(Permissions).GetEnumerator();

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
