using System.Runtime.CompilerServices;
using Qommon;

namespace Disqord.Rest.Api
{
    internal static partial class ContentValidation
    {
        public static class GuildEvents
        {
            public static void ValidateName(string name, [CallerArgumentExpression("name")] string nameExpression = null)
            {
                Guard.IsNotNullOrWhiteSpace(name, nameExpression);
                Guard.HasSizeBetweenOrEqualTo(name, Discord.Limits.GuildEvents.MinNameLength, Discord.Limits.GuildEvents.MaxNameLength, nameExpression);
            }

            public static void ValidateDescription(Optional<string> description, [CallerArgumentExpression("description")] string nameExpression = null)
            {
                OptionalGuard.CheckValue(description, value =>
                {
                    Guard.IsNotNullOrWhiteSpace(value);
                    Guard.HasSizeBetweenOrEqualTo(value, Discord.Limits.GuildEvents.MinDescriptionLength, Discord.Limits.GuildEvents.MaxDescriptionLength);
                }, nameExpression);
            }

            public static class Metadata
            {
                public static void ValidateLocation(Optional<string> location, [CallerArgumentExpression("location")] string nameExpression = null)
                {
                    OptionalGuard.CheckValue(location, value =>
                    {
                        Guard.IsNotNullOrWhiteSpace(value);
                        Guard.HasSizeBetweenOrEqualTo(value, Discord.Limits.GuildEvents.Metadata.MinLocationLength, Discord.Limits.GuildEvents.Metadata.MaxLocationLength);
                    }, nameExpression);
                }
            }
        }
    }
}
