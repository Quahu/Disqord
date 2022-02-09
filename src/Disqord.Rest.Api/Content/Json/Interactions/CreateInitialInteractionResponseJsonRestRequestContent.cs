using System;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class CreateInitialInteractionResponseJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("type")]
        public InteractionResponseType Type;

        [JsonProperty("data")]
        public Optional<JsonModel> Data;

        protected override void OnValidate()
        {
            if (Type == default)
                throw new InvalidOperationException("The interaction response's type must be set.");

            switch (Type)
            {
                case InteractionResponseType.Pong:
                    OptionalGuard.HasNoValue(Data);
                    break;
                case InteractionResponseType.ApplicationCommandAutoComplete:
                    Guard.IsAssignableToType<InteractionCallbackAutoCompleteDataJsonModel>(Data.Value);
                    break;
                case InteractionResponseType.ChannelMessage or InteractionResponseType.MessageUpdate:
                    Guard.IsAssignableToType<InteractionCallbackMessageDataJsonModel>(Data.Value);
                    break;
            }
        }
    }
}
