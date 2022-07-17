using System;
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord.OAuth2;

/// <inheritdoc cref="IBearerAuthorization"/>
public class TransientBearerAuthorization : TransientClientEntity<AuthorizationJsonModel>, IBearerAuthorization
{
    /// <inheritdoc/>
    public IApplication Application => _application ??= new TransientApplication(Client, Model.Application);

    private IApplication? _application;

    /// <inheritdoc/>
    public IReadOnlyList<string> Scopes => Model.Scopes;

    /// <inheritdoc/>
    public DateTimeOffset ExpiresAt => Model.Expires;

    /// <inheritdoc/>
    public IUser? User
    {
        get
        {
            if (!Model.User.HasValue)
                return null;

            return _user ??= new TransientUser(Client, Model.User.Value);
        }
    }
    private IUser? _user;

    public TransientBearerAuthorization(IClient client, AuthorizationJsonModel model)
        : base(client, model)
    { }
}