namespace Disqord
{
    public sealed class CreateGuildEventActionProperties
    {
        public Optional<Snowflake> ChannelId { internal get; set; }

        public Optional<string> Description { internal get; set; }
    }
}
