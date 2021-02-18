using System;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
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
        public ChannelMentionJsonModel[] MentionChannels;

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
        public int Type;

        [JsonProperty("activity")]
        public Optional</*MessageActivity*/JsonModel> Activity;

        [JsonProperty("application")]
        public Optional</*MessageApplication*/JsonModel> Application;

        [JsonProperty("message_reference")]
        public Optional</*MessageReference*/JsonModel> MessageReference;

        [JsonProperty("flags")]
        public Optional<MessageFlag> Flags;
    }
}
