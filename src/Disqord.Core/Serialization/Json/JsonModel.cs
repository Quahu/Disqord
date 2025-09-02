using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Disqord.Serialization.Json;

/// <summary>
///     Represents a JSON model.
/// </summary>
public class JsonModel : IJsonNode
{
    /// <summary>
    ///     The runtime cache for extension data dictionaries.
    /// </summary>
    public static readonly ConditionalWeakTable<JsonModel, IDictionary<string, IJsonNode?>> ExtensionDataCache = new();

    /// <summary>
    ///     Gets the extension data dictionary.
    /// </summary>
    public IDictionary<string, IJsonNode?> ExtensionData
    {
        get => ExtensionDataCache.GetValue(this, _ => new Dictionary<string, IJsonNode?>());
        set => ExtensionDataCache.AddOrUpdate(this, value);
    }

    /// <summary>
    ///     Gets or sets an <see cref="IJsonNode"/> as extension data with the given key.
    /// </summary>
    /// <param name="key"> The extension data key. </param>
    public IJsonNode? this[string key]
    {
        get => ExtensionDataCache.TryGetValue(this, out var extensionData) ? extensionData[key] : null;
        set => ExtensionData[key] = value;
    }

    /// <summary>
    ///     Instantiates a new <see cref="JsonModel"/>.
    /// </summary>
    public JsonModel()
    { }

    /// <inheritdoc cref="Validate"/>
    /// <remarks>
    ///     This is called by <see cref="Validate"/>.
    /// </remarks>
    protected virtual void OnValidate()
    { }

    /// <summary>
    ///     Ensures that this JSON model is valid to be sent to the Discord API.
    /// </summary>
    /// <remarks>
    ///     This method is used internally.
    ///     This is a virtual method that by default does nothing.
    ///     Validation will not exist for all possible models.
    /// </remarks>
    public void Validate()
    {
        try
        {
            OnValidate();
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Failed to validate model {GetType().Name}.", ex);
        }
    }

    /// <summary>
    ///     Attempts to get the extension data dictionary of the model without allocating it.
    /// </summary>
    /// <param name="jsonModel"> The model to get the extension data for. </param>
    /// <param name="extensionData"> The extension data dictionary. </param>
    /// <returns>
    ///     <see langword="true"/> if the model has extension data.
    /// </returns>
    public static bool TryGetExtensionData(JsonModel jsonModel, [MaybeNullWhen(false)] out IDictionary<string, IJsonNode?> extensionData)
    {
        return ExtensionDataCache.TryGetValue(jsonModel, out extensionData);
    }

    /// <summary>
    ///     Attempts to get the extension datum of the model with the specified name.
    /// </summary>
    /// <param name="jsonModel"> The model to get the extension datum for. </param>
    /// <param name="name"> The JSON property name. </param>
    /// <param name="extensionDatum"> The extension datum. </param>
    /// <returns>
    ///     <see langword="true"/> if the model has extension datum.
    /// </returns>
    public static bool TryGetExtensionDatum<T>(JsonModel jsonModel, string name, out T? extensionDatum)
    {
        if (TryGetExtensionData(jsonModel, out var extensionData) && extensionData.TryGetValue(name, out var extensionDatumNode))
        {
            extensionDatum = extensionDatumNode != null
                ? extensionDatumNode.ToType<T>()
                : default;

            return true;
        }

        extensionDatum = default;
        return false;
    }

    string IJsonNode.Path => "$";

    JsonValueType IJsonNode.Type => JsonValueType.Object;

    T IJsonNode.ToType<T>()
    {
        throw new NotSupportedException();
    }

    string IJsonNode.ToJsonString(JsonFormatting formatting)
    {
        throw new NotSupportedException();
    }
}
