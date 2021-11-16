﻿namespace Disqord.AuditLogs
{
    public interface IGuildEventAuditLogChanges
    {
        AuditLogChange<Snowflake> ChannelId { get; }

        AuditLogChange<string> Name { get; }

        AuditLogChange<string> Description { get; }

        AuditLogChange<GuildEventTargetType> TargetEntityType { get; }

        AuditLogChange<string> Location { get; }

        AuditLogChange<PrivacyLevel> PrivacyLevel { get; }

        AuditLogChange<GuildEventStatus> Status { get; }
    }
}
