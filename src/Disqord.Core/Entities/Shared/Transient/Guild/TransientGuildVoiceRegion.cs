using Disqord.Models;

namespace Disqord
{
    public class TransientGuildVoiceRegion : TransientEntity<VoiceRegionJsonModel>, IGuildVoiceRegion
    {
        /// <inheritdoc/>
        public Snowflake GuildId { get; }
        
        /// <inheritdoc/>
        public string Id => Model.Id;
        
        /// <inheritdoc/>
        public string Name => Model.Name;
        
        /// <inheritdoc/>
        public bool Vip => Model.Vip;
        
        /// <inheritdoc/>
        public bool Optimal => Model.Optimal;
        
        /// <inheritdoc/>
        public bool Deprecated => Model.Deprecated;
        
        /// <inheritdoc/>
        public bool Custom => Model.Custom;
        
        public TransientGuildVoiceRegion(IClient client, Snowflake guildId, VoiceRegionJsonModel model)
            : base(client, model)
        {
            GuildId = guildId;
        }
    }
}
