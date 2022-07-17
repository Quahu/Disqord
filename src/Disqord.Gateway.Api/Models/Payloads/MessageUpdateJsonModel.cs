using System;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

// Properties are mirrored from MessageJsonModel,
// except almost all of them are optional.
public class MessageUpdateJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("channel_id")]
    public Snowflake ChannelId;

    [JsonProperty("guild_id")]
    public Optional<Snowflake> GuildId;

    [JsonProperty("author")]
    public Optional<UserJsonModel> Author;

    [JsonProperty("member")]
    public Optional<MemberJsonModel> Member;

    [JsonProperty("content")]
    public Optional<string> Content;

    [JsonProperty("timestamp")]
    public Optional<DateTimeOffset> Timestamp;

    [JsonProperty("edited_timestamp")]
    public Optional<DateTimeOffset?> EditedTimestamp;

    [JsonProperty("tts")]
    public Optional<bool> Tts;

    [JsonProperty("mention_everyone")]
    public Optional<bool> MentionEveryone;

    [JsonProperty("mentions")]
    public Optional<UserJsonModel[]> Mentions;

    [JsonProperty("mention_roles")]
    public Optional<Snowflake[]> MentionRoles;

    [JsonProperty("mention_channels")]
    public Optional<ChannelMentionJsonModel[]> MentionChannels;

    [JsonProperty("attachments")]
    public Optional<AttachmentJsonModel[]> Attachments;

    [JsonProperty("embeds")]
    public Optional<EmbedJsonModel[]> Embeds;

    [JsonProperty("reactions")]
    public Optional<ReactionJsonModel[]> Reactions;

    [JsonProperty("nonce")]
    public Optional<string> Nonce;

    [JsonProperty("pinned")]
    public Optional<bool> Pinned;

    [JsonProperty("webhook_id")]
    public Optional<Snowflake> WebhookId;

    [JsonProperty("type")]
    public Optional<int> Type;

    [JsonProperty("activity")]
    public Optional<MessageActivityJsonModel> Activity;

    [JsonProperty("application")]
    public Optional<MessageApplicationJsonModel> Application;

    [JsonProperty("message_reference")]
    public Optional<MessageReferenceJsonModel> MessageReference;

    [JsonProperty("flags")]
    public Optional<MessageFlags> Flags;

    [JsonProperty("referenced_message")]
    public Optional<MessageJsonModel> ReferencedMessage;

    [JsonProperty("components")]
    public Optional<ComponentJsonModel[]> Components;

    [JsonProperty("sticker_items")]
    public Optional<StickerItemJsonModel[]> StickerItems;
}
