using System;
using System.Collections.Generic;
using System.ComponentModel;
using Qommon;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalEmbedExtensions
{
    public static TEmbed WithTitle<TEmbed>(this TEmbed embed, string title)
        where TEmbed : LocalEmbed
    {
        embed.Title = title;
        return embed;
    }

    public static TEmbed WithDescription<TEmbed>(this TEmbed embed, string description)
        where TEmbed : LocalEmbed
    {
        embed.Description = description;
        return embed;
    }

    public static TEmbed WithUrl<TEmbed>(this TEmbed embed, string url)
        where TEmbed : LocalEmbed
    {
        embed.Url = url;
        return embed;
    }

    public static TEmbed WithImageUrl<TEmbed>(this TEmbed embed, string imageUrl)
        where TEmbed : LocalEmbed
    {
        embed.ImageUrl = imageUrl;
        return embed;
    }

    public static TEmbed WithThumbnailUrl<TEmbed>(this TEmbed embed, string thumbnailUrl)
        where TEmbed : LocalEmbed
    {
        embed.ThumbnailUrl = thumbnailUrl;
        return embed;
    }

    public static TEmbed WithTimestamp<TEmbed>(this TEmbed embed, DateTimeOffset timestamp)
        where TEmbed : LocalEmbed
    {
        embed.Timestamp = timestamp;
        return embed;
    }

    public static TEmbed WithColor<TEmbed>(this TEmbed embed, Color color)
        where TEmbed : LocalEmbed
    {
        embed.Color = color;
        return embed;
    }

    public static TEmbed WithFooter<TEmbed>(this TEmbed embed, string text, string? iconUrl = null)
        where TEmbed : LocalEmbed
    {
        var footer = embed.Footer.GetValueOrDefault();
        if (footer == null)
        {
            embed.Footer = footer = new LocalEmbedFooter();
        }

        footer.Text = text;
        footer.IconUrl = Optional.FromNullable(iconUrl);
        return embed;
    }

    public static TEmbed WithFooter<TEmbed>(this TEmbed embed, LocalEmbedFooter footer)
        where TEmbed : LocalEmbed
    {
        embed.Footer = footer;
        return embed;
    }

    public static TEmbed WithAuthor<TEmbed>(this TEmbed embed, IUser user)
        where TEmbed : LocalEmbed
    {
        Guard.IsNotNull(user);

        return embed.WithAuthor(user.Tag, user.GetAvatarUrl(size: 128));
    }

    public static TEmbed WithAuthor<TEmbed>(this TEmbed embed, string name, string? iconUrl = null, string? url = null)
        where TEmbed : LocalEmbed
    {
        var author = embed.Author.GetValueOrDefault();
        if (author == null)
        {
            embed.Author = author = new LocalEmbedAuthor();
        }

        author.Name = name;
        author.IconUrl = Optional.FromNullable(iconUrl);
        author.Url = Optional.FromNullable(url);
        embed.Author = author;
        return embed;
    }

    public static TEmbed WithAuthor<TEmbed>(this TEmbed embed, LocalEmbedAuthor author)
        where TEmbed : LocalEmbed
    {
        embed.Author = author;
        return embed;
    }

    public static TEmbed AddBlankField<TEmbed>(this TEmbed embed, bool isInline = false)
        where TEmbed : LocalEmbed
    {
        return embed.AddField("\u200b", "\u200b", isInline);
    }

    public static TEmbed AddField<TEmbed>(this TEmbed embed, string name, object value, bool isInline = false)
        where TEmbed : LocalEmbed
    {
        var field = new LocalEmbedField()
            .WithName(name)
            .WithValue(value)
            .WithIsInline(isInline);

        return embed.AddField(field);
    }

    public static TEmbed AddField<TEmbed>(this TEmbed embed, string name, string value, bool isInline = false)
        where TEmbed : LocalEmbed
    {
        var field = new LocalEmbedField()
            .WithName(name)
            .WithValue(value)
            .WithIsInline(isInline);

        return embed.AddField(field);
    }

    public static TEmbed AddField<TEmbed>(this TEmbed embed, LocalEmbedField field)
        where TEmbed : LocalEmbed
    {
        if (embed.Fields.Add(field, out var list))
            embed.Fields = new(list);

        return embed;
    }

    public static TEmbed WithFields<TEmbed>(this TEmbed embed, IEnumerable<LocalEmbedField> fields)
        where TEmbed : LocalEmbed
    {
        Guard.IsNotNull(fields);

        if (embed.Fields.With(fields, out var list))
            embed.Fields = new(list);

        return embed;
    }

    public static TEmbed WithFields<TEmbed>(this TEmbed embed, params LocalEmbedField[] fields)
        where TEmbed : LocalEmbed
    {
        return embed.WithFields(fields as IEnumerable<LocalEmbedField>);
    }
}
