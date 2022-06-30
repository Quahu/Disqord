using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Components;

/// <summary>
///     Marks the decorated method as a component command.
/// </summary>
public abstract class ComponentCommandAttribute : CommandAttribute
{
    /// <summary>
    ///     Gets or sets the custom ID pattern of this command.
    /// </summary>
    /// <remarks>
    ///     If <see cref="IsRegexPattern"/> is <see langword="false"/> the pattern is "primitive", i.e.
    ///     it can have 0 or more "holes", which are marked using asterisks (<c>*</c>)
    ///     and are separated from text and other holes using colons (<c>:</c>).
    ///     <br/>
    ///     Examples of the format:<br/>
    ///     • <c>MyButton</c><br/>
    ///     • <c>LoremIpsum:*:DolorSitAmet:*</c>
    ///     <para/>
    ///     If <see cref="IsRegexPattern"/> is <see langword="true"/> the pattern is a regular expression (<see cref="System.Text.RegularExpressions.Regex"/>).
    ///     Due to the inferior performance primitive patterns should be used as often as possible instead.
    ///     <para/>
    ///     Values matched from the custom ID via the pattern are passed as parameters to the command.
    ///     These parameters must appear first in the command.
    /// </remarks>
    public string Pattern { get; }

    /// <summary>
    ///     Gets or sets whether the pattern is a regular expression pattern.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see langword="false"/>.
    /// </remarks>
    public bool IsRegexPattern { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="ComponentCommandAttribute"/> with the specified pattern.
    /// </summary>
    /// <param name="pattern"> The pattern of the command. </param>
    /// <remarks>
    ///     <inheritdoc cref="Pattern"/>
    /// </remarks>
    protected ComponentCommandAttribute(string pattern)
    {
        Pattern = pattern;
    }

    /// <summary>
    ///     Gets the <see cref="ComponentType"/> of this attribute.
    /// </summary>
    /// <returns></returns>
    protected abstract ComponentCommandType GetCommandType();

    /// <inheritdoc/>
    public override void Apply(ICommandBuilder builder)
    {
        var componentBuilder = Guard.IsAssignableToType<ComponentCommandBuilder>(builder);
        componentBuilder.Pattern = Pattern;
        componentBuilder.IsRegexPattern = IsRegexPattern;
        componentBuilder.Type = GetCommandType();
    }
}
