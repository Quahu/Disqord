using Disqord.Models;
using Qommon;

namespace Disqord.Rest
{
    public class TransientRestUser : TransientUser, IRestUser
    {
        /// <inheritdoc/>
        public virtual string BannerHash => Model.Banner.GetValueOrDefault();

        /// <inheritdoc/>
        public virtual Color? AccentColor => Model.AccentColor.HasValue
            ? Model.AccentColor.Value
            : null;

        public TransientRestUser(IClient client, UserJsonModel model)
            : base(client, model)
        { }
    }
}
