using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientThreadMember : TransientClientEntity<ThreadMemberJsonModel>, IThreadMember
    {
        public Snowflake Id => Model.UserId.Value;

        public Snowflake ThreadId => Model.Id.Value;

        public DateTimeOffset JoinedAt => Model.JoinTimestamp;

        public int Flags => Model.Flags;

        public TransientThreadMember(IClient client, ThreadMemberJsonModel model)
            : base(client, model)
        { }
    }
}
