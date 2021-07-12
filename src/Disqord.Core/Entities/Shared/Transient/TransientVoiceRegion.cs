using Disqord.Models;

namespace Disqord
{
    public class TransientVoiceRegion : TransientEntity<VoiceRegionJsonModel>, IVoiceRegion
    {
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

        public TransientVoiceRegion(IClient client, VoiceRegionJsonModel model)
            : base(client, model)
        { }
    }
}
