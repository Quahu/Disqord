using System.Collections.Generic;

namespace Disqord.Rest.ActionProperties.Modify
{
    public sealed class ModifyGuildDiscoveryMetadataActionProperties
    {
        public Optional<int> PrimaryCategoryId { internal get; set; }

        public Optional<IEnumerable<string>> Keywords { internal get; set; }

        public Optional<bool> HasEmojis { internal get; set; }

        internal ModifyGuildDiscoveryMetadataActionProperties()
        { }
    }
}