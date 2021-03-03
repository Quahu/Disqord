using System;

namespace Disqord.Gateway.Api
{
    public readonly struct ShardId : IEquatable<ShardId>
    {
        /// <summary>
        ///     Represents an empty <see cref="ShardId"/>.
        /// </summary>
        public static readonly ShardId None = default;

        /// <summary>
        ///     Represents the <see cref="ShardId"/> of the default shard, i.e. the first shard.
        /// </summary>
        public static readonly ShardId Default = new(0, 1);

        public int Id { get; }

        public int Count { get; }

        public ShardId(int id, int count)
        {
            if (id < 0 || id > count)
                throw new ArgumentOutOfRangeException(nameof(id));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Id = id;
            Count = count;
        }

        public bool Equals(ShardId other)
            => Id == other.Id;

        public override int GetHashCode()
            => Id.GetHashCode();

        public override bool Equals(object obj)
            => obj is ShardId shardId && Equals(shardId);

        public override string ToString()
            => Count != 0
            ? $"Shard {Id + 1} (#{Id}) of {Count}"
            : "<no shard ID>";

        public static bool operator ==(ShardId left, ShardId right)
            => left.Equals(right);

        public static bool operator !=(ShardId left, ShardId right)
            => !left.Equals(right);

        public static ShardId ForGuildId(Snowflake guildId, int count)
            => new((int) ((guildId >> 22) % (ulong) count), count);
    }
}
