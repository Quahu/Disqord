using System;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Restricts the <see cref="IAttachment"/> application command parameter to given file types.
/// </summary>
/// <remarks>
///     <b>Do not use this for text commands.</b>
///     <br/>
///     For application commands this attribute is turned into API-side validation.
///     <br/>
///     The supported values are <c>image</c>, <c>video</c>, <c>audio</c>,
///     or any dot-prefixed file extension such as <c>.pdf</c>.
///     Discord only validates the extension of the file name, not its contents.
/// </remarks>
[AttributeUsage(AttributeTargets.Parameter)]
public class FileTypesAttribute : Attribute
{
    /// <summary>
    ///     Gets the file types of this attribute.
    /// </summary>
    public string[] FileTypes { get; }

    /// <summary>
    ///     Instantiates a new <see cref="FileTypesAttribute"/> with the specified file types.
    /// </summary>
    /// <param name="fileTypes"> The file types to restrict the option to. </param>
    public FileTypesAttribute(params string[] fileTypes)
    {
        FileTypes = fileTypes;
    }
}
