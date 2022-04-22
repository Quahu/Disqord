using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
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
        public UserJsonModel Author;

        [JsonProperty("member")]
        public Optional<MemberJsonModel> Member;

        [JsonProperty("content")]
        public string Content;

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp;

        [JsonProperty("edited_timestamp")]
        public DateTimeOffset? EditedTimestamp;

        [JsonProperty("tts")]
        public bool Tts;

        [JsonProperty("mention_everyone")]
        public bool MentionEveryone;

        [JsonProperty("mentions")]
        public UserJsonModel[] Mentions;

        [JsonProperty("mention_roles")]
        public Snowflake[] MentionRoles;

        [JsonProperty("mention_channels")]
        public Optional<ChannelMentionJsonModel[]> MentionChannels;

        [JsonProperty("attachments")]
        public AttachmentJsonModel[] Attachments;

        [JsonProperty("embeds")]
        public EmbedJsonModel[] Embeds;

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
        public Optional<MessageFlag> Flags;

        [JsonProperty("referenced_message")]
        public Optional<MessageJsonModel> ReferencedMessage;

        [JsonProperty("interaction")]
        public Optional<MessageInteractionJsonModel> Interaction;

        [JsonProperty("components")]
        public Optional<ComponentJsonModel[]> Components;

        [JsonProperty("sticker_items")]
        public Optional<StickerItemJsonModel[]> StickerItems;
    }
}
