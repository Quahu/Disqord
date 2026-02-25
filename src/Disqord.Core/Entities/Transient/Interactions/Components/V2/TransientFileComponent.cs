using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientFileComponent(FileComponentJsonModel model)
    : TransientBaseComponent<FileComponentJsonModel>(model), IFileComponent
{
    [field: MaybeNull]
    public IUnfurledMediaItem File => field ??= TransientUnfurledMediaItem.Create(Model.File);

    public bool IsSpoiler => Model.Spoiler.GetValueOrDefault();
}
