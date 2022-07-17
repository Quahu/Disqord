using System.Collections.Generic;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class ModifyWebhookMessageJsonRestRequestContent : JsonModelRestRequestContent, IAttachmentRestRequestContent
{
    [JsonProperty("content")]
    public Optional<string?> Content;

    [JsonProperty("embeds")]
    public Optional<EmbedJsonModel[]> Embeds;

    [JsonProperty("allowed_mentions")]
    public Optional<AllowedMentionsJsonModel> AllowedMentions;

    [JsonProperty("components")]
    public Optional<ComponentJsonModel[]> Components;

    [JsonProperty("attachments")]
    public Optional<IList<PartialAttachmentJsonModel>> Attachments;

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

        OptionalGuard.CheckValue(Embeds, embeds =>
        {
            for (var i = 0; i < embeds.Length; i++)
                embeds[i].Validate();
        });

        OptionalGuard.CheckValue(AllowedMentions, allowedMentions =>
        {
            allowedMentions.Validate();
        });

        OptionalGuard.CheckValue(Components, components =>
        {
            for (var i = 0; i < components.Length; i++)
                components[i].Validate();
        });
    }
}
