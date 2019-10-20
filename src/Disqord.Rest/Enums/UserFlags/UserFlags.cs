namespace Disqord
{
    public readonly struct UserFlags
    {
        public bool HasStaff => Discord.HasFlag(RawValue, (ulong) UserFlag.Staff);

        public bool HasPartner => Discord.HasFlag(RawValue, (ulong) UserFlag.Partner);

        public bool HasHypeSquad => Discord.HasFlag(RawValue, (ulong) UserFlag.HypeSquad);

        public bool HasBugHunter => Discord.HasFlag(RawValue, (ulong) UserFlag.BugHunter);

        public bool HasHypeSquadBravery => Discord.HasFlag(RawValue, (ulong) UserFlag.HypeSquadBravery);

        public bool HasHypeSquadBrilliance => Discord.HasFlag(RawValue, (ulong) UserFlag.HypeSquadBrilliance);

        public bool HasHypeSquadBalance => Discord.HasFlag(RawValue, (ulong) UserFlag.HypeSquadBalance);

        public bool HasEarlySupporter => Discord.HasFlag(RawValue, (ulong) UserFlag.EarlySupporter);

        public UserFlag Flags => (UserFlag) RawValue;

        public ulong RawValue { get; }

        public UserFlags(UserFlag flag) : this((ulong) flag)
        { }

        public UserFlags(ulong rawValue)
        {
            RawValue = rawValue;
        }

        public bool Has(UserFlag flag)
            => Discord.HasFlag(RawValue, (ulong) flag);

        public static implicit operator UserFlags(ulong value)
            => new UserFlags(value);

        public static implicit operator ulong(UserFlags value)
            => value.RawValue;

        public static implicit operator UserFlags(UserFlag value)
            => (ulong) value;

        public static implicit operator UserFlag(UserFlags value)
            => value.Flags;

        public override string ToString()
            => Flags.ToString();
    }
}
