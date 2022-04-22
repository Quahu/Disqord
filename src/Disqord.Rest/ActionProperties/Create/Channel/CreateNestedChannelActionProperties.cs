using Qommon;

namespace Disqord
{
    public abstract class CreateNestedChannelActionProperties : CreateGuildChannelActionProperties
    {
        public Optional<Snowflake> CategoryId { internal get; set; }

        internal CreateNestedChannelActionProperties()
        { }
    }
}
