using System;
using System.Collections;
using System.Collections.Generic;
using Disqord.Utilities;
using Qommon;

namespace Disqord;

[Obsolete("The ChannelPermissions struct was removed. For checking permissions use HasFlag on the Permissions enum. Combining permissions should stay the same.", true)]
public readonly partial struct ChannelPermissions : IEquatable<ulong>, IEquatable<Permissions>, IEquatable<ChannelPermissions>,
    IEnumerable<Permissions>
{
    public static ChannelPermissions All => AllPermissionsValue;

    public static ChannelPermissions Text => TextPermissionsValue;

    public static ChannelPermissions Voice => VoicePermissionsValue;

    public static ChannelPermissions Stage => StagePermissionsValue;

    public static ChannelPermissions Category => CategoryPermissionsValue;

    public static ChannelPermissions None => 0;

    public bool CreateInvites => Has(Permissions.CreateInvites);

    public bool ManageChannels => Has(Permissions.ManageChannels);

    public bool AddReactions => Has(Permissions.AddReactions);

    public bool PrioritySpeaker => Has(Permissions.UsePrioritySpeaker);

    public bool Stream => Has(Permissions.Stream);

    public bool ViewChannels => Has(Permissions.ViewChannels);

    public bool SendMessages => Has(Permissions.SendMessages);

    public bool UseTextToSpeech => Has(Permissions.UseTextToSpeech);

    public bool ManageMessages => Has(Permissions.ManageMessages);

    public bool SendEmbeds => Has(Permissions.SendEmbeds);

    public bool SendAttachments => Has(Permissions.SendAttachments);

    public bool ReadMessageHistory => Has(Permissions.ReadMessageHistory);

    public bool MentionEveryone => Has(Permissions.MentionEveryone);

    public bool UseExternalEmojis => Has(Permissions.UseExternalEmojis);

    public bool Connect => Has(Permissions.Connect);

    public bool Speak => Has(Permissions.Speak);

    public bool MuteMembers => Has(Permissions.MuteMembers);

    public bool DeafenMembers => Has(Permissions.DeafenMembers);

    public bool MoveMembers => Has(Permissions.MoveMembers);

    public bool UseVoiceActivity => Has(Permissions.UseVoiceActivity);

    public bool ManageRoles => Has(Permissions.ManageRoles);

    public bool ManageWebhooks => Has(Permissions.ManageWebhooks);

    public bool UseApplicationCommands => Has(Permissions.UseApplicationCommands);

    public bool RequestToSpeak => Has(Permissions.RequestToSpeak);

    public bool ManageThreads => Has(Permissions.ManageThreads);

    public bool CreatePublicThreads => Has(Permissions.CreatePublicThreads);

    public bool CreatePrivateThreads => Has(Permissions.CreatePrivateThreads);

    public bool UseExternalStickers => Has(Permissions.UseExternalStickers);

    public bool SendMessagesInThreads => Has(Permissions.SendMessagesInThreads);

    public bool StartActivities => Has(Permissions.StartActivities);

    public Permissions Flags => (Permissions) RawValue;

    public ulong RawValue { get; }

    public ChannelPermissions(Permissions permissions)
        : this((ulong) permissions)
    { }

    public ChannelPermissions(ulong rawValue)
    {
        RawValue = rawValue;
    }

    public static ChannelPermissions Mask(Permissions permissions, IGuildChannel channel)
    {
        var mask = channel switch
        {
            IVoiceChannel _ => VoicePermissionsValue, // Handle voice channel first since they are IMessageGuildChannels as well
            IMessageGuildChannel _ => TextPermissionsValue,
            IStageChannel _ => StagePermissionsValue,
            ICategoryChannel _ => CategoryPermissionsValue,
            _ => AllPermissionsValue,
        };

        return new ChannelPermissions(permissions & (Permissions) mask);
    }

    public static ChannelPermissions Mask(Permissions permissions, out Permissions remainingPermissions)
    {
        const Permissions allPermission = (Permissions) AllPermissionsValue;
        remainingPermissions = permissions & ~allPermission;
        return new ChannelPermissions(permissions & allPermission);
    }

    public bool Has(Permissions permissions)
        => Flags.HasFlag(permissions);

    public bool Equals(ulong other)
        => RawValue == other;

    public bool Equals(Permissions other)
        => RawValue == (ulong) other;

    public bool Equals(ChannelPermissions other)
        => RawValue == other.RawValue;

    public override bool Equals(object? obj)
    {
        if (obj is ChannelPermissions channelPermissions)
            return Equals(channelPermissions);

        if (obj is Permissions permission)
            return Equals(permission);

        if (obj is ulong rawValue)
            return Equals(rawValue);

        return false;
    }

    public override int GetHashCode()
        => RawValue.GetHashCode();

    public override string ToString()
        => Flags.ToString();

    public IEnumerator<Permissions> GetEnumerator()
        => FlagUtilities.GetFlags(Flags).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public static implicit operator ChannelPermissions(ulong value)
        => new(value);

    public static implicit operator ChannelPermissions(Permissions value)
        => new(value);

    public static implicit operator Permissions(ChannelPermissions value)
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
