using System;

namespace Disqord.Gateway.Api
{
    public readonly struct ShardId
    {
        public static readonly ShardId None = default;

        public int Id { get; }

        public int Count { get; }

        public ShardId(int id, int count)
        {
            if (id < 0 || id > count)
                throw new ArgumentOutOfRangeException(nameof(id));

            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Id = id;
            Count = count;
        }

        public override int GetHashCode()
            => HashCode.Combine(Id, Count);

        public override bool Equals(object obj)
            => obj is ShardId shardId && Id == shardId.Id;

        public override string ToString()
            => $"Shard {Id + 1} (#{Id}) of {Count}";
    }
}
