using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientPollResults(PollResultsJsonModel model) : TransientEntity<PollResultsJsonModel>(model), IPollResults
{
    public bool IsFinalized => Model.IsFinalized;

    public IReadOnlyList<IPollAnswerCount> AnswerCounts => _answerCounts ??= Model.AnswerCounts.ToReadOnlyList(count => new TransientPollAnswerCount(count));

    private IReadOnlyList<IPollAnswerCount>? _answerCounts;
}
