using System;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord
{
    public class TransientGuildTemplate : TransientEntity<GuildTemplateJsonModel>, IGuildTemplate
    {
        /// <inheritdoc/>
        public string Code => Model.Code;
        
        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public string Description => Model.Description;

        /// <inheritdoc/>
        public int UsageCount => Model.UsageCount;

        /// <inheritdoc/>
        public Snowflake CreatorId => Model.CreatorId;

        /// <inheritdoc/>
        public IUser Creator => new TransientUser(Client, Model.Creator);

        /// <inheritdoc/>
        public DateTimeOffset CreatedAt => Model.CreatedAt;

        /// <inheritdoc/>
        public DateTimeOffset UpdatedAt => Model.UpdatedAt;

        /// <inheritdoc/>
        public Snowflake GuildId => Model.SourceGuildId;

        /// <inheritdoc/>
        public IJsonObject SerializedSourceGuild => Model.SerializedSourceGuild;

        /// <inheritdoc/>
        public bool? IsDirty => Model.IsDirty;

        public TransientGuildTemplate(IClient client, GuildTemplateJsonModel model) 
            : base(client, model)
        { }
    }
}