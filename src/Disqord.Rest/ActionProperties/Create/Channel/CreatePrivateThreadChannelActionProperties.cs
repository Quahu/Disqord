using Qommon;

namespace Disqord;

public sealed class CreatePrivateThreadChannelActionProperties : CreateThreadChannelActionProperties
{
    public Optional<bool> AllowsInvitation { internal get; set; }

    internal CreatePrivateThreadChannelActionProperties()
    { }
}