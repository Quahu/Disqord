using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientFileComponent(IClient client, FileComponentJsonModel model)
    : TransientBaseComponent<FileComponentJsonModel>(client, model), IFileComponent
{
    [field: MaybeNull]
    public IUnfurledMediaItem File => field ??= new TransientUnfurledMediaItem(Model.File);

    public bool IsSpoiler => Model.Spoiler.GetValueOrDefault();
}
