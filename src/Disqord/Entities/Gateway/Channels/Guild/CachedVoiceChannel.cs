using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public sealed partial class CachedVoiceChannel : CachedNestedChannel, IVoiceChannel
    {
        public int Bitrate { get; private set; }

        public int MemberLimit { get; private set; }

        public IReadOnlyDictionary<Snowflake, CachedMember> Members
            => new ReadOnlyValuePredicateArgumentDictionary<Snowflake, CachedMember, Snowflake>(
                Guild._members, (x, id) => x.VoiceChannel != null && x.VoiceChannel.Id == id, Id);

        internal CachedVoiceChannel(CachedGuild guild, ChannelModel model) : base(guild, model)
        {
            Update(model);
        }

        internal override void Update(ChannelModel model)
        {
            if (model.Bitrate.HasValue)
                Bitrate = model.Bitrate.Value;

            if (model.UserLimit.HasValue)
                MemberLimit = model.UserLimit.Value;

            base.Update(model);
        }
    }
}
