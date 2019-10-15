using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Disqord.Rest;
using Disqord.Rest.AuditLogs;

namespace Disqord
{
    public interface IGuild : ISnowflakeEntity, IDeletable
    {
        string Name { get; }

        string IconHash { get; }

        string SplashHash { get; }

        Snowflake OwnerId { get; }

        string VoiceRegionId { get; }

        Snowflake? AfkChannelId { get; }

        int AfkTimeout { get; }

        Snowflake? EmbedChannelId { get; }

        bool IsEmbedEnabled { get; }

        VerificationLevel VerificationLevel { get; }

        DefaultNotificationLevel DefaultNotificationLevel { get; }

        ExplicitFilterLevel ExplicitFilterLevel { get; }

        IReadOnlyDictionary<Snowflake, IRole> Roles { get; }

        IReadOnlyList<IGuildEmoji> Emojis { get; }

        IReadOnlyList<string> Features { get; }

        MfaLevel MfaLevel { get; }

        Snowflake? ApplicationId { get; }

        bool IsWidgetEnabled { get; }

        Snowflake? WidgetChannelId { get; }

        Snowflake? SystemChannelId { get; }

        int MaxPresenceCount { get; }

        int MaxMemberCount { get; }

        string VanityUrlCode { get; }

        string Description { get; }

        string BannerHash { get; }

        BoostTier BoostTier { get; }

        int BoostingMemberCount { get; }

        CultureInfo PreferredLocale { get; }

        Task<RestMember> GetMemberAsync(Snowflake id, RestRequestOptions options = null);

        string GetIconUrl(ImageFormat? imageFormat = null, int size = 2048);

        string GetSplashUrl(int size = 2048);

        Task<IReadOnlyList<RestAuditLog>> GetAuditLogsAsync(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null);

        Task<IReadOnlyList<T>> GetAuditLogsAsync<T>(int limit = 100, Snowflake? userId = null, Snowflake? startFromId = null, RestRequestOptions options = null) where T : RestAuditLog;

        Task<RestRole> CreateRoleAsync(Action<CreateRoleProperties> action, RestRequestOptions options = null);
    }
}
