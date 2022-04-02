﻿using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class SelectOptionJsonModel : JsonModel
    {
        [JsonProperty("label")]
        public Optional<string> Label;

        [JsonProperty("value")]
        public Optional<string> Value;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("emoji")]
        public Optional<EmojiJsonModel> Emoji;

        [JsonProperty("default")]
        public Optional<bool> Default;

        protected override void OnValidate()
        {
            OptionalGuard.HasValue(Label);
            Guard.IsNotNullOrWhiteSpace(Label.Value);
            Guard.IsLessThanOrEqualTo(Label.Value.Length, Discord.Limits.Components.Selection.Option.MaxLabelLength);

            OptionalGuard.HasValue(Value);
            Guard.IsNotNullOrWhiteSpace(Value.Value);
            Guard.IsLessThanOrEqualTo(Value.Value.Length, Discord.Limits.Components.Selection.Option.MaxValueLength);

            OptionalGuard.CheckValue(Description, description =>
            {
                Guard.IsNotNullOrWhiteSpace(description);
                Guard.IsLessThanOrEqualTo(description.Length, Discord.Limits.Components.Selection.Option.MaxDescriptionLength);
            });

        }
    }
}
