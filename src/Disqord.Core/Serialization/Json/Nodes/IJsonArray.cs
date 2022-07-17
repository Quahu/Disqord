using System.Collections.Generic;

namespace Disqord.Serialization.Json;

/// <summary>
///     Represents a JSON array node, i.e. an array of <see cref="IJsonNode"/>s.
/// </summary>
public interface IJsonArray : IJsonNode, IReadOnlyList<IJsonNode?>
{ }