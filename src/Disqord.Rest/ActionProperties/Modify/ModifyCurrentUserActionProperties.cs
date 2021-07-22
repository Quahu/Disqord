using System.IO;

namespace Disqord
{
    public sealed class ModifyCurrentUserActionProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<Stream> Avatar { internal get; set; }

        internal ModifyCurrentUserActionProperties()
        { }
    }
}
