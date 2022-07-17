using System;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord;

public class TransientGuildTemplate : TransientClientEntity<GuildTemplateJsonModel>, IGuildTemplate
{
    /// <inheritdoc/>
    public Snowflake GuildId => Model.SourceGuildId;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public string Code => Model.Code;

    /// <inheritdoc/>
    public string? Description => Model.Description;

    /// <inheritdoc/>
    public int Uses => Model.UsageCount;

    /// <inheritdoc/>
    public Snowflake CreatorId => Model.CreatorId;

    /// <inheritdoc/>
    public IUser Creator => _creator ??= new TransientUser(Client, Model.Creator);

    private TransientUser? _creator;

    /// <inheritdoc/>
    public DateTimeOffset CreatedAt => Model.CreatedAt;

    /// <inheritdoc/>
    public DateTimeOffset UpdatedAt => Model.UpdatedAt;

    /// <inheritdoc/>
    public IJsonObject SerializedGuild => Model.SerializedSourceGuild;

    /// <inheritdoc/>
    public bool IsDirty => Model.IsDirty.GetValueOrDefault();

    public TransientGuildTemplate(IClient client, GuildTemplateJsonModel model)
        : base(client, model)
    { }
}
