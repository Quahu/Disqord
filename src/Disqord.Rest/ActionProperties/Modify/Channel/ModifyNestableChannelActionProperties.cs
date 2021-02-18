namespace Disqord
{
    public abstract class ModifyNestableChannelActionProperties : ModifyGuildChannelActionProperties
    {
        public Optional<Snowflake?> CategoryId { internal get; set; }

        internal ModifyNestableChannelActionProperties()
        { }
    }
}
