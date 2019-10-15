using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class AuditLogEntryOptionsModel
    {
        /// <summary>
        ///     <see cref="AuditLogAction.MembersPruned"/>
        /// </summary>
        [JsonProperty("delete_member_days")]
        public int DeleteMemberDays { get; set; }

        /// <summary>
        ///     <see cref="AuditLogAction.MembersPruned"/>
        /// </summary>
        [JsonProperty("members_removed")]
        public int MembersRemoved { get; set; }

        /// <summary>
        ///     <see cref="AuditLogAction.MessagesDeleted"/>
        /// </summary>
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }

        /// <summary>
        ///     <see cref="AuditLogAction.MessagesDeleted"/>
        /// </summary>
        [JsonProperty("count ")]
        public int Count { get; set; }

        /// <summary>
        ///     <see cref="AuditLogAction.OverwriteCreated"/>
        ///     <para/>
        ///     <see cref="AuditLogAction.OverwriteUpdated"/>
        ///     <para/>
        ///     <see cref="AuditLogAction.OverwriteDeleted"/>
        /// </summary>
        [JsonProperty("id")]
        public ulong Id { get; set; }

        /// <summary>
        ///     <see cref="AuditLogAction.OverwriteCreated"/>
        ///     <para/>
        ///     <see cref="AuditLogAction.OverwriteUpdated"/>
        ///     <para/>
        ///     <see cref="AuditLogAction.OverwriteDeleted"/>
        /// </summary>
        [JsonProperty("type")]
        public OverwriteTargetType Type { get; set; }

        /// <summary>
        ///     <see cref="AuditLogAction.OverwriteCreated"/>
        ///     <para/>
        ///     <see cref="AuditLogAction.OverwriteUpdated"/>
        ///     <para/>
        ///     <see cref="AuditLogAction.OverwriteDeleted"/>
        /// </summary>
        [JsonProperty("role_name")]
        public string RoleName { get; set; }
    }
}