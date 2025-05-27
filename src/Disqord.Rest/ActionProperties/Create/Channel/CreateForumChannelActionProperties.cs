using Qommon;

namespace Disqord;

public sealed class CreateForumChannelActionProperties : CreateMediaChannelActionProperties
{
    public Optional<ForumLayout> DefaultLayout { internal get; set; }

    internal CreateForumChannelActionProperties()
    { }
}
