using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientGuildEventMetadata : TransientEntity<GuildEventEntityMetadataJsonModel>, IGuildEventMetadata
    {
        public IReadOnlyList<Snowflake> SpeakerIds => Optional.ConvertOrDefault(Model.SpeakerIds, x => x.ToReadOnlyList(), ReadOnlyList<Snowflake>.Empty);

        public string Location => Model.Location.GetValueOrDefault();

        public TransientGuildEventMetadata(GuildEventEntityMetadataJsonModel model)
            : base(model)
        { }
    }
}
