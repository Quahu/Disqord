using System.Collections.Generic;
using System.ComponentModel;
using Qommon;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalPollExtensions
{
    public static TPoll WithQuestion<TPoll>(this TPoll poll, string text)
        where TPoll : LocalPoll
    {
        var media = new LocalPollMedia()
        {
            Text = text
        };

        return poll.WithQuestion(media);
    }

    public static TPoll WithQuestion<TPoll>(this TPoll poll, LocalPollMedia question)
        where TPoll : LocalPoll
    {
        poll.Question = question;
        return poll;
    }

    public static TPoll AddAnswer<TPoll>(this TPoll poll, string text, LocalEmoji? emoji = null)
        where TPoll : LocalPoll
    {
        var media = new LocalPollMedia
        {
            Text = text,
            Emoji = Optional.FromNullable(emoji)
        };

        var answer = new LocalPollAnswer()
        {
            Media = media
        };

        return poll.AddAnswer(answer);
    }

    public static TPoll AddAnswer<TPoll>(this TPoll poll, LocalPollAnswer answer)
        where TPoll : LocalPoll
    {
        if (poll.Answers.Add(answer, out var list))
            poll.Answers = new(list);

        return poll;
    }

    public static TPoll WithAnswers<TPoll>(this TPoll poll, IEnumerable<LocalPollAnswer> answers)
        where TPoll : LocalPoll
    {
        Guard.IsNotNull(answers);

        if (poll.Answers.With(answers, out var list))
            poll.Answers = new(list);

        return poll;
    }

    public static TPoll WithAnswers<TPoll>(this TPoll poll, params LocalPollAnswer[] answers)
        where TPoll : LocalPoll
    {
        return poll.WithAnswers(answers as IEnumerable<LocalPollAnswer>);
    }

    public static TPoll WithDuration<TPoll>(this TPoll poll, int duration)
        where TPoll : LocalPoll
    {
        poll.Duration = duration;
        return poll;
    }

    public static TPoll WithAllowMultiselect<TPoll>(this TPoll poll, bool allowMultiselect = true)
        where TPoll : LocalPoll
    {
        poll.AllowMultiselect = allowMultiselect;
        return poll;
    }

    public static TPoll WithLayoutType<TPoll>(this TPoll poll, PollLayoutType layoutType)
        where TPoll : LocalPoll
    {
        poll.LayoutType = layoutType;
        return poll;
    }
}
