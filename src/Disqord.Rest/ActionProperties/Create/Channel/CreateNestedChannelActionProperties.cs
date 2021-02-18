namespace Disqord
{
    public abstract class CreateNestedChannelActionProperties : CreateGuildChannelActionProperties
    {
        public Optional<Snowflake> ParentId { internal get; set; }

        internal CreateNestedChannelActionProperties()
        { }
    }
}
