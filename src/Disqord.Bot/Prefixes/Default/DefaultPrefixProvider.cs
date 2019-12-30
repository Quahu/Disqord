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

        public DefaultPrefixProvider AddPrefix(char value, bool ignoreCase = true)
        {
            Prefixes.Add(new CharPrefix(value, ignoreCase));
            return this;
        }

        public DefaultPrefixProvider AddMentionPrefix()
        {
            Prefixes.Add(MentionPrefix.Instance);
            return this;
        }

        public bool TryFind(CachedUserMessage message, out IPrefix foundPrefix, out string output)
        {
            if (!string.IsNullOrEmpty(message.Content))
            {
                foreach (var prefix in Prefixes)
                {
                    if (prefix.TryFind(message, out output))
                    {
                        foundPrefix = prefix;
                        return true;
                    }
                }
            }

            foundPrefix = null;
            output = null;
            return false;
        }

        public ValueTask<IEnumerable<IPrefix>> GetPrefixesAsync(CachedUserMessage message)
            => new ValueTask<IEnumerable<IPrefix>>(Prefixes);
    }
}
