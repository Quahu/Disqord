using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway
{
    public class TransientRichActivityParty : TransientEntity<ActivityPartyJsonModel>, IRichActivityParty
    {
        /// <inheritdoc/>
        public string Id => Model.Id.GetValueOrDefault();

        /// <inheritdoc/>
        public int? Size => Model.Size.GetValueOrDefault()?[0];

        /// <inheritdoc/>
        public int? MaximumSize => Model.Size.GetValueOrDefault()?[1];

        public TransientRichActivityParty(ActivityPartyJsonModel model)
            : base(model)
        { }
    }
}
