using Disqord.Models;

namespace Disqord;

public class TransientPollAnswer(PollAnswerJsonModel model) : TransientEntity<PollAnswerJsonModel>(model), IPollAnswer
{
    public int Id => Model.AnswerId.Value;

    public IPollMedia Media => _media ??= new TransientPollMedia(Model.PollMedia);

    private IPollMedia? _media;
}
