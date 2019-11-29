using System.IO;

namespace Disqord
{
    public sealed class ModifyCurrentUserProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<Stream> Avatar { internal get; set; }

        public Optional<string> Password { internal get; set; }

        public Optional<string> Discriminator { internal get; set; }

        internal ModifyCurrentUserProperties()
        { }
    }
}
