using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientModalFileUploadComponent(ModalFileUploadComponentJsonModel model)
    : TransientModalComponent<ModalFileUploadComponentJsonModel>(model), IModalFileUploadComponent
{
    public string CustomId => Model.CustomId;

    [field: MaybeNull]
    public IReadOnlyList<Snowflake> AttachmentIds => field ??= Model.Values.ToReadOnlyList(Snowflake.Parse);
}
