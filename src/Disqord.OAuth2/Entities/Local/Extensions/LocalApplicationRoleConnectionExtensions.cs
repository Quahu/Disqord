using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Qommon;

namespace Disqord.OAuth2;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalApplicationRoleConnectionExtensions
{
    public static TApplicationRoleConnection WithPlatformName<TApplicationRoleConnection>(this TApplicationRoleConnection connection, string platformName)
        where TApplicationRoleConnection : LocalApplicationRoleConnection
    {
        connection.PlatformName = platformName;
        return connection;
    }

    public static TApplicationRoleConnection WithPlatformUsername<TApplicationRoleConnection>(this TApplicationRoleConnection connection, string platformUsername)
        where TApplicationRoleConnection : LocalApplicationRoleConnection
    {
        connection.PlatformUsername = platformUsername;
        return connection;
    }

    public static TApplicationRoleConnection AddMetadata<TApplicationRoleConnection>(this TApplicationRoleConnection connection, string key, long value)
        where TApplicationRoleConnection : LocalApplicationRoleConnection
    {
        return connection.AddMetadata(key, value.ToString(CultureInfo.InvariantCulture));
    }

    public static TApplicationRoleConnection AddMetadata<TApplicationRoleConnection>(this TApplicationRoleConnection connection, string key, DateTimeOffset value)
        where TApplicationRoleConnection : LocalApplicationRoleConnection
    {
        return connection.AddMetadata(key, value.ToString("O"));
    }

    public static TApplicationRoleConnection AddMetadata<TApplicationRoleConnection>(this TApplicationRoleConnection connection, string key, bool value)
        where TApplicationRoleConnection : LocalApplicationRoleConnection
    {
        return connection.AddMetadata(key, value ? "1" : "0");
    }

    public static TApplicationRoleConnection AddMetadata<TApplicationRoleConnection>(this TApplicationRoleConnection connection, string key, string value)
        where TApplicationRoleConnection : LocalApplicationRoleConnection
    {
        Guard.IsNotNull(key);
        Guard.IsNotNull(value);

        if (connection.Metadata.Add(key, value, out var dictionary))
            connection.Metadata = new(dictionary);

        return connection;
    }

    public static TApplicationRoleConnection WithMetadata<TApplicationRoleConnection>(this TApplicationRoleConnection connection, IEnumerable<KeyValuePair<string, string>> metadata)
        where TApplicationRoleConnection : LocalApplicationRoleConnection
    {
        Guard.IsNotNull(metadata);

        if (connection.Metadata.With(metadata, out var dictionary))
            connection.Metadata = new(dictionary);

        return connection;
    }
}
