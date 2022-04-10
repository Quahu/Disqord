using System.Runtime.CompilerServices;
using Disqord.Models;
using Qommon;

namespace Disqord.Rest.Api
{
    internal static partial class ContentValidation
    {
        public static class ApplicationCommands
        {
            public static void ValidateName(string name, [CallerArgumentExpression("name")] string nameExpression = null)
            {
                Guard.IsNotNullOrWhiteSpace(name, nameExpression);
                Guard.HasSizeBetweenOrEqualTo(name, Discord.Limits.ApplicationCommands.MinNameLength, Discord.Limits.ApplicationCommands.MaxNameLength, nameExpression);
            }

            public static void ValidateDescription(Optional<string> description, [CallerArgumentExpression("description")] string nameExpression = null)
            {
                OptionalGuard.CheckValue(description, value =>
                {
                    Guard.IsNotNullOrWhiteSpace(value);
                    Guard.HasSizeBetweenOrEqualTo(value, Discord.Limits.ApplicationCommands.MinDescriptionLength, Discord.Limits.ApplicationCommands.MaxDescriptionLength);
                }, nameExpression);
            }

            public static void ValidateOptions(Optional<ApplicationCommandOptionJsonModel[]> options, [CallerArgumentExpression("options")] string nameExpression = null)
            {
                OptionalGuard.CheckValue(options, value =>
                {
                    Guard.IsNotNull(value);
                    Guard.HasSizeLessThanOrEqualTo(value, Discord.Limits.ApplicationCommands.MaxOptionAmount);

                    var isRequired = true;
                    foreach (var option in value)
                    {
                        if (!isRequired && option.Required.GetValueOrDefault())
                        {
                            Throw.ArgumentException("Required options must appear before optional ones.", nameof(value));
                            return;
                        }

                        isRequired = option.Required.GetValueOrDefault();
                        option.Validate();
                    }
                }, nameExpression);
            }
        }
    }
}
