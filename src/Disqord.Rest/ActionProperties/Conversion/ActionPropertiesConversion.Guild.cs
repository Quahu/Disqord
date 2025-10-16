using System;
using System.Linq;
using Disqord.Models;
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
            Colors = Optional.Conditional(properties.Colors.HasValue, new RoleColorsJsonModel
            {
                PrimaryColor = properties.Colors.Value.GetValueOrDefault().PrimaryColor,
                SecondaryColor = properties.Colors.Value.GetValueOrDefault().SecondaryColor,
                TertiaryColor = properties.Colors.Value.GetValueOrDefault().TertiaryColor
            }),
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

    public static ModifyMemberJsonRestRequestContent ToContent(this Action<ModifyMemberActionProperties> action)
    {
        Guard.IsNotNull(action);

        var properties = new ModifyMemberActionProperties();
        action(properties);

        var content = new ModifyMemberJsonRestRequestContent
        {
            Nick = properties.Nick,
            Roles = Optional.Convert(properties.RoleIds, x => x.ToArray()),
            ChannelId = properties.VoiceChannelId,
            Mute = properties.Mute,
            Deaf = properties.Deaf,
            CommunicationDisabledUntil = properties.TimedOutUntil,
            Flags = properties.Flags
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
            Nick = properties.Nick,
            Avatar = properties.Avatar,
            Banner = properties.Banner,
            Bio = properties.Bio,
        };

        return content;
    }
}
