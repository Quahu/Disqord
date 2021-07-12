using Disqord.Models;

namespace Disqord
{
    public class TransientVanityInvite : TransientEntity<InviteJsonModel>, IVanityInvite
    {
        public string Code => Model.Code;
        public int Uses => Model.Uses.Value;
        
        public TransientVanityInvite(IClient client, InviteJsonModel model) 
            : base(client, model)
        { }
    }
}