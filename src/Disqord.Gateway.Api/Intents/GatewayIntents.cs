using System;
using System.Collections;
using System.Collections.Generic;
using Disqord.Utilities;

namespace Disqord.Gateway
{
    public readonly partial struct GatewayIntents : IEquatable<ulong>, IEquatable<GatewayIntent>, IEquatable<GatewayIntents>,
        IEnumerable<GatewayIntent>
    {
        public static GatewayIntents All => ALL_INTENTS_VALUE;

        public static GatewayIntents Unprivileged => UNPRIVILEGED_INTENTS_VALUE;

        public static GatewayIntents None => 0;

        public static GatewayIntents Recommended => RECOMMENDED;

        public static GatewayIntents RecommendedUnprivileged => Recommended - GatewayIntent.Members;

        public bool Guilds => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.Guilds);

        public bool Members => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.Members);

        public bool Bans => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.Bans);

        public bool Emojis => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.Emojis);

        public bool Integrations => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.Integrations);

        public bool Webhooks => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.Webhooks);

        public bool Invites => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.Invites);

        public bool VoiceStates => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.VoiceStates);

        public bool Presences => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.Presences);

        public bool GuildMessages => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.GuildMessages);

        public bool GuildReactions => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.GuildReactions);

        public bool GuildTyping => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.GuildTyping);

        public bool DirectMessages => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.DirectMessages);

        public bool DirectReactions => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.DirectReactions);

        public bool DirectTyping => FlagUtilities.HasFlag(RawValue, (ulong) GatewayIntent.DirectTyping);

        public GatewayIntent Intents => (GatewayIntent) RawValue;

        public ulong RawValue { get; }

        public GatewayIntents(GatewayIntent intent) : this((ulong) intent)
        { }

        public GatewayIntents(ulong rawValue)
        {
            RawValue = rawValue;
        }

        public bool Has(GatewayIntent intent)
            => FlagUtilities.HasFlag(RawValue, (ulong) intent);

        public bool Equals(ulong other)
            => RawValue == other;

        public bool Equals(GatewayIntent other)
            => RawValue == (ulong) other;

        public bool Equals(GatewayIntents other)
            => RawValue == other.RawValue;

        public override bool Equals(object obj)
        {
            if (obj is GatewayIntents intents)
                return Equals(intents);

            if (obj is GatewayIntent intent)
                return Equals(intent);

            if (obj is ulong rawValue)
                return Equals(rawValue);

            return false;
        }

        public override int GetHashCode()
            => RawValue.GetHashCode();

        public override string ToString()
            => Intents.ToString();

        public IEnumerator<GatewayIntent> GetEnumerator()
            => FlagUtilities.GetFlags(Intents).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public static implicit operator GatewayIntents(ulong value)
            => new GatewayIntents(value);

        public static implicit operator ulong(GatewayIntents value)
            => value.RawValue;

        public static implicit operator GatewayIntents(GatewayIntent value)
            => (ulong) value;

        public static implicit operator GatewayIntent(GatewayIntents value)
            => value.Intents;

        public static GatewayIntents operator +(GatewayIntents left, GatewayIntent right)
        {
            var rawValue = left.RawValue;
            FlagUtilities.SetFlag(ref rawValue, (ulong) right);
            return rawValue;
        }

        public static GatewayIntents operator -(GatewayIntents left, GatewayIntent right)
        {
            var rawValue = left.RawValue;
            FlagUtilities.UnsetFlag(ref rawValue, (ulong) right);
            return rawValue;
        }

        public static bool operator ==(GatewayIntents left, GatewayIntent right)
            => left.Equals(right);

        public static bool operator !=(GatewayIntents left, GatewayIntent right)
            => !left.Equals(right);
    }
}
