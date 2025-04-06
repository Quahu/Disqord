using Qommon;

namespace Disqord;

public class ModifyForumChannelActionProperties : ModifyMediaChannelActionProperties
{
    public Optional<ForumLayout> DefaultLayout { internal get; set; }

    internal ModifyForumChannelActionProperties()
    { }
}
