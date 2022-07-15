using System.Runtime.CompilerServices;
using Disqord.Models;
using Qommon;

namespace Disqord.Rest.Api
{
    public static partial class RestContentValidation
    {
        // TODO: validate localizations
        public static class ApplicationCommands
        {
            public static void ValidateName(string name, [CallerArgumentExpression("name")] string argumentExpression = null)
            {
                Guard.IsNotNullOrWhiteSpace(name, argumentExpression);
                Guard.HasSizeBetweenOrEqualTo(name, Discord.Limits.ApplicationCommands.MinNameLength, Discord.Limits.ApplicationCommands.MaxNameLength, argumentExpression);
            }

            public static void ValidateDescription(Optional<string> description, [CallerArgumentExpression("description")] string argumentExpression = null)
            {
                OptionalGuard.CheckValue(description, description =>
                {
                    Guard.IsNotNullOrWhiteSpace(description);
                    Guard.HasSizeBetweenOrEqualTo(description, Discord.Limits.ApplicationCommands.MinDescriptionLength, Discord.Limits.ApplicationCommands.MaxDescriptionLength);
                }, argumentExpression);
            }

            public static void ValidateOptions(Optional<ApplicationCommandOptionJsonModel[]> options, [CallerArgumentExpression("options")] string argumentExpression = null)
            {
                OptionalGuard.CheckValue(options, options =>
                {
                    Guard.IsNotNull(options);
                    Guard.HasSizeLessThanOrEqualTo(options, Discord.Limits.ApplicationCommands.MaxOptionAmount);

                    var isRequired = true;
                    foreach (var option in options)
                    {
                        if (!isRequired && option.Required.GetValueOrDefault())
                        {
                            Throw.ArgumentException("Required options must appear before optional ones.", nameof(options));
                            return;
                        }

                        isRequired = option.Required.GetValueOrDefault();
                        option.Validate();
                    }
                }, argumentExpression);
            }
        }
    }
}
