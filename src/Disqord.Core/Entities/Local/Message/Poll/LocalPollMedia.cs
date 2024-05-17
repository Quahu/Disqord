using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalPollMedia : ILocalConstruct<LocalPollMedia>, IJsonConvertible<PollMediaJsonModel>
{
    /// <summary>
    ///     Gets or sets the text of this poll media.
    /// </summary>
    public Optional<string> Text { get; set; }

    /// <summary>
    ///     Gets or sets the emoji of this poll media.
    /// </summary>
    public Optional<LocalEmoji> Emoji { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalPollMedia"/>.
    /// </summary>
    public LocalPollMedia()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalPollMedia"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalPollMedia(LocalPollMedia other)
    {
        Text = other.Text;
        Emoji = other.Emoji.Clone();
    }

    /// <inheritdoc/>
    public virtual LocalPollMedia Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public virtual PollMediaJsonModel ToModel()
    {
        return new PollMediaJsonModel
        {
            Text = Text,
            Emoji = Optional.Convert(Emoji, emoji => emoji.ToModel())
        };
    }

    /// <summary>
    ///     Converts the specified poll media to a <see cref="LocalPollMedia"/>.
    /// </summary>
    /// <param name="pollMedia"> The poll media to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalPollMedia"/>.
    /// </returns>
    public static LocalPollMedia CreateFrom(IPollMedia pollMedia)
    {
        var localPoll = new LocalPollMedia
            { };

        return localPoll;
    }
}
