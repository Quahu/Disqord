using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Bot.Prefixes
{
    public sealed partial class DefaultPrefixProvider : IPrefixProvider
    {
        public PrefixCollection Prefixes { get; }

        public DefaultPrefixProvider()
        {
            Prefixes = new PrefixCollection();
        }

        public DefaultPrefixProvider AddPrefix(string value, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            Prefixes.Add(new StringPrefix(value, comparison));
            return this;
        }

        public DefaultPrefixProvider AddPrefix(char value, bool caseSensitive = false)
        {
            Prefixes.Add(new CharPrefix(value, caseSensitive));
            return this;
        }

        public DefaultPrefixProvider AddMentionPrefix()
        {
            Prefixes.Add(MentionPrefix.Instance);
            return this;
        }

        public ValueTask<IEnumerable<IPrefix>> GetPrefixesAsync(CachedUserMessage message)
            => new ValueTask<IEnumerable<IPrefix>>(Prefixes.ToArray());
    }
}
