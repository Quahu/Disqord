using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

[JsonSkippedProperties("stickers")]
public class MessageJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("channel_id")]
    public Snowflake ChannelId;

    [JsonProperty("guild_id")]
    public Optional<Snowflake> GuildId;

    [JsonProperty("author")]
    public UserJsonModel Author = null!;

    [JsonProperty("member")]
    public Optional<MemberJsonModel> Member;

    [JsonProperty("content")]
    public string Content = null!;

    [JsonProperty("timestamp")]
    public DateTimeOffset Timestamp;

    [JsonProperty("edited_timestamp")]
    public DateTimeOffset? EditedTimestamp;

    [JsonProperty("tts")]
    public bool Tts;

    [JsonProperty("mention_everyone")]
    public bool MentionEveryone;

    [JsonProperty("mentions")]
    public UserJsonModel[] Mentions = null!;

    [JsonProperty("mention_roles")]
    public Snowflake[] MentionRoles = null!;

    [JsonProperty("mention_channels")]
    public Optional<ChannelMentionJsonModel[]> MentionChannels;

    [JsonProperty("attachments")]
    public AttachmentJsonModel[] Attachments = null!;

    [JsonProperty("embeds")]
    public EmbedJsonModel[] Embeds = null!;

    [JsonProperty("reactions")]
    public Optional<ReactionJsonModel[]> Reactions;

    [JsonProperty("nonce")]
    public Optional<string> Nonce;

    [JsonProperty("pinned")]
    public bool Pinned;

    [JsonProperty("webhook_id")]
    public Optional<Snowflake> WebhookId;

    [JsonProperty("type")]
    public UserMessageType Type;

    [JsonProperty("activity")]
    public Optional<MessageActivityJsonModel> Activity;

    [JsonProperty("application")]
    public Optional<MessageApplicationJsonModel> Application;

    [JsonProperty("application_id")]
    public Optional<Snowflake> ApplicationId;

    [JsonProperty("message_reference")]
    public Optional<MessageReferenceJsonModel> MessageReference;

    [JsonProperty("flags")]
    public Optional<MessageFlags> Flags;

    [JsonProperty("referenced_message")]
    public Optional<MessageJsonModel?> ReferencedMessage;

    [JsonProperty("interaction")]
    public Optional<MessageInteractionJsonModel> Interaction;

    public Optional<ChannelJsonModel> Thread => TryGetExtensionData(this, out var extensionData) && extensionData.TryGetValue("thread", out var threadModel)
        ? threadModel!.ToType<ChannelJsonModel>()!
        : Optional<ChannelJsonModel>.Empty;

    [JsonProperty("components")]
    public Optional<ComponentJsonModel[]> Components;

    [JsonProperty("sticker_items")]
    public Optional<StickerItemJsonModel[]> StickerItems;
}
