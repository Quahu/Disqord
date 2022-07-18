using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalGuildWelcomeScreenChannel : ILocalConstruct<LocalGuildWelcomeScreenChannel>, IJsonConvertible<WelcomeScreenChannelJsonModel>
{
    public Optional<Snowflake> ChannelId { get; set; }

    public Optional<string> Description { get; set; }

    public Optional<LocalEmoji> Emoji { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalGuildWelcomeScreenChannel"/>.
    /// </summary>
    public LocalGuildWelcomeScreenChannel()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalGuildWelcomeScreenChannel"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalGuildWelcomeScreenChannel(LocalGuildWelcomeScreenChannel other)
    {
        ChannelId = other.ChannelId;
        Description = other.Description;
        Emoji = other.Emoji.Clone();
    }

    /// <inheritdoc/>
    public virtual LocalGuildWelcomeScreenChannel Clone()
    {
        return new(this);
    }

    /// <inheritdoc />
    public WelcomeScreenChannelJsonModel ToModel()
    {
        OptionalGuard.HasValue(ChannelId);
        OptionalGuard.HasValue(Description);

        return new()
        {
            ChannelId = ChannelId.Value,
            Description = Description.Value,
            EmojiId = (Emoji.GetValueOrDefault() as LocalCustomEmoji)?.Id.GetValueOrNullable(),
            EmojiName = Emoji.GetValueOrDefault()?.Name.GetValueOrDefault()
        };
    }
}
