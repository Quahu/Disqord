using Disqord.Models;
using Qommon;

namespace Disqord
{
    /// <inheritdoc cref="IStoreApplication"/>
    public class TransientStoreApplication : TransientApplication, IStoreApplication
    {
        /// <inheritdoc/>
        public string Summary => Model.Summary;

        /// <inheritdoc/>
        public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

        /// <inheritdoc/>
        public Snowflake? PrimarySkuId => Model.PrimarySkuId.GetValueOrNullable();

        /// <inheritdoc/>
        public string Slug => Model.Slug.GetValueOrDefault();

        /// <inheritdoc/>
        public string CoverHash => Model.CoverImage.GetValueOrDefault();

        public TransientStoreApplication(IClient client, ApplicationJsonModel model)
            : base(client, model)
        { }
    }
}
