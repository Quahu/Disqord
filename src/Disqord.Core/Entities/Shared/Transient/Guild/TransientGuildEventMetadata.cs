using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientGuildEventMetadata : TransientClientEntity<GuildEventEntityMetadataJsonModel>, IGuildEventMetadata
    {
        public IReadOnlyList<Snowflake> SpeakerIds => Optional.ConvertOrDefault(Model.SpeakerIds, x => x.ToReadOnlyList(), ReadOnlyList<Snowflake>.Empty);

        public string Location => Model.Location.GetValueOrDefault();

        public TransientGuildEventMetadata(IClient client, GuildEventEntityMetadataJsonModel model)
            : base(client, model)
        { }
    }
}
