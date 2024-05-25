using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalPollAnswerExtensions
{
    public static TPollAnswer WithMedia<TPollAnswer>(this TPollAnswer answer, LocalPollMedia media)
        where TPollAnswer : LocalPollAnswer
    {
        answer.Media = media;
        return answer;
    }
}
