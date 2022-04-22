using Qommon;

namespace Disqord
{
    public abstract class ModifyNestableChannelActionProperties : ModifyGuildChannelActionProperties
    {
        public virtual Optional<Snowflake?> CategoryId { internal get; set; }

        internal ModifyNestableChannelActionProperties()
        { }
    }
}
