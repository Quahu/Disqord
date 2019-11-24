using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class AuditLogEntryOptionsModel
    {
        /// <summary>
        ///     <see cref="AuditLogType.MembersPruned"/>
        /// </summary>
        [JsonProperty("delete_member_days")]
        public int DeleteMemberDays { get; set; }

        /// <summary>
        ///     <see cref="AuditLogType.MembersPruned"/>
        /// </summary>
        [JsonProperty("members_removed")]
        public int MembersRemoved { get; set; }

        /// <summary>
        ///     <see cref="AuditLogType.MembersMoved"/>
        ///     <para/>
        ///     <see cref="AuditLogType.MessagesDeleted"/>
        ///     <para/>
        ///     <see cref="AuditLogType.MessagesBulkDeleted"/>
        ///     <para/>
        ///     <see cref="AuditLogType.MessagePinned"/>
        ///     <para/>
        ///     <see cref="AuditLogType.MessageUnpinned"/>
        /// </summary>
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }

        /// <summary>
        ///     <see cref="AuditLogType.MessagePinned"/>
        ///     <para/>
        ///     <see cref="AuditLogType.MessageUnpinned"/>
        /// </summary>
        [JsonProperty("message_id")]
        public ulong MessageId { get; set; }

        /// <summary>
        ///     <see cref="AuditLogType.MembersMoved"/>
        ///     <para/>
        ///     <see cref="AuditLogType.MembersDisconnected"/>
        ///     <para/>
        ///     <see cref="AuditLogType.MessagesDeleted"/>
        ///     <para/>
        ///     <see cref="AuditLogType.MessagesBulkDeleted"/>
        /// </summary>
        [JsonProperty("count ")]
        public int Count { get; set; }

        /// <summary>
        ///     <see cref="AuditLogType.OverwriteCreated"/>
        ///     <para/>
        ///     <see cref="AuditLogType.OverwriteUpdated"/>
        ///     <para/>
        ///     <see cref="AuditLogType.OverwriteDeleted"/>
        /// </summary>
        [JsonProperty("id")]
        public ulong Id { get; set; }

        /// <summary>
        ///     <see cref="AuditLogType.OverwriteCreated"/>
        ///     <para/>
        ///     <see cref="AuditLogType.OverwriteUpdated"/>
        ///     <para/>
        ///     <see cref="AuditLogType.OverwriteDeleted"/>
        /// </summary>
        [JsonProperty("type")]
        public OverwriteTargetType Type { get; set; }

        /// <summary>
        ///     <see cref="AuditLogType.OverwriteCreated"/>
        ///     <para/>
        ///     <see cref="AuditLogType.OverwriteUpdated"/>
        ///     <para/>
        ///     <see cref="AuditLogType.OverwriteDeleted"/>
        /// </summary>
        [JsonProperty("role_name")]
        public string RoleName { get; set; }
    }
}