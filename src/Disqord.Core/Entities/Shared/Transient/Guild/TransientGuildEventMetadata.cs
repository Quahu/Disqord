using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientGuildEventMetadata : TransientEntity<GuildEventEntityMetadataJsonModel>, IGuildEventMetadata
    {
        /// <inheritdoc/>
        public IReadOnlyList<Snowflake> SpeakerIds => Optional.ConvertOrDefault(Model.SpeakerIds, x => x.ToReadOnlyList(), ReadOnlyList<Snowflake>.Empty);

        public TransientGuildEventMetadata(IClient client, GuildEventEntityMetadataJsonModel model)
            : base(client, model)
        { }
    }
}