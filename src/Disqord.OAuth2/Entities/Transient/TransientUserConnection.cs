using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord.OAuth2
{
    /// <inheritdoc cref="IUserConnection"/>
    public class TransientUserConnection : TransientClientEntity<ConnectionJsonModel>, IUserConnection
    {
        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public string Id => Model.Id;

        /// <inheritdoc/>
        public string Type => Model.Type;

        /// <inheritdoc/>
        public bool IsRevoked => Model.Revoked.GetValueOrDefault();

        /// <inheritdoc/>
        public IReadOnlyList<IIntegration> Integrations
        {
            get
            {
                if (!Model.Integrations.HasValue)
                    return ReadOnlyList<IIntegration>.Empty;

                return _integrations ??= Model.Integrations.Value.ToReadOnlyList(Client, (model, client) => new TransientIntegration(client, 0, model));
            }
        }
        private IReadOnlyList<TransientIntegration> _integrations;

        /// <inheritdoc/>
        public bool IsVerified => Model.Verified;

        /// <inheritdoc/>
        public bool HasFriendSync => Model.FriendSync;

        /// <inheritdoc/>
        public bool ShowsActivity => Model.ShowActivity;

        /// <inheritdoc/>
        public UserConnectionVisibility Visibility => Model.Visibility;

        public TransientUserConnection(IClient client, ConnectionJsonModel model)
            : base(client, model)
        { }
    }
}
