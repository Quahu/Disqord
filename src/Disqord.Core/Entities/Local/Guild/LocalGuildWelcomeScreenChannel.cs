using Qommon;

namespace Disqord;

public class LocalGuildWelcomeScreenChannel : ILocalConstruct<LocalGuildWelcomeScreenChannel>
{
    public Optional<Snowflake> ChannelId { get; set; }

    public Optional<string> Description { get; set; }

    public Optional<LocalEmoji> Emoji { get; set; }

    public LocalGuildWelcomeScreenChannel()
    { }

    protected LocalGuildWelcomeScreenChannel(LocalGuildWelcomeScreenChannel other)
    {
        ChannelId = other.ChannelId;
        Description = other.Description;
        Emoji = other.Emoji.Clone();
    }

    public LocalGuildWelcomeScreenChannel(Snowflake channelId, string description, LocalEmoji? emoji = null)
    {
        ChannelId = channelId;
        Description = description;
        Emoji = Optional.FromNullable(emoji);
    }

    public virtual LocalGuildWelcomeScreenChannel Clone()
    {
        return new(this);
    }
}
