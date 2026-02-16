using System;
using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway;

public class TransientRichActivityParty : TransientEntity<ActivityPartyJsonModel>, IRichActivityParty
{
    /// <inheritdoc/>
    public string? Id => Model.Id.GetValueOrDefault();

    /// <inheritdoc/>
    public int? Size => ParseSizeInt(Model.Size.GetValueOrDefault(), 0);

    /// <inheritdoc/>
    public int? MaximumSize => ParseSizeInt(Model.Size.GetValueOrDefault(), 1);

    public TransientRichActivityParty(ActivityPartyJsonModel model)
        : base(model)
    { }

    private static int? ParseSizeInt(string[]? array, int index)
    {
        if (array == null || index >= array.Length)
            return null;

        var stringValue = array[index];
        if (string.IsNullOrEmpty(stringValue))
            return null;

        if (long.TryParse(stringValue, out var longValue))
        {
            if (longValue > int.MaxValue)
                return int.MaxValue;
            if (longValue < int.MinValue)
                return int.MinValue;
            return (int)longValue;
        }

        if (double.TryParse(stringValue, out var doubleValue))
        {
            if (doubleValue > int.MaxValue)
                return int.MaxValue;
            if (doubleValue < int.MinValue)
                return int.MinValue;
            return (int)doubleValue;
        }

        return null;
    }
}
