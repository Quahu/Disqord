using System.Runtime.CompilerServices;
using Qommon;

namespace Disqord.Rest.Api;

public static partial class RestContentValidation
{
    public static class GuildEvents
    {
        public static void ValidateName(string name, [CallerArgumentExpression("name")] string? argumentExpression = null)
        {
            Guard.IsNotNullOrWhiteSpace(name, argumentExpression);
            Guard.HasSizeBetweenOrEqualTo(name, Discord.Limits.Guild.Event.MinNameLength, Discord.Limits.Guild.Event.MaxNameLength, argumentExpression);
        }

        public static void ValidateDescription(Optional<string> description, [CallerArgumentExpression("description")] string? argumentExpression = null)
        {
            OptionalGuard.CheckValue(description, value =>
            {
                Guard.IsNotNullOrWhiteSpace(value);
                Guard.HasSizeBetweenOrEqualTo(value, Discord.Limits.Guild.Event.MinDescriptionLength, Discord.Limits.Guild.Event.MaxDescriptionLength);
            }, argumentExpression);
        }

        public static class Metadata
        {
            public static void ValidateLocation(Optional<string> location, [CallerArgumentExpression("location")] string? argumentExpression = null)
            {
                OptionalGuard.CheckValue(location, value =>
                {
                    Guard.IsNotNullOrWhiteSpace(value);
                    Guard.HasSizeBetweenOrEqualTo(value, Discord.Limits.Guild.Event.Metadata.MinLocationLength, Discord.Limits.Guild.Event.Metadata.MaxLocationLength);
                }, argumentExpression);
            }
        }
    }
}
