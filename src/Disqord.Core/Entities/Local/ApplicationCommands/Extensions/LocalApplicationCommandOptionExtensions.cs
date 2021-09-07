using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord
{
    public static class LocalApplicationCommandOptionExtensions
    {
        public static ApplicationCommandOptionJsonModel ToModel(this LocalApplicationCommandOption option, IJsonSerializer serializer)
            => option == null ? null : new ApplicationCommandOptionJsonModel
            {
                Type = option.Type,
                Name = option.Name,
                Description = option.Description,
                Required = option.Required,
                Choices = option.Choices.Select(x => x.ToModel(serializer)).ToArray(),
                Options = option.Options.Select(x => x.ToModel(serializer)).ToArray()
            };

        public static ApplicationCommandOptionChoiceJsonModel ToModel(this LocalApplicationCommandOptionChoice choice, IJsonSerializer serializer)
            => choice == null ? null : new ApplicationCommandOptionChoiceJsonModel
            {
                Name = choice.Name,
                Value = (IJsonValue)serializer.GetJsonNode(choice.Value)
            };
    }
}
