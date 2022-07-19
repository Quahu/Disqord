using System;
using System.Linq;
using Qommon;

namespace Disqord.Rest.Api;

internal static partial class ActionPropertiesConversion
{
    public static ModifyRoleJsonRestRequestContent ToContent(this Action<ModifyRoleActionProperties> action, out Optional<int> position)
    {
        Guard.IsNotNull(action);

        var properties = new ModifyRoleActionProperties();
        action(properties);

        var content = new ModifyRoleJsonRestRequestContent
        {
            Name = properties.Name,
            Permissions = properties.Permissions,
            Color = Optional.Convert(properties.Color, color => color?.RawValue ?? 0),
            Hoist = properties.IsHoisted,
            Icon = properties.Icon,
            Mentionable = properties.IsMentionable
        };

        if (properties.UnicodeEmoji.TryGetValue(out var unicodeEmoji))
        {
            Guard.IsNotAssignableToType<LocalCustomEmoji>(unicodeEmoji);
            OptionalGuard.HasValue(unicodeEmoji.Name);
            Guard.IsNotNullOrWhiteSpace(unicodeEmoji.Name.Value);

            content.UnicodeEmoji = Optional.Convert(properties.UnicodeEmoji, emoji => emoji.Name.Value!);
        }

        position = properties.Position;

        return content;
    }

    public static ModifyMemberJsonRestRequestContent ToContent(this Action<ModifyMemberActionProperties> action, out Optional<string> nick)
    {
        Guard.IsNotNull(action);

        var properties = new ModifyMemberActionProperties();
        action(properties);

        nick = properties.Nick;

        var content = new ModifyMemberJsonRestRequestContent
        {
            Nick = properties.Nick,
            Roles = Optional.Convert(properties.RoleIds, x => x.ToArray()),
            ChannelId = properties.VoiceChannelId,
            Mute = properties.Mute,
            Deaf = properties.Deaf,
            CommunicationDisabledUntil = properties.TimedOutUntil
        };

        return content;
    }

    public static ModifyCurrentMemberJsonRestRequestContent ToContent(this Action<ModifyCurrentMemberActionProperties> action)
    {
        Guard.IsNotNull(action);

        var properties = new ModifyCurrentMemberActionProperties();
        action(properties);

        var content = new ModifyCurrentMemberJsonRestRequestContent
        {
            Nick = properties.Nick
        };

        return content;
    }
}
