using System;
using System.Collections.Generic;
using System.Globalization;
using Qommon;

namespace Disqord.Bot.Commands.Application.Default;

public partial class DefaultApplicationCommandCacheProvider
{
    protected static bool AreLocalizationsEquivalent(Optional<Dictionary<string, string>> localizations, Optional<IDictionary<CultureInfo, string>> otherLocalizations)
    {
        if (!localizations.HasValue && !otherLocalizations.HasValue)
            return true;

        if (localizations.HasValue != otherLocalizations.HasValue)
            return false;

        var modelLocalizations = localizations.Value;
        var localLocalizations = otherLocalizations.Value;
        if (modelLocalizations.Count == 0 && localLocalizations.Count == 0)
            return true;

        if (modelLocalizations.Count != localLocalizations.Count)
            return false;

        foreach (var (locale, localLocalization) in localLocalizations)
        {
            if (!modelLocalizations.TryGetValue(locale.Name, out var modelLocalization))
                return false;

            if (!localLocalization.Equals(modelLocalization, StringComparison.Ordinal))
                return false;
        }

        return true;
    }

    protected static bool AreEqual<TModel, TLocal>(Optional<TModel[]> collection, Optional<IList<TLocal>> otherCollection)
        where TModel : IEquatable<TLocal>
    {
        if (!collection.HasValue && !otherCollection.HasValue)
            return true;

        if (collection.HasValue != otherCollection.HasValue)
            return false;

        var modelCollection = collection.Value;
        var localCollection = otherCollection.Value;
        if (modelCollection.Length == 0 && localCollection.Count == 0)
            return true;

        if (modelCollection.Length != localCollection.Count)
            return false;

        for (var i = 0; i < modelCollection.Length; i++)
        {
            var modelValue = modelCollection[i];
            var localValue = localCollection[i];
            if (!modelValue.Equals(localValue))
                return false;
        }

        return true;
    }

    protected static bool AreEquivalent<T>(Optional<T[]> collection, Optional<IList<T>> otherCollection, IEqualityComparer<T>? equalityComparer = null, IComparer<T>? comparer = null)
    {
        if (!collection.HasValue && !otherCollection.HasValue)
            return true;

        if (collection.HasValue != otherCollection.HasValue)
            return false;

        var modelCollection = collection.Value;
        var localCollection = otherCollection.Value;
        if (modelCollection.Length == 0 && localCollection.Count == 0)
            return true;

        if (modelCollection.Length != localCollection.Count)
            return false;

        equalityComparer ??= EqualityComparer<T>.Default;
        comparer ??= Comparer<T>.Default;

        Array.Sort(modelCollection, comparer);

        var localCollectionArray = new T[localCollection.Count];
        Array.Sort(localCollectionArray, comparer);

        for (var i = 0; i < modelCollection.Length; i++)
        {
            var modelValue = modelCollection[i];
            var localValue = localCollection[i];
            if (!equalityComparer.Equals(localValue, modelValue))
                return false;
        }

        return true;
    }
}
