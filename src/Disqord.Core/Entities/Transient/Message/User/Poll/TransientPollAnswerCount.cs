using Disqord.Models;

namespace Disqord;

public class TransientPollAnswerCount(PollAnswerCountsJsonModel model) : TransientEntity<PollAnswerCountsJsonModel>(model), IPollAnswerCount
{
    public int AnswerId => Model.Id;

    public int Count => Model.Count;

    public bool HasOwnVote => Model.MeVoted;
}
