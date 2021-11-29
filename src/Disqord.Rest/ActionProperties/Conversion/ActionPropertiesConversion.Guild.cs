using System;
using Qommon;

namespace Disqord.Rest.Api
{
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
                Permissions = Optional.Convert(properties.Permissions, permissions => permissions.RawValue),
                Color = Optional.Convert(properties.Color, color => color?.RawValue ?? 0),
                Hoist = properties.IsHoisted,
                Icon = properties.Icon,
                Mentionable = properties.IsMentionable,
                UnicodeEmoji = Optional.Convert(properties.UnicodeEmoji, emoji => emoji.Name)
            };
            position = properties.Position;

            return content;
        }
    }
}
