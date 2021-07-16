using System;
using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Gateway;
using Disqord.Gateway.Api.Models;

namespace Disqord
{
    public class TransientGatewayGuild : TransientGuild, IGatewayGuild, ITransientEntity<GatewayGuildJsonModel>
    {
        public DateTimeOffset JoinedAt => Model.JoinedAt;

        public bool IsLarge => Model.Large;

        public bool IsUnavailable => Model.Unavailable.GetValueOrDefault();

        public int MemberCount => Model.MemberCount;

        public IReadOnlyDictionary<Snowflake, IVoiceState> VoiceStates
        {
            get
            {
                if (_voiceStates == null)
                    _voiceStates = Model.VoiceStates.ToReadOnlyDictionary(Client, (x, _) => x.UserId, (x, client) => new TransientVoiceState(client, x) as IVoiceState);

                return _voiceStates;
            }
        }
        private IReadOnlyDictionary<Snowflake, IVoiceState> _voiceStates;

        public IReadOnlyDictionary<Snowflake, IMember> Members
        {
            get
            {
                if (_members == null)
                    _members = Model.Members.ToReadOnlyDictionary((Client, Id), (x, _) => x.User.Value.Id, (x, tuple) =>
                    {
                        var (client, guildId) = tuple;
                        return new TransientMember(client, guildId, x) as IMember;
                    });

                return _members;
            }
        }
        private IReadOnlyDictionary<Snowflake, IMember> _members;

        public IReadOnlyDictionary<Snowflake, IGuildChannel> Channels
        {
            get
            {
                if (_channels == null)
                    _channels = Model.Channels.ToReadOnlyDictionary(Client, (x, _) => x.Id, (x, client) => TransientGuildChannel.Create(client, x) as IGuildChannel);

                return _channels;
            }
        }
        private IReadOnlyDictionary<Snowflake, IGuildChannel> _channels;

        public IReadOnlyDictionary<Snowflake, IPresence> Presences
        {
            get
            {
                if (_presences == null)
                    _presences = Model.Presences.ToReadOnlyDictionary(Client, (x, _) => x.User.Id, (x, client) => new TransientPresence(client, x) as IPresence);

                return _presences;
            }
        }
        private IReadOnlyDictionary<Snowflake, IPresence> _presences;

        public IReadOnlyDictionary<Snowflake, IStage> Stages
        {
            get
            {
                if (_Stages == null)
                    _Stages = Model.StageInstances.ToReadOnlyDictionary(Client, (x, _) => x.Id, (x, client) => new TransientStage(client, x) as IStage);

                return _Stages;
            }
        }
        private IReadOnlyDictionary<Snowflake, IStage> _Stages;

        public new GatewayGuildJsonModel Model => base.Model as GatewayGuildJsonModel;

        public new IGatewayClient Client => base.Client as IGatewayClient;

        public TransientGatewayGuild(IClient client, GatewayGuildJsonModel model)
            : base(client, model)
        { }
    }
}
