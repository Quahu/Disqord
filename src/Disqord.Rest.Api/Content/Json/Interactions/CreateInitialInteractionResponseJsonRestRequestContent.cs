using System;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateInitialInteractionResponseJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("type")]
        public InteractionResponseType Type;

        [JsonProperty("data")]
        public Optional<IInteractionCallbackData> Data;

        protected override void OnValidate()
        {
            if (Type == InteractionResponseType.ApplicationCommandAutoComplete && Data.Value is not InteractionCallbackAutoCompleteDataJsonModel)
            {
                throw new InvalidOperationException("Data must be an instance of InteractionCallbackAutoCompleteDataJsonModel.");
            }
        }
    }
}
