using System;
using System.Collections;
using System.Collections.Generic;
using Disqord.Utilities;
using Qommon;

namespace Disqord;

[Obsolete("The GuildPermissions struct was removed. For checking permissions use HasFlag on the Permissions enum. Combining permissions should stay the same.", true)]
public readonly partial struct GuildPermissions : IEquatable<ulong>, IEquatable<Permissions>, IEquatable<GuildPermissions>,
    IEnumerable<Permissions>
{
    public static GuildPermissions All => AllPermissionsValue;

    public static GuildPermissions None => 0;

    public bool CreateInvites => Has(Permissions.CreateInvites);

    public bool KickMembers => Has(Permissions.KickMembers);

    public bool BanMembers => Has(Permissions.BanMembers);

    public bool Administrator => Has(Permissions.Administrator);

    public bool ManageChannels => Has(Permissions.ManageChannels);

    public bool ManageGuild => Has(Permissions.ManageGuild);

    public bool AddReactions => Has(Permissions.AddReactions);

    public bool ViewAuditLog => Has(Permissions.ViewAuditLog);

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

    public bool ViewGuildInsights => Has(Permissions.ViewGuildInsights);

    public bool Connect => Has(Permissions.Connect);

    public bool Speak => Has(Permissions.Speak);

    public bool MuteMembers => Has(Permissions.MuteMembers);

    public bool DeafenMembers => Has(Permissions.DeafenMembers);

    public bool MoveMembers => Has(Permissions.MoveMembers);

    public bool UseVoiceActivity => Has(Permissions.UseVoiceActivity);

    public bool SetNick => Has(Permissions.SetNick);

    public bool ManageNicks => Has(Permissions.ManageNicks);

    public bool ManageRoles => Has(Permissions.ManageRoles);

    public bool ManageWebhooks => Has(Permissions.ManageWebhooks);

    public bool ManageEmojisAndStickers => Has(Permissions.ManageEmojisAndStickers);

    public bool UseApplicationCommands => Has(Permissions.UseApplicationCommands);

    public bool RequestToSpeak => Has(Permissions.RequestToSpeak);

    public bool ManageEvents => Has(Permissions.ManageEvents);

    public bool ManageThreads => Has(Permissions.ManageThreads);

    public bool CreatePublicThreads => Has(Permissions.CreatePublicThreads);

    public bool CreatePrivateThreads => Has(Permissions.CreatePrivateThreads);

    public bool UseExternalStickers => Has(Permissions.UseExternalStickers);

    public bool SendMessagesInThreads => Has(Permissions.SendMessagesInThreads);

    public bool StartActivities => Has(Permissions.StartActivities);

    public bool ModerateMembers => Has(Permissions.ModerateMembers);

    public Permissions Flags => (Permissions) RawValue;

    public ulong RawValue { get; }

    public GuildPermissions(Permissions permissions)
        : this((ulong) permissions)
    { }

    public GuildPermissions(ulong rawValue)
    {
        RawValue = rawValue;
    }

    public bool Has(Permissions permissions)
        => Flags.HasFlag(permissions);

    public bool Equals(ulong other)
        => RawValue == other;

    public bool Equals(Permissions other)
        => RawValue == (ulong) other;

    public bool Equals(GuildPermissions other)
        => RawValue == other.RawValue;

    public override bool Equals(object? obj)
    {
        if (obj is GuildPermissions guildPermissions)
            return Equals(guildPermissions);

        if (obj is Permissions permission)
            return Equals(permission);

        if (obj is ulong rawValue)
            return Equals(rawValue);

        return false;
    }

    public override int GetHashCode()
        => RawValue.GetHashCode();

    public override string ToString()
    {
        if (Flags.HasFlag(Permissions.Administrator))
            return nameof(Permissions.Administrator);

        return Flags.ToString();
    }

    public IEnumerator<Permissions> GetEnumerator()
        => FlagUtilities.GetFlags(Flags).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public static implicit operator GuildPermissions(ulong value)
        => new(value);

    public static implicit operator GuildPermissions(Permissions value)
        => new(value);

    public static implicit operator Permissions(GuildPermissions value)
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
