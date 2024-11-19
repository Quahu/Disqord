using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Disqord.Serialization.Json;
using Microsoft.Extensions.Logging;

namespace Disqord.Models;

internal static class JsonArrayExtensions
{
    public static IEnumerable<TJsonModel> SafelyDeserializeItems<TJsonModel>(this IJsonArray jsonArray, ILogger logger, [CallerArgumentExpression(nameof(jsonArray))] string? caller = null)
        where TJsonModel : JsonModel
    {
        var count = jsonArray.Count;
        for (var i = 0; i < count; i++)
        {
            TJsonModel? model = null;
            try
            {
                model = jsonArray[i]?.ToType<TJsonModel>();
            }
            catch (Exception ex)
            {
                if (Library.Debug.LogSafeDeserializationExceptions)
                {
                    logger.LogWarning(ex, "Failed to deserialize {Caller}.", caller);
                }
            }

            if (model == null)
            {
                continue;
            }

            yield return model;
        }
    }
}
