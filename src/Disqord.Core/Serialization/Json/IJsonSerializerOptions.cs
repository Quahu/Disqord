namespace Disqord.Serialization.Json;

/// <summary>
///     Represents options used to customize JSON serialization.
/// </summary>
public interface IJsonSerializerOptions
{
    /// <summary>
    ///     Gets or sets the desired formatting of the output JSON.
    /// </summary>
    JsonFormatting Formatting { get; set; }
}