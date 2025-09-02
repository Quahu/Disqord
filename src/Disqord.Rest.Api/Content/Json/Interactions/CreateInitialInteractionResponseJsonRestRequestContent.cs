﻿using System;
using System.Collections.Generic;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class CreateInitialInteractionResponseJsonRestRequestContent : JsonModelRestRequestContent, IAttachmentRestRequestContent
{
    [JsonProperty("type")]
    public InteractionResponseType Type;

    [JsonProperty("data")]
    public Optional<object> Data;

    IList<PartialAttachmentJsonModel> IAttachmentRestRequestContent.Attachments
    {
        set
        {
            OptionalGuard.HasValue(Data);
            var data = Guard.IsAssignableToType<InteractionCallbackMessageDataJsonModel>(Data.Value);

            data.Attachments = new(value);
        }
    }

    protected override void OnValidate()
    {
        if (Type == default)
            throw new InvalidOperationException("The interaction response's type must be set.");

        switch (Type)
        {
            case InteractionResponseType.Pong or InteractionResponseType.PremiumRequired:
                OptionalGuard.HasNoValue(Data);
                break;
            case InteractionResponseType.ApplicationCommandAutoComplete:
                Guard.IsAssignableToType<InteractionCallbackAutoCompleteDataJsonModel>(Data.Value);
                break;
            case InteractionResponseType.ChannelMessage or InteractionResponseType.MessageUpdate:
                Guard.IsAssignableToType<InteractionCallbackMessageDataJsonModel>(Data.Value);
                break;
            case InteractionResponseType.Modal:
                Guard.IsAssignableToType<InteractionCallbackModalDataJsonModel>(Data.Value);
                break;
        }

        OptionalGuard.CheckValue(Data, data =>
        {
            Guard.IsNotNull(data);
            var model = Guard.IsAssignableToType<JsonModel>(data);
            model.Validate();
        });
    }
}
