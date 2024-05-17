using System.Collections.Generic;
using System.Linq;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalPoll : ILocalConstruct<LocalPoll>, IJsonConvertible<PollJsonModel>
{
    /// <summary>
    ///     Gets or sets the question of this poll.
    /// </summary>
    public Optional<LocalPollMedia> Question { get; set; }

    /// <summary>
    ///     Gets or sets the answers of this poll.
    /// </summary>
    public Optional<IList<LocalPollAnswer>> Answers { get; set; }

    /// <summary>
    ///     Gets or sets the duration in hours of this poll.
    /// </summary>
    public Optional<int> Duration { get; set; }

    /// <summary>
    ///     Gets or sets whether this poll allows selection of multiple answers.
    /// </summary>
    public Optional<bool> AllowMultiselect { get; set; }

    /// <summary>
    ///     Gets or sets the layout type of this poll.
    /// </summary>
    public Optional<PollLayoutType> LayoutType { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalPoll"/>.
    /// </summary>
    public LocalPoll()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalPoll"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalPoll(LocalPoll other)
    {
        Question = other.Question.Clone();
        Answers = other.Answers.DeepClone();
        Duration = other.Duration;
        AllowMultiselect = other.AllowMultiselect;
        LayoutType = other.LayoutType;
    }

    /// <inheritdoc/>
    public virtual LocalPoll Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public virtual PollJsonModel ToModel()
    {
        OptionalGuard.HasValue(Question);
        OptionalGuard.HasValue(Answers);

        return new PollJsonModel
        {
            Question = Question.Value.ToModel(),
            Answers = Answers.Value.Select(answer => answer.ToModel()).ToArray(),
            Duration = Duration,
            AllowMultiselect = AllowMultiselect.GetValueOrDefault(),
            LayoutType = LayoutType.GetValueOrDefault(PollLayoutType.Default)
        };
    }

    /// <summary>
    ///     Converts the specified poll to a <see cref="LocalPoll"/>.
    /// </summary>
    /// <param name="poll"> The poll to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalPoll"/>.
    /// </returns>
    public static LocalPoll CreateFrom(IPoll poll)
    {
        var localPoll = new LocalPoll
            { };

        return localPoll;
    }
}
