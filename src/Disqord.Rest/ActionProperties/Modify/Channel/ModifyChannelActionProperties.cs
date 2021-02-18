namespace Disqord
{
    public abstract class ModifyChannelActionProperties
    {
        public Optional<string> Name { internal get; set; }

        internal ModifyChannelActionProperties()
        { }
    }
}
