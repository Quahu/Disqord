using System.Collections.Generic;
using System.ComponentModel;
using Qommon;

namespace Disqord;

/// <summary>
///     Defines <see cref="LocalAllowedMentions"/> extensions.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalAllowedMentionsExtensions
{
    /// <summary>
    ///     Sets the mention types Discord will parse from the message's content.
    /// </summary>
    /// <param name="allowedMentions"> The <see cref="LocalAllowedMentions"/> instance. </param>
    /// <param name="parsedMentions"> The parsed mentions. </param>
    /// <typeparam name="TAllowedMentions"> The <see cref="LocalAllowedMentions"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    public static TAllowedMentions WithParsedMentions<TAllowedMentions>(this TAllowedMentions allowedMentions, ParsedMentions parsedMentions)
        where TAllowedMentions : LocalAllowedMentions
    {
        allowedMentions.ParsedMentions = parsedMentions;
        return allowedMentions;
    }

    /// <summary>
    ///     Add an ID of the user that can be mentioned.
    /// </summary>
    /// <param name="allowedMentions"> The <see cref="LocalAllowedMentions"/> instance. </param>
    /// <param name="userId"> The ID of the user. </param>
    /// <typeparam name="TAllowedMentions"> The <see cref="LocalAllowedMentions"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    public static TAllowedMentions AddUserId<TAllowedMentions>(this TAllowedMentions allowedMentions, Snowflake userId)
        where TAllowedMentions : LocalAllowedMentions
    {
        if (allowedMentions.UserIds.Add(userId, out var list))
            allowedMentions.UserIds = new(list);

        return allowedMentions;
    }

    /// <summary>
    ///     Sets the IDs of the users that can be mentioned.
    /// </summary>
    /// <param name="allowedMentions"> The <see cref="LocalAllowedMentions"/> instance. </param>
    /// <param name="userIds"> The IDs of the users. </param>
    /// <typeparam name="TAllowedMentions"> The <see cref="LocalAllowedMentions"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    public static TAllowedMentions WithUserIds<TAllowedMentions>(this TAllowedMentions allowedMentions, IEnumerable<Snowflake> userIds)
        where TAllowedMentions : LocalAllowedMentions
    {
        Guard.IsNotNull(userIds);

        if (allowedMentions.UserIds.With(userIds, out var list))
            allowedMentions.UserIds = new(list);

        return allowedMentions;
    }

    /// <summary>
    ///     Sets the IDs of the users that can be mentioned.
    /// </summary>
    /// <param name="allowedMentions"> The <see cref="LocalAllowedMentions"/> instance. </param>
    /// <param name="userIds"> The IDs of the users. </param>
    /// <typeparam name="TAllowedMentions"> The <see cref="LocalAllowedMentions"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    public static TAllowedMentions WithUserIds<TAllowedMentions>(this TAllowedMentions allowedMentions, params Snowflake[] userIds)
        where TAllowedMentions : LocalAllowedMentions
    {
        return allowedMentions.WithUserIds(userIds as IEnumerable<Snowflake>);
    }

    /// <summary>
    ///     Add an ID of the role that can be mentioned.
    /// </summary>
    /// <param name="allowedMentions"> The <see cref="LocalAllowedMentions"/> instance. </param>
    /// <param name="roleId"> The ID of the role. </param>
    /// <typeparam name="TAllowedMentions"> The <see cref="LocalAllowedMentions"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    public static TAllowedMentions AddRoleId<TAllowedMentions>(this TAllowedMentions allowedMentions, Snowflake roleId)
        where TAllowedMentions : LocalAllowedMentions
    {
        if (allowedMentions.RoleIds.Add(roleId, out var list))
            allowedMentions.RoleIds = new(list);

        return allowedMentions;
    }

    /// <summary>
    ///     Sets the IDs of the roles that can be mentioned.
    /// </summary>
    /// <param name="allowedMentions"> The <see cref="LocalAllowedMentions"/> instance. </param>
    /// <param name="roleIds"> The IDs of the roles. </param>
    /// <typeparam name="TAllowedMentions"> The <see cref="LocalAllowedMentions"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    public static TAllowedMentions WithRoleIds<TAllowedMentions>(this TAllowedMentions allowedMentions, IEnumerable<Snowflake> roleIds)
        where TAllowedMentions : LocalAllowedMentions
    {
        Guard.IsNotNull(roleIds);

        if (allowedMentions.RoleIds.With(roleIds, out var list))
            allowedMentions.RoleIds = new(list);

        return allowedMentions;
    }

    /// <summary>
    ///     Sets the IDs of the roles that can be mentioned.
    /// </summary>
    /// <param name="allowedMentions"> The <see cref="LocalAllowedMentions"/> instance. </param>
    /// <param name="roleIds"> The IDs of the roles. </param>
    /// <typeparam name="TAllowedMentions"> The <see cref="LocalAllowedMentions"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    public static TAllowedMentions WithRoleIds<TAllowedMentions>(this TAllowedMentions allowedMentions, params Snowflake[] roleIds)
        where TAllowedMentions : LocalAllowedMentions
    {
        return allowedMentions.WithRoleIds(roleIds as IEnumerable<Snowflake>);
    }

    /// <summary>
    ///     Sets whether the author of the replied to message is going to be mentioned.
    /// </summary>
    /// <param name="allowedMentions"> The <see cref="LocalAllowedMentions"/> instance. </param>
    /// <param name="mentionRepliedToUser"> Whether the author of the replied to message should be mentioned. </param>
    /// <typeparam name="TAllowedMentions"> The <see cref="LocalAllowedMentions"/> type. </typeparam>
    /// <returns>
    ///     The input instance.
    /// </returns>
    public static TAllowedMentions WithMentionRepliedToUser<TAllowedMentions>(this TAllowedMentions allowedMentions, bool mentionRepliedToUser = true)
        where TAllowedMentions : LocalAllowedMentions
    {
        allowedMentions.MentionRepliedToUser = mentionRepliedToUser;
        return allowedMentions;
    }
}
