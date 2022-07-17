using System.Collections.Generic;

namespace Disqord.Serialization.Json;

/// <summary>
///     Represents a JSON object node, i.e. a dictionary of <see cref="IJsonNode"/>s keyed by the property names.
/// </summary>
public interface IJsonObject : IJsonNode, IReadOnlyDictionary<string, IJsonNode?>
{ }