using System;
using System.Collections.Generic;
using Qommon;

namespace Disqord
{
    public class LocalAutoModerationTriggerMetadata : ILocalConstruct
    {
        public Optional<IList<string>> Keywords { get; set; }

        public Optional<IList<AutoModerationKeywordPresetType>> Presets { get; set; }

        public LocalAutoModerationTriggerMetadata()
        { }

        public virtual LocalAutoModerationTriggerMetadata Clone()
            => MemberwiseClone() as LocalAutoModerationTriggerMetadata;

        object ICloneable.Clone()
            => Clone();
    }
}
