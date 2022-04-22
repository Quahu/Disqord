using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientRoleTags : TransientEntity<RoleTagsJsonModel>, IRoleTags
    {
        /// <inheritdoc/>
        public Snowflake? BotId => Model.BotId.GetValueOrNullable();

        /// <inheritdoc/>
        public Snowflake? IntegrationId => Model.IntegrationId.GetValueOrNullable();

        /// <inheritdoc/>
        public bool IsNitroBooster => Model.PremiumSubscriber.HasValue;

        public TransientRoleTags(RoleTagsJsonModel model)
            : base(model)
        { }
    }
}
