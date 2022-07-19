using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using ParsedMentionFlags = Disqord.ParsedMentions;

namespace Disqord;

/// <summary>
///     Represents the allowed mentions in a Discord message.<br/>
///     See <a href="https://discord.com/developers/docs/resources/channel#allowed-mentions-object">Discord documentation</a>.
/// </summary>
public class LocalAllowedMentions : ILocalConstruct<LocalAllowedMentions>, IJsonConvertible<AllowedMentionsJsonModel>
{
    /// <summary>
    ///     Creates an instance in which all mentions are ignored.
    /// </summary>
    public static LocalAllowedMentions None => new LocalAllowedMentions()
        .WithParsedMentions(ParsedMentionFlags.None);

    /// <summary>
    ///     Creates an instance in which all <see cref="Mention.Here"/> and <see cref="Mention.Everyone"/> mentions are ignored.
    /// </summary>
    public static LocalAllowedMentions ExceptEveryone => new LocalAllowedMentions()
        .WithParsedMentions(ParsedMentionFlags.Users | ParsedMentionFlags.Roles);

    /// <summary>
    ///     Gets or sets the mention types Discord will parse from the message's content.
    /// </summary>
    public Optional<ParsedMentionFlags> ParsedMentions { get; set; }

    /// <summary>
    ///     Gets or sets the IDs of the users that can be mentioned.
    /// </summary>
    public Optional<IList<Snowflake>> UserIds { get; set; }

    /// <summary>
    ///     Gets or sets the IDs of the roles that can be mentioned.
    /// </summary>
    public Optional<IList<Snowflake>> RoleIds { get; set; }

    /// <summary>
    ///     Gets or sets whether the author of the replied to message is going to be mentioned.
    /// </summary>
    public Optional<bool> MentionRepliedToUser { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalAllowedMentions"/>.
    /// </summary>
    public LocalAllowedMentions()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalAllowedMentions"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalAllowedMentions(LocalAllowedMentions other)
    {
        ParsedMentions = other.ParsedMentions;
        UserIds = other.UserIds.Clone();
        RoleIds = other.RoleIds.Clone();
        MentionRepliedToUser = other.MentionRepliedToUser;
    }

    /// <inheritdoc/>
    public virtual LocalAllowedMentions Clone()
    {
        return new(this);
    }

    /// <inheritdoc />
    public AllowedMentionsJsonModel ToModel()
    {
        if (ParsedMentions.TryGetValue(out var parsedMentions) && parsedMentions != ParsedMentionFlags.None)
        {
            if (parsedMentions.HasFlag(ParsedMentionFlags.Users) && UserIds.HasValue && UserIds.Value.Count != 0)
                throw new InvalidOperationException("Parsed mentions and IDs are mutually exclusive, meaning you must not set both the parsed mentions for users and user IDs.");

            if (parsedMentions.HasFlag(ParsedMentionFlags.Roles) && RoleIds.HasValue && RoleIds.Value.Count != 0)
                throw new InvalidOperationException("Parsed mentions and IDs are mutually exclusive, meaning you must not set both the parsed mentions for roles and role IDs.");
        }

        const int MaxMentionAmount = Discord.Limits.Message.AllowedMentions.MaxMentionAmount;
        if (UserIds.HasValue && UserIds.Value.Count > MaxMentionAmount)
            throw new InvalidOperationException($"The amount of user mentions must not exceed {MaxMentionAmount} mentions.");

        if (RoleIds.HasValue && RoleIds.Value.Count > MaxMentionAmount)
            throw new InvalidOperationException($"The amount of role mentions must not exceed {MaxMentionAmount} mentions.");

        var model = new AllowedMentionsJsonModel
        {
            Users = UserIds.ToArray(),
            Roles = RoleIds.ToArray()
        };

        if (ParsedMentions.TryGetValue(out parsedMentions))
        {
            if (parsedMentions == ParsedMentionFlags.None)
            {
                model.Parse = Array.Empty<string>();
            }
            else
            {
                var parse = new List<string>(3);
                if (parsedMentions.HasFlag(ParsedMentionFlags.Everyone))
                    parse.Add("everyone");

                if (parsedMentions.HasFlag(ParsedMentionFlags.Users))
                    parse.Add("users");

                if (parsedMentions.HasFlag(ParsedMentionFlags.Roles))
                    parse.Add("roles");

                model.Parse = parse;
            }
        }

        model.RepliedUser = MentionRepliedToUser;

        return model;
    }
}
