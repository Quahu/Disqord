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
        public bool Optimal => Model.Optimal;
        
        /// <inheritdoc/>
        public bool Deprecated => Model.Deprecated;

        public TransientVoiceRegion(IClient client, VoiceRegionJsonModel model)
            : base(client, model)
        { }
    }
}
