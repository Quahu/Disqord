using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientMessageApplication : TransientEntity<MessageApplicationJsonModel>, IMessageApplication
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public string CoverImageHash => Model.CoverImage.GetValueOrDefault();

        /// <inheritdoc/>
        public string Description => Model.Description;

        /// <inheritdoc/>
        public string IconHash => Model.Icon;

        public TransientMessageApplication(MessageApplicationJsonModel model)
            : base(model)
        { }

        public override string ToString()
            => $"'{Name}' ({Id})";
    }
}
