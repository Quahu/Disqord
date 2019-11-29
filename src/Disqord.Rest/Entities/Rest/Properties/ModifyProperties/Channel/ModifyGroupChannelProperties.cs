using System.IO;

namespace Disqord
{
    public class ModifyGroupChannelProperties : ModifyChannelProperties
    {
        public Optional<Stream> Icon { internal get; set; }

        internal ModifyGroupChannelProperties()
        { }
    }
}
