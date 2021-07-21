﻿using Disqord.Models;

namespace Disqord
{
    public class TransientVoiceRegion : TransientEntity<VoiceRegionJsonModel>, IVoiceRegion
    {
        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public string Id => Model.Id;

        /// <inheritdoc/>
        public bool IsOptimal => Model.Optimal;

        /// <inheritdoc/>
        public bool IsDeprecated => Model.Deprecated;

        public TransientVoiceRegion(IClient client, VoiceRegionJsonModel model)
            : base(client, model)
        { }
    }
}
