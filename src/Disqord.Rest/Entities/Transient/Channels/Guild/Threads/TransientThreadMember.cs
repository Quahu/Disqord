using Disqord.Models;

namespace Disqord.Rest;

public class TransientRestThreadMember : TransientThreadMember, IRestThreadMember
{
    /// <inheritdoc/>
    public IMember? Member
    {
        get
        {
            if (!Model.Member.HasValue)
                return null;

            return _member ??= new TransientMember(Client, 0, Model.Member.Value);
        }
    }
    private IMember? _member;

    /// <inheritdoc/>
    public TransientRestThreadMember(IClient client, ThreadMemberJsonModel model)
        : base(client, model)
    { }
}
