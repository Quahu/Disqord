using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class AuditLogEntryOptionsJsonModel : JsonModel
{
    /// <summary>
    ///     <see cref="AuditLogActionType.MembersPruned"/>
    /// </summary>
    [JsonProperty("delete_member_days")]
    public Optional<int> DeleteMemberDays;

    /// <summary>
    ///     <see cref="AuditLogActionType.MembersPruned"/>
    /// </summary>
    [JsonProperty("members_removed")]
    public Optional<int> MembersRemoved;

    /// <summary>
    ///     <see cref="AuditLogActionType.MembersMoved"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.MessagesDeleted"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.MessagesBulkDeleted"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.MessagePinned"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.MessageUnpinned"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.AutoModerationMessageBlocked"/>
    /// </summary>
    [JsonProperty("channel_id")]
    public Optional<Snowflake> ChannelId;

    /// <summary>
    ///     <see cref="AuditLogActionType.MessagePinned"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.MessageUnpinned"/>
    /// </summary>
    [JsonProperty("message_id")]
    public Optional<Snowflake> MessageId;

    /// <summary>
    ///     <see cref="AuditLogActionType.MembersMoved"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.MembersDisconnected"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.MessagesDeleted"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.MessagesBulkDeleted"/>
    /// </summary>
    [JsonProperty("count")]
    public Optional<int> Count;

    /// <summary>
    ///     <see cref="AuditLogActionType.OverwriteCreated"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.OverwriteUpdated"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.OverwriteDeleted"/>
    /// </summary>
    [JsonProperty("id")]
    public Optional<Snowflake> Id;

    /// <summary>
    ///     <see cref="AuditLogActionType.OverwriteCreated"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.OverwriteUpdated"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.OverwriteDeleted"/>
    /// </summary>
    [JsonProperty("type")]
    public Optional<OverwriteTargetType> Type;

    /// <summary>
    ///     <see cref="AuditLogActionType.OverwriteCreated"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.OverwriteUpdated"/>
    ///     <para/>
    ///     <see cref="AuditLogActionType.OverwriteDeleted"/>
    /// </summary>
    [JsonProperty("role_name")]
    public Optional<string> RoleName;

    /// <summary>
    ///     <see cref="AuditLogActionType.ApplicationCommandPermissionsUpdate"/>
    /// </summary>
    [JsonProperty("application_id")]
    public Optional<Snowflake> ApplicationId;

    /// <summary>
    ///     <see cref="AuditLogActionType.AutoModerationMessageBlocked"/>
    /// </summary>
    [JsonProperty("auto_moderation_rule_name")]
    public Optional<string> AutoModerationRuleName;

    /// <summary>
    ///     <see cref="AuditLogActionType.AutoModerationMessageBlocked"/>
    /// </summary>
    [JsonProperty("auto_moderation_rule_trigger_type")]
    public Optional<AutoModerationRuleTrigger> AutoModerationRuleTriggerType;
}