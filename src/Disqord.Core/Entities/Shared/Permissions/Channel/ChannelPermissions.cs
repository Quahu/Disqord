using System;
using System.Collections;
using System.Collections.Generic;
using Disqord.Utilities;
using Qommon;

namespace Disqord
{
    public readonly partial struct ChannelPermissions : IEquatable<ulong>, IEquatable<Permission>, IEquatable<ChannelPermissions>,
        IEnumerable<Permission>
    {
        public static ChannelPermissions All => AllPermissionsValue;

        public static ChannelPermissions Text => TextPermissionsValue;

        public static ChannelPermissions Voice => VoicePermissionsValue;

        public static ChannelPermissions Stage => StagePermissionsValue;

        public static ChannelPermissions Category => CategoryPermissionsValue;

        public static ChannelPermissions None => 0;

        public bool CreateInvites => Has(Permission.CreateInvites);

        public bool ManageChannels => Has(Permission.ManageChannels);

        public bool AddReactions => Has(Permission.AddReactions);

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

        public bool Connect => Has(Permission.Connect);

        public bool Speak => Has(Permission.Speak);

        public bool MuteMembers => Has(Permission.MuteMembers);

        public bool DeafenMembers => Has(Permission.DeafenMembers);

        public bool MoveMembers => Has(Permission.MoveMembers);

        public bool UseVoiceActivity => Has(Permission.UseVoiceActivity);

        public bool ManageRoles => Has(Permission.ManageRoles);

        public bool ManageWebhooks => Has(Permission.ManageWebhooks);

        public bool UseApplicationCommands => Has(Permission.UseApplicationCommands);

        public bool RequestToSpeak => Has(Permission.RequestToSpeak);

        public bool ManageThreads => Has(Permission.ManageThreads);

        public bool CreatePublicThreads => Has(Permission.CreatePublicThreads);

        public bool CreatePrivateThreads => Has(Permission.CreatePrivateThreads);

        public bool UseExternalStickers => Has(Permission.UseExternalStickers);

        public bool SendMessagesInThreads => Has(Permission.SendMessagesInThreads);

        public bool StartActivities => Has(Permission.StartActivities);

        public Permission Flags => (Permission) RawValue;

        public ulong RawValue { get; }

        public ChannelPermissions(Permission permissions)
            : this((ulong) permissions)
        { }

        public ChannelPermissions(ulong rawValue)
        {
            RawValue = rawValue;
        }

        public static ChannelPermissions Mask(Permission permissions, IGuildChannel channel)
        {
            var mask = channel switch
            {
                IMessageGuildChannel _ => TextPermissionsValue,
                IVoiceChannel _ => VoicePermissionsValue,
                IStageChannel _ => StagePermissionsValue,
                ICategoryChannel _ => CategoryPermissionsValue,
                _ => AllPermissionsValue,
            };

            return new ChannelPermissions(permissions & (Permission) mask);
        }

        public static ChannelPermissions Mask(Permission permissions, out Permission remainingPermissions)
        {
            var allPermission = (Permission) AllPermissionsValue;
            remainingPermissions = permissions & ~allPermission;
            return new ChannelPermissions(permissions & allPermission);
        }

        public bool Has(Permission permissions)
            => Flags.HasFlag(permissions);

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
            => Flags.ToString();

        public IEnumerator<Permission> GetEnumerator()
            => FlagUtilities.GetFlags(Flags).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public static implicit operator ChannelPermissions(ulong value)
            => new(value);

        public static implicit operator ChannelPermissions(Permission value)
            => new(value);

        public static implicit operator Permission(ChannelPermissions value)
            => value.Flags;

        public static bool operator ==(ChannelPermissions left, ChannelPermissions right)
            => left.RawValue == right.RawValue;

        public static bool operator !=(ChannelPermissions left, ChannelPermissions right)
            => left.RawValue != right.RawValue;

        public static ChannelPermissions operator ~(ChannelPermissions value)
            => ~value.RawValue;

        public static ChannelPermissions operator &(ChannelPermissions left, ChannelPermissions right)
            => left.RawValue & right.RawValue;

        public static ChannelPermissions operator ^(ChannelPermissions left, ChannelPermissions right)
            => left.RawValue ^ right.RawValue;

        public static ChannelPermissions operator |(ChannelPermissions left, ChannelPermissions right)
            => left.RawValue | right.RawValue;

        [Obsolete("The '+' and '-' operators have been removed, use '|' and '& ~' respectively.", true)]
        public static ChannelPermissions operator +(ChannelPermissions left, ChannelPermissions right)
            => Throw.InvalidOperationException<ChannelPermissions>();

        [Obsolete("The '+' and '-' operators have been removed, use '|' and '& ~' respectively.", true)]
        public static ChannelPermissions operator -(ChannelPermissions left, ChannelPermissions right)
            => Throw.InvalidOperationException<ChannelPermissions>();
    }
}
