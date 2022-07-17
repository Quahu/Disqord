using System.Collections.Generic;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class ModifyMessageJsonRestRequestContent : JsonModelRestRequestContent, IAttachmentRestRequestContent
{
    [JsonProperty("content")]
    public Optional<string?> Content;

    [JsonProperty("embeds")]
    public Optional<EmbedJsonModel[]> Embeds;

    [JsonProperty("flags")]
    public Optional<MessageFlags> Flags;

    [JsonProperty("allowed_mentions")]
    public Optional<AllowedMentionsJsonModel> AllowedMentions;

    [JsonProperty("components")]
    public Optional<ComponentJsonModel[]> Components;

    [JsonProperty("sticker_ids")]
    public Optional<Snowflake[]> StickerIds;

    [JsonProperty("attachments")]
    public Optional<IList<PartialAttachmentJsonModel>> Attachments;

    IList<PartialAttachmentJsonModel> IAttachmentRestRequestContent.Attachments
    {
        set => Attachments = new(value);
    }

    protected override void OnValidate()
    {
        OptionalGuard.CheckValue(Components, components =>
        {
            for (var i = 0; i < components.Length; i++)
                components[i].Validate();
        });
    }
}
