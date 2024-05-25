using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientPoll(PollJsonModel model) : TransientEntity<PollJsonModel>(model), IPoll
{
    public IPollMedia Question => _question ??= new TransientPollMedia(Model.Question);

    private IPollMedia? _question;

    public IReadOnlyList<IPollAnswer> Answers => _answers ??= Model.Answers.ToReadOnlyList(answer => new TransientPollAnswer(answer));

    private IReadOnlyList<IPollAnswer>? _answers;

    public DateTimeOffset? Expiry => Model.Expiry.GetValueOrDefault();

    public bool AllowMultiselect => Model.AllowMultiselect;

    public PollLayoutType LayoutType => Model.LayoutType;

    public IPollResults? Results => _results ??= Optional.ConvertOrDefault(Model.Results, results => new TransientPollResults(results));

    private IPollResults? _results;
}
