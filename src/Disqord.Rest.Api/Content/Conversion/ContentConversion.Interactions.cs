using System;
using System.Collections.Generic;
using System.Linq;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

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
                Choices = Optional.Convert(autoCompleteResponse.Choices, choices => choices.Select(choice => new ApplicationCommandOptionChoiceJsonModel
                {
                    Name = choice.Key,
                    Value = (serializer.GetJsonNode(choice.Value) as IJsonValue)!
                }).ToArray())
            };
        }
        else if (response is LocalInteractionMessageResponse messageResponse)
        {
            content.Data = new InteractionCallbackMessageDataJsonModel
            {
                Tts = messageResponse.IsTextToSpeech,
                Content = messageResponse.Content,
                Embeds = Optional.Convert(messageResponse.Embeds, embeds => embeds.Select(embed => embed.ToModel()).ToArray()),
                AllowedMentions = Optional.Convert(messageResponse.AllowedMentions, allowedMentions => allowedMentions.ToModel()),
                Components = Optional.Convert(messageResponse.Components, components => components.Select(component => component.ToModel()).ToArray()),
                Flags = messageResponse.Flags
            };

            if (messageResponse.Attachments.HasValue)
                attachments = messageResponse.Attachments.Value;
        }
        else if (response is LocalInteractionModalResponse modalResponse)
        {
            content.Data = new InteractionCallbackModalDataJsonModel
            {
                CustomId = modalResponse.CustomId,
                Title = modalResponse.Title,
                Components = Optional.Convert(modalResponse.Components, components => components.Select(component => component.ToModel()).ToArray())
            };
        }

        return content;
    }
}
