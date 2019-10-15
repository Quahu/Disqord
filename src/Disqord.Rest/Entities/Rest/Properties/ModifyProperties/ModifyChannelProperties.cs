using System.Collections.Generic;

namespace Disqord
{
    public sealed class ModifyChannelProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<int> Position { internal get; set; }

        public Optional<string> Topic { internal get; set; }

        public Optional<bool> IsNSFW { internal get; set; }

        public Optional<int> Slowmode { internal get; set; }

        public Optional<int> Bitrate { internal get; set; }

        public Optional<int> UserLimit { internal get; set; }

        public Optional<IReadOnlyList<LocalOverwrite>> Overwrites { internal get; set; }

        public Optional<ulong> CategoryId { internal get; set; }

        internal ModifyChannelProperties()
        { }
    }
}
