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
        /// <remarks>
        ///     This property's <see cref="Count"/> will always return <c>-1</c>.
        /// </remarks>
        public static readonly ShardId Default = 0;

        /// <summary>
        ///     Gets the zero-based shard ID.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets the total count of shards.
        /// </summary>
        /// <remarks>
        ///     Returns <c>-1</c>, if the total count is unknown.
        /// </remarks>
        public int Count { get; }

        public ShardId(int id, int count)
        {
            if (id < 0 || id >= count)
                throw new ArgumentOutOfRangeException(nameof(id));

            Id = id;
            Count = count;
        }

        private ShardId(int id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            Id = id;
            Count = -1;
        }

        public bool Equals(ShardId other)
        {
            if (Count == 0 && other.Count == 0)
                return true;

            else if (Count == 0 || other.Count == 0)
                return false;

            return Id == other.Id;
        }

        public override int GetHashCode()
            => Count != 0
            ? Id.GetHashCode()
            : -1;

        public override bool Equals(object obj)
            => obj is ShardId shardId && Equals(shardId);

        public override string ToString()
            => Count != 0
            ? Count != -1
                ? $"Shard {Id + 1} (#{Id}) of {Count}"
                : $"Shard {Id + 1} (#{Id})"
            : "<invalid shard ID>";

        public static bool operator ==(ShardId left, ShardId right)
            => left.Equals(right);

        public static bool operator !=(ShardId left, ShardId right)
            => !left.Equals(right);

        public static implicit operator ShardId(int id)
            => new(id);

        public static ShardId ForGuildId(Snowflake guildId, int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            var id = (int) ((guildId >> 22) % (ulong) count);
            return new(id, count);
        }
    }
}
