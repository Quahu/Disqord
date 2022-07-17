using System;
using System.Globalization;
using Disqord.Models;
using Microsoft.Extensions.Logging;
using Qommon;

namespace Disqord.AuditLogs;

public class TransientGuildAuditLogChanges : IGuildAuditLogChanges
{
    /// <inheritdoc/>
    public AuditLogChange<string> Name { get; }

    /// <inheritdoc/>
    public AuditLogChange<string?> Description { get; }

    /// <inheritdoc/>
    public AuditLogChange<string?> IconHash { get; }

    /// <inheritdoc/>
    public AuditLogChange<string?> SplashHash { get; }

    /// <inheritdoc/>
    public AuditLogChange<string?> DiscoverySplashHash { get; }

    /// <inheritdoc/>
    public AuditLogChange<string?> BannerHash { get; }

    /// <inheritdoc/>
    public AuditLogChange<Snowflake> OwnerId { get; }

    /// <inheritdoc/>
    public AuditLogChange<Optional<IUser>> Owner { get; }

    /// <inheritdoc/>
    public AuditLogChange<CultureInfo> PreferredLocale { get; }

    /// <inheritdoc/>
    public AuditLogChange<Snowflake?> AfkChannelId { get; }

    /// <inheritdoc/>
    public AuditLogChange<int> AfkTimeout { get; }

    /// <inheritdoc/>
    public AuditLogChange<Snowflake?> RulesChannelId { get; }

    /// <inheritdoc/>
    public AuditLogChange<Snowflake?> PublicUpdatesChannelId { get; }

    /// <inheritdoc/>
    public AuditLogChange<GuildMfaLevel> MfaLevel { get; }

    /// <inheritdoc/>
    public AuditLogChange<GuildVerificationLevel> VerificationLevel { get; }

    /// <inheritdoc/>
    public AuditLogChange<GuildContentFilterLevel> ContentFilterLevel { get; }

    /// <inheritdoc/>
    public AuditLogChange<GuildNotificationLevel> DefaultNotificationLevel { get; }

    /// <inheritdoc/>
    public AuditLogChange<string?> VanityUrlCode { get; }

    /// <inheritdoc/>
    public AuditLogChange<int?> PruneDays { get; }

    /// <inheritdoc/>
    public AuditLogChange<bool> IsWidgetEnabled { get; }

    /// <inheritdoc/>
    public AuditLogChange<Snowflake?> WidgetChannelId { get; }

    /// <inheritdoc/>
    public AuditLogChange<Snowflake?> SystemChannelId { get; }

    public TransientGuildAuditLogChanges(IClient client, AuditLogJsonModel auditLogJsonModel, AuditLogEntryJsonModel model)
    {
        for (var i = 0; i < model.Changes.Value.Length; i++)
        {
            var change = model.Changes.Value[i];
            switch (change.Key)
            {
                case "name":
                {
                    Name = AuditLogChange<string>.Convert(change);
                    break;
                }
                case "description":
                {
                    Description = AuditLogChange<string?>.Convert(change);
                    break;
                }
                case "icon_hash":
                {
                    IconHash = AuditLogChange<string?>.Convert(change);
                    break;
                }
                case "splash_hash":
                {
                    SplashHash = AuditLogChange<string?>.Convert(change);
                    break;
                }
                case "discovery_splash_hash":
                {
                    DiscoverySplashHash = AuditLogChange<string?>.Convert(change);
                    break;
                }
                case "banner_hash":
                {
                    BannerHash = AuditLogChange<string?>.Convert(change);
                    break;
                }
                case "owner_id":
                {
                    OwnerId = AuditLogChange<Snowflake>.Convert(change);
                    IUser? oldOwner = null;
                    var oldOwnerModel = Array.Find(auditLogJsonModel.Users, x => x.Id == OwnerId.OldValue.Value);
                    if (oldOwnerModel != null)
                        oldOwner = new TransientUser(client, oldOwnerModel);

                    IUser? newOwner = null;
                    var newOwnerModel = Array.Find(auditLogJsonModel.Users, x => x.Id == OwnerId.NewValue.Value);
                    if (newOwnerModel != null)
                        newOwner = new TransientUser(client, newOwnerModel);

                    Owner = new AuditLogChange<Optional<IUser>>(Optional.FromNullable(oldOwner), Optional.FromNullable(newOwner));
                    break;
                }
                case "preferred_locale":
                {
                    PreferredLocale = AuditLogChange<CultureInfo>.Convert<string>(change, Discord.Internal.GetLocale);
                    break;
                }
                case "afk_channel_id":
                {
                    AfkChannelId = AuditLogChange<Snowflake?>.Convert(change);
                    break;
                }
                case "afk_timeout":
                {
                    AfkTimeout = AuditLogChange<int>.Convert(change);
                    break;
                }
                case "rules_channel_id":
                {
                    RulesChannelId = AuditLogChange<Snowflake?>.Convert(change);
                    break;
                }
                case "public_updates_channel_id":
                {
                    PublicUpdatesChannelId = AuditLogChange<Snowflake?>.Convert(change);
                    break;
                }
                case "mfa_level":
                {
                    MfaLevel = AuditLogChange<GuildMfaLevel>.Convert(change);
                    break;
                }
                case "verification_level":
                {
                    VerificationLevel = AuditLogChange<GuildVerificationLevel>.Convert(change);
                    break;
                }
                case "explicit_content_filter":
                {
                    ContentFilterLevel = AuditLogChange<GuildContentFilterLevel>.Convert(change);
                    break;
                }
                case "default_message_notifications":
                {
                    DefaultNotificationLevel = AuditLogChange<GuildNotificationLevel>.Convert(change);
                    break;
                }
                case "vanity_url_code":
                {
                    VanityUrlCode = AuditLogChange<string?>.Convert(change);
                    break;
                }
                case "prune_days":
                {
                    PruneDays = AuditLogChange<int?>.Convert(change);
                    break;
                }
                case "widget_enabled":
                {
                    IsWidgetEnabled = AuditLogChange<bool>.Convert(change);
                    break;
                }
                case "widget_channel_id":
                {
                    WidgetChannelId = AuditLogChange<Snowflake?>.Convert(change);
                    break;
                }
                case "system_channel_id":
                {
                    SystemChannelId = AuditLogChange<Snowflake?>.Convert(change);
                    break;
                }
                default:
                {
                    client.Logger.LogDebug("Unknown key {0} for {1}", change.Key, this);
                    break;
                }
            }
        }
    }
}
