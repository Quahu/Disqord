using System;
using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway;

public class TransientRichActivityParty : TransientEntity<ActivityPartyJsonModel>, IRichActivityParty
{
    /// <inheritdoc/>
    public string? Id => Model.Id.GetValueOrDefault();

    /// <inheritdoc/>
    public int? Size => ParseInt(Model.Size.GetValueOrDefault(), 0);

    /// <inheritdoc/>
    public int? MaximumSize => ParseInt(Model.Size.GetValueOrDefault(), 1);

    public TransientRichActivityParty(ActivityPartyJsonModel model)
        : base(model)
    { }

    private static int? ParseInt(string[]? array, int index)
    {
        if (array == null || index >= array.Length)
            return null;

        var stringValue = array[index];
        if (string.IsNullOrEmpty(stringValue))
            return null;

        // Try to parse as long first to handle overflow cases
        if (long.TryParse(stringValue, out var longValue))
        {
            // Clamp to int range
            if (longValue > int.MaxValue)
                return int.MaxValue;
            if (longValue < int.MinValue)
                return int.MinValue;
            return (int)longValue;
        }

        // Try to parse as double for floating point values
        if (double.TryParse(stringValue, out var doubleValue))
        {
            // Clamp to int range
            if (doubleValue > int.MaxValue)
                return int.MaxValue;
            if (doubleValue < int.MinValue)
                return int.MinValue;
            return (int)doubleValue;
        }

        // If parsing fails, return null instead of throwing
        return null;
    }
}
