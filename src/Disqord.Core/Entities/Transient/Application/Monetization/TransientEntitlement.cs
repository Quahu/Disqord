using System;
using Disqord.Models;
using Qommon;

namespace Disqord;

/// <inheritdoc cref="IEntitlement"/>
public class TransientEntitlement : TransientClientEntity<EntitlementJsonModel>, IEntitlement
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc/>
    public Snowflake? GuildId => Model.GuildId.GetValueOrDefault();

    /// <inheritdoc/>
    public Snowflake SkuId => Model.SkuId;

    /// <inheritdoc/>
    public Snowflake ApplicationId => Model.ApplicationId;

    /// <inheritdoc/>
    public Snowflake? UserId => Model.UserId.GetValueOrDefault();

    /// <inheritdoc/>
    public EntitlementType Type => Model.Type;

    /// <inheritdoc/>
    public bool Deleted => Model.Deleted;

    /// <inheritdoc/>
    public DateTimeOffset? StartsAt => Model.StartsAt;

    /// <inheritdoc/>
    public DateTimeOffset? EndsAt => Model.StartsAt;

    public TransientEntitlement(IClient client, EntitlementJsonModel model)
        : base(client, model)
    { }
}
