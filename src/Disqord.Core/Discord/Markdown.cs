using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Qommon.Collections.ReadOnly;

namespace Disqord;

/// <summary>
///     Represents utility methods for Discord's Markdown syntax.
/// </summary>
public static class Markdown
{
    /// <summary>
    ///     The set containing the markdown characters that get escaped by <see cref="Escape(string)"/>.
    /// </summary>
    public static readonly IReadOnlySet<char> EscapedCharacters = new HashSet<char>(5)
    {
        '\\',
        '*',
        '`',
        '_',
        '~'
    }.ReadOnly();

    // *text*
    public static string Italics(object? value)
    {
        return Italics((value?.ToString()).AsSpan());
    }

    public static string Italics(string? text)
    {
        return Italics(text.AsSpan());
    }

    public static string Italics(ReadOnlySpan<char> text)
    {
        return $"*{text}*";
    }

    // **text**
    public static string Bold(object? value)
    {
        return Bold((value?.ToString()).AsSpan());
    }

    public static string Bold(string? text)
    {
        return Bold(text.AsSpan());
    }

    public static string Bold(ReadOnlySpan<char> text)
    {
        return $"**{text}**";
    }

    // ***text***
    public static string BoldItalics(object? value)
    {
        return BoldItalics((value?.ToString()).AsSpan());
    }

    public static string BoldItalics(string? text)
    {
        return BoldItalics(text.AsSpan());
    }

    public static string BoldItalics(ReadOnlySpan<char> text)
    {
        return $"***{text}***";
    }

    // __text__
    public static string Underline(object? value)
    {
        return Underline((value?.ToString()).AsSpan());
    }

    public static string Underline(string? text)
    {
        return Underline(text.AsSpan());
    }

    public static string Underline(ReadOnlySpan<char> text)
    {
        return $"__{text}__";
    }

    // ~~text~~
    public static string Strikethrough(object? value)
    {
        return Strikethrough((value?.ToString()).AsSpan());
    }

    public static string Strikethrough(string? text)
    {
        return Strikethrough(text.AsSpan());
    }

    public static string Strikethrough(ReadOnlySpan<char> text)
    {
        return $"~~{text}~~";
    }

    // [title](url)
    public static string Link(string? title, string? url)
    {
        return Link(title.AsSpan(), url.AsSpan());
    }

    public static string Link(ReadOnlySpan<char> title, ReadOnlySpan<char> url)
    {
        return $"[{title}]({url})";
    }

    // `code`
    public static string Code(object? value)
    {
        return Code((value?.ToString()).AsSpan());
    }

    public static string Code(string? code)
    {
        return Code(code.AsSpan());
    }

    public static string Code(ReadOnlySpan<char> code)
    {
        return $"`{code}`";
    }

    // ```\ncode```
    public static string CodeBlock(object? value)
    {
        return CodeBlock((value?.ToString()).AsSpan());
    }

    public static string CodeBlock(string? code)
    {
        return CodeBlock(code.AsSpan());
    }

    public static string CodeBlock(ReadOnlySpan<char> code)
    {
        return $"```\n{code}```";
    }

    // ```language\ncode```
    public static string CodeBlock(string? language, object? value)
    {
        return CodeBlock(language, value?.ToString());
    }

    public static string CodeBlock(string? language, string? code)
    {
        return CodeBlock(language.AsSpan(), code.AsSpan());
    }

    public static string CodeBlock(ReadOnlySpan<char> language, ReadOnlySpan<char> code)
    {
        return $"```{language}\n{code}```";
    }

    public static string Escape(string? text)
    {
        return Escape(text.AsSpan());
    }

    public static string Escape(ReadOnlySpan<char> text)
    {
        var stringHandler = text.Length <= 1024
            ? new DefaultInterpolatedStringHandler(0, 0, null, stackalloc char[text.Length + 11])
            : new DefaultInterpolatedStringHandler(text.Length, 1);

        for (var i = 0; i < text.Length; i++)
        {
            var character = text[i];
            if (EscapedCharacters.Contains(character))
                stringHandler.AppendLiteral("\\");

            stringHandler.AppendFormatted(character);
        }

        return stringHandler.ToStringAndClear();
    }

    public static string Timestamp(DateTimeOffset dateTimeOffset)
    {
        return Timestamp(dateTimeOffset.ToUnixTimeSeconds());
    }

    public static string Timestamp(long unixTimestamp)
    {
        return $"<t:{unixTimestamp}>";
    }

    public static string Timestamp(DateTimeOffset dateTimeOffset, TimestampFormat format)
    {
        return Timestamp(dateTimeOffset.ToUnixTimeSeconds(), format);
    }

    public static string Timestamp(long unixTimestamp, TimestampFormat format)
    {
        return $"<t:{unixTimestamp}:{(char) format}>";
    }

    public enum TimestampFormat
    {
        ShortTime = 't',

        LongTime = 'T',

        ShortDate = 'd',

        LongDate = 'D',

        ShortDateTime = 'f',

        LongDateTime = 'F',

        RelativeTime = 'R'
    }
}
