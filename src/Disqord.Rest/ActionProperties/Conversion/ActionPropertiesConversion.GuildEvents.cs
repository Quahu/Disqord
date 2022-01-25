using System;
using Disqord.Models;
using Qommon;

namespace Disqord.Rest.Api
{
    internal static partial class ActionPropertiesConversion
    {
        public static ModifyGuildScheduledEventJsonRestRequestContent ToContent(this Action<ModifyGuildEventActionProperties> action)
        {
            Guard.IsNotNull(action);

            var properties = new ModifyGuildEventActionProperties();
            action(properties);

            var content = new ModifyGuildScheduledEventJsonRestRequestContent
            {
                ChannelId = properties.ChannelId,
                Name = properties.Name,
                PrivacyLevel = properties.PrivacyLevel,
                ScheduledStartTime = properties.StartsAt,
                ScheduledEndTime = properties.EndsAt,
                Description = properties.Description,
                EntityType = properties.TargetType,
                Status = properties.Status,
                Image = properties.CoverImage
            };

            if (properties.Location.HasValue)
            {
                content.EntityMetadata = new GuildScheduledEventEntityMetadataJsonModel
                {
                    Location = properties.Location.Value
                };
            }

            return content;
        }
    }
}
