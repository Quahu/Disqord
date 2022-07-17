using System.Collections.Generic;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class CreateMessageJsonRestRequestContent : JsonModelRestRequestContent, IAttachmentRestRequestContent
{
    [JsonProperty("content")]
    public Optional<string?> Content;

    [JsonProperty("nonce")]
    public Optional<string> Nonce;

    [JsonProperty("tts")]
    public Optional<bool> Tts;

    [JsonProperty("embeds")]
    public Optional<EmbedJsonModel[]> Embeds;

    [JsonProperty("allowed_mentions")]
    public Optional<AllowedMentionsJsonModel> AllowedMentions;

    [JsonProperty("message_reference")]
    public Optional<MessageReferenceJsonModel> MessageReference;

    [JsonProperty("components")]
    public Optional<ComponentJsonModel[]> Components;

    [JsonProperty("sticker_ids")]
    public Optional<Snowflake[]> StickerIds;

    [JsonProperty("attachments")]
    public Optional<IList<PartialAttachmentJsonModel>> Attachments;

    [JsonProperty("flags")]
    public Optional<MessageFlags> Flags;

    IList<PartialAttachmentJsonModel> IAttachmentRestRequestContent.Attachments
    {
        set => Attachments = new(value);
    }

    protected override void OnValidate()
    {
        OptionalGuard.CheckValue(Content, content =>
        {
            if (content != null)
            {
                Guard.HasSizeLessThanOrEqualTo(content, Discord.Limits.Message.MaxContentLength);
            }
        });

        OptionalGuard.CheckValue(Nonce, nonce =>
        {
            Guard.IsNotNull(nonce);
            Guard.HasSizeLessThanOrEqualTo(nonce, Discord.Limits.Message.MaxNonceLength);
        });

        OptionalGuard.CheckValue(Embeds, embeds =>
        {
            Guard.HasSizeLessThanOrEqualTo(embeds, Discord.Limits.Message.MaxEmbedAmount);

            for (var i = 0; i < embeds.Length; i++)
                embeds[i].Validate();
        });

        OptionalGuard.CheckValue(AllowedMentions, allowedMentions =>
        {
            allowedMentions.Validate();
        });

        OptionalGuard.CheckValue(MessageReference, referenceJsonModel =>
        {
            referenceJsonModel.Validate();
        });

        OptionalGuard.CheckValue(Components, components =>
        {
            for (var i = 0; i < components.Length; i++)
                components[i].Validate();
        });
    }
}
