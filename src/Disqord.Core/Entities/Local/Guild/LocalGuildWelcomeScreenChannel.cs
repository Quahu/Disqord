using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalGuildWelcomeScreenChannel : ILocalConstruct<LocalGuildWelcomeScreenChannel>, IJsonConvertible<WelcomeScreenChannelJsonModel>
{
    /// <summary>
    ///     Gets or sets the ID of the channel of this welcome screen channel.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<Snowflake> ChannelId { get; set; }

    /// <summary>
    ///     Gets or sets the description of this welcome screen channel.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<string> Description { get; set; }

    /// <summary>
    ///     Gets or sets the emoji of this welcome screen channel.
    /// </summary>
    public Optional<LocalEmoji?> Emoji { get; set; }

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

    /// <inheritdoc/>
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

    /// <summary>
    ///     Converts the specified welcome screen channel to a <see cref="LocalGuildWelcomeScreenChannel"/>.
    /// </summary>
    /// <param name="channel"> The welcome screen channel to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalGuildWelcomeScreenChannel"/>.
    /// </returns>
    public static LocalGuildWelcomeScreenChannel CreateFrom(IGuildWelcomeScreenChannel channel)
    {
        return new LocalGuildWelcomeScreenChannel
        {
            ChannelId = channel.ChannelId,
            Description = channel.Description,
            Emoji = LocalEmoji.FromEmoji(channel.Emoji)
        };
    }
}
