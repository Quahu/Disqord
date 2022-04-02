﻿using Disqord.Models;

namespace Disqord
{
    /// <inheritdoc cref="ISelectionComponentOption"/>
    public class TransientSelectionComponentOption : TransientClientEntity<SelectOptionJsonModel>, ISelectionComponentOption
    {
        /// <inheritdoc/>
        public string Label => Model.Label.GetValueOrDefault();

        /// <inheritdoc/>
        public string Value => Model.Value.GetValueOrDefault();

        /// <inheritdoc/>
        public string Description => Model.Description.GetValueOrDefault();

        /// <inheritdoc/>
        public IEmoji Emoji => Optional.ConvertOrDefault(Model.Emoji, TransientEmoji.Create);

        /// <inheritdoc/>
        public bool IsDefault => Model.Default.GetValueOrDefault();

        public TransientSelectionComponentOption(IClient client, SelectOptionJsonModel model)
            : base(client, model)
        { }
    }
}
