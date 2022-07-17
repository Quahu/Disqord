using System.ComponentModel;
using Disqord.Serialization.Json;

namespace Disqord.Models;

/// <summary>
///     Represents a local construct that is convertible to a JSON model.
/// </summary>
/// <typeparam name="TModel"> The type of the JSON model. </typeparam>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IJsonConvertible<out TModel>
    where TModel : JsonModel
{
    /// <summary>
    ///     Converts this local construct to the <typeparamref name="TModel"/> model.
    /// </summary>
    /// <returns>
    ///     The output JSON model.
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    TModel ToModel();
}
