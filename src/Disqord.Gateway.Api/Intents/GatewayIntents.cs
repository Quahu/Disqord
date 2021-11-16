using System;
using System.Collections;
using System.Collections.Generic;
using Disqord.Utilities;
using Qommon;

namespace Disqord.Gateway
{
    public readonly partial struct GatewayIntents : IEquatable<ulong>, IEquatable<GatewayIntent>, IEquatable<GatewayIntents>,
        IEnumerable<GatewayIntent>
    {
        public static GatewayIntents All => AllIntentsValue;

        public static GatewayIntents Unprivileged => UnprivilegedIntentsValue;

        public static GatewayIntents Recommended => RecommendedValue;

        public static GatewayIntents RecommendedUnprivileged => Recommended & ~GatewayIntent.Members;

        public static GatewayIntents None => 0;

        public bool Guilds => Has(GatewayIntent.Guilds);

        public bool Members => Has(GatewayIntent.Members);

        public bool Bans => Has(GatewayIntent.Bans);

        public bool EmojisAndStickers => Has(GatewayIntent.EmojisAndStickers);

        public bool Integrations => Has(GatewayIntent.Integrations);

        public bool Webhooks => Has(GatewayIntent.Webhooks);

        public bool Invites => Has(GatewayIntent.Invites);

        public bool VoiceStates => Has(GatewayIntent.VoiceStates);

        public bool Presences => Has(GatewayIntent.Presences);

        public bool GuildMessages => Has(GatewayIntent.GuildMessages);

        public bool GuildReactions => Has(GatewayIntent.GuildReactions);

        public bool GuildTyping => Has(GatewayIntent.GuildTyping);

        public bool DirectMessages => Has(GatewayIntent.DirectMessages);

        public bool DirectReactions => Has(GatewayIntent.DirectReactions);

        public bool DirectTyping => Has(GatewayIntent.DirectTyping);

        public bool GuildEvents => Has(GatewayIntent.GuildEvents);

        public GatewayIntent Flags => (GatewayIntent) RawValue;

        public ulong RawValue { get; }

        public GatewayIntents(GatewayIntent intents)
            : this((ulong) intents)
        { }

        public GatewayIntents(ulong rawValue)
        {
            RawValue = rawValue;
        }

        public bool Has(GatewayIntent intents)
            => Flags.HasFlag(intents);

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
            => Flags.ToString();

        public IEnumerator<GatewayIntent> GetEnumerator()
            => FlagUtilities.GetFlags(Flags).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public static implicit operator GatewayIntents(ulong value)
            => new(value);

        public static implicit operator GatewayIntents(GatewayIntent value)
            => (ulong) value;

        public static implicit operator GatewayIntent(GatewayIntents value)
            => value.Flags;

        public static bool operator ==(GatewayIntents left, GatewayIntents right)
            => left.RawValue == right.RawValue;

        public static bool operator !=(GatewayIntents left, GatewayIntents right)
            => left.RawValue != right.RawValue;

        public static GatewayIntents operator ~(GatewayIntents value)
            => ~value.RawValue;

        public static GatewayIntents operator &(GatewayIntents left, GatewayIntents right)
            => left.RawValue & right.RawValue;

        public static GatewayIntents operator ^(GatewayIntents left, GatewayIntents right)
            => left.RawValue ^ right.RawValue;

        public static GatewayIntents operator |(GatewayIntents left, GatewayIntents right)
            => left.RawValue | right.RawValue;

        [Obsolete("The '+' and '-' operators have been removed, use '|' and '& ~' respectively.", true)]
        public static GatewayIntents operator +(GatewayIntents left, GatewayIntents right)
            => Throw.InvalidOperationException<GatewayIntents>();

        [Obsolete("The '+' and '-' operators have been removed, use '|' and '& ~' respectively.", true)]
        public static GatewayIntents operator -(GatewayIntents left, GatewayIntents right)
            => Throw.InvalidOperationException<GatewayIntents>();
    }
}
