using System;
using System.Collections.Generic;
using System.Linq;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    internal static partial class ContentConversion
    {
        public static CreateInitialInteractionResponseJsonRestRequestContent ToContent(this ILocalInteractionResponse response,
            IJsonSerializer serializer, out IList<LocalAttachment> attachments)
        {
            Guard.IsNotNull(response);

            var content = new CreateInitialInteractionResponseJsonRestRequestContent
            {
                Type = response.Type
            };
            attachments = Array.Empty<LocalAttachment>();

            if (response is LocalInteractionAutoCompleteResponse autoCompleteResponse)
            {
                content.Data = new InteractionCallbackAutoCompleteDataJsonModel
                {
                    Choices = Optional.Convert(autoCompleteResponse.Choices, choices => choices?.Select(choice => choice.ToModel(serializer)).ToArray())
                };
            }
            else if (response is LocalInteractionMessageResponse messageResponse)
            {
                if (content.Type is not InteractionResponseType.MessageUpdate)
                {
                    messageResponse.Validate();
                    content.Data = new InteractionCallbackMessageDataJsonModel
                    {
                        Tts = Optional.Conditional(messageResponse.IsTextToSpeech, true),
                        Content = Optional.FromNullable(messageResponse.Content),
                        Embeds = Optional.Conditional(messageResponse.Embeds.Count != 0,
                            x => x.Select(x => x.ToModel()).ToArray(), messageResponse.Embeds),
                        AllowedMentions = Optional.FromNullable(messageResponse.AllowedMentions.ToModel()),
                        Components = Optional.Conditional(messageResponse.Components.Count != 0,
                            x => x.Select(x => x.ToModel()).ToArray(), messageResponse.Components),
                        Flags = Optional.Conditional(messageResponse.Flags != MessageFlag.None,
                            messageResponse.Flags)
                    };
                }
                else
                {
                    // TODO: make properties properly optional via different LocalInteractionResponse types?
                    content.Data = new InteractionCallbackMessageDataJsonModel
                    {
                        //Tts = messageResponse.IsTextToSpeech,
                        Content = messageResponse.Content,
                        Embeds = messageResponse.Embeds.Select(x => x.ToModel()).ToArray(),
                        AllowedMentions = messageResponse.AllowedMentions?.ToModel(),
                        Components = messageResponse.Components.Select(x => x.ToModel()).ToArray(),
                        Flags = messageResponse.Flags
                    };
                }
                attachments = messageResponse.Attachments;
            }
            else if (response is LocalInteractionModalResponse modalResponse)
            {
                content.Data = new InteractionCallbackModalDataJsonModel
                {
                    CustomId = modalResponse.CustomId,
                    Title = modalResponse.Title,
                    Components = Optional.Convert(modalResponse.Components, components => components?.Select(component => component?.ToModel()).ToArray())
                };
            }

            return content;
        }
    }
}
