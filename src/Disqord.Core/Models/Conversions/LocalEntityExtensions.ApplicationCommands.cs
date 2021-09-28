using System.Linq;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public static partial class LocalEntityExtensions
    {
        public static ApplicationCommandOptionJsonModel ToModel(this LocalSlashCommandOption option, IJsonSerializer serializer)
            => option == null ? null : new ApplicationCommandOptionJsonModel
            {
                Type = option.Type,
                Name = option.Name,
                Description = option.Description,
                Required = option.IsRequired,
                Choices = option.Choices.Select(x => x.ToModel(serializer)).ToArray(),
                Options = option.Options.Select(x => x.ToModel(serializer)).ToArray(),
                ChannelTypes = option.ChannelTypes.ToArray()
            };

        public static ApplicationCommandOptionChoiceJsonModel ToModel(this LocalSlashCommandOptionChoice choice, IJsonSerializer serializer)
            => choice == null ? null : new ApplicationCommandOptionChoiceJsonModel
            {
                Name = choice.Name,
                Value = serializer.GetJsonNode(choice.Value) as IJsonValue
            };
    }
}
