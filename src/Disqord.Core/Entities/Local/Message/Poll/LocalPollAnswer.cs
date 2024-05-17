using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalPollAnswer : ILocalConstruct<LocalPollAnswer>, IJsonConvertible<PollAnswerJsonModel>
{
    /// <summary>
    ///     Gets or sets the poll media of this poll answer.
    /// </summary>
    public Optional<LocalPollMedia> Media { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalPollAnswer"/>.
    /// </summary>
    public LocalPollAnswer()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalPollAnswer"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalPollAnswer(LocalPollAnswer other)
    {
        Media = other.Media.Clone();
    }

    /// <inheritdoc/>
    public virtual LocalPollAnswer Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public virtual PollAnswerJsonModel ToModel()
    {
        OptionalGuard.HasValue(Media);

        return new PollAnswerJsonModel
        {
            PollMedia = Media.Value.ToModel()
        };
    }

    /// <summary>
    ///     Converts the specified poll answer to a <see cref="LocalPollAnswer"/>.
    /// </summary>
    /// <param name="pollAnswer"> The poll answer to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalPollAnswer"/>.
    /// </returns>
    public static LocalPollAnswer CreateFrom(IPollAnswer pollAnswer)
    {
        var localPoll = new LocalPollAnswer
            { };

        return localPoll;
    }
}
