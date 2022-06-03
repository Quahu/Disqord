using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
    /// <inheritdoc cref="IAutoModerationTriggerMetadata"/>
    public class TransientAutoModerationTriggerMetadata : TransientEntity<AutoModerationTriggerMetadataJsonModel>, IAutoModerationTriggerMetadata
    {
        /// <inheritdoc/>
        public IReadOnlyList<string> Keywords
        {
            get
            {
                if (!Model.KeywordFilter.HasValue)
                    return ReadOnlyList<string>.Empty;

                return _keywords ??= Model.KeywordFilter.Value.ToReadOnlyList();
            }
        }
        private IReadOnlyList<string> _keywords;

        /// <inheritdoc/>
        public IReadOnlyList<string> Presets
        {
            get
            {
                if (!Model.KeywordLists.HasValue)
                    return ReadOnlyList<string>.Empty;

                return _presets ??= Model.KeywordLists.Value.ToReadOnlyList();
            }
        }
        private IReadOnlyList<string> _presets;

        public TransientAutoModerationTriggerMetadata(AutoModerationTriggerMetadataJsonModel model)
            : base(model)
        { }
    }
}
