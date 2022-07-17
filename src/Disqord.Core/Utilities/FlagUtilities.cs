using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Qommon.Collections.Synchronized;

namespace Disqord.Utilities;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class FlagUtilities
{
    public static IEnumerable<TFlag> GetFlags<TFlag>(TFlag flags)
        where TFlag : Enum
    {
        var value = (flags as IConvertible).ToUInt64(null);
        var allFlags = GetAllFlags<TFlag>();
        for (var i = 0; i < allFlags.Length; i++)
        {
            var flag = allFlags[i];
            if (flag > value)
                yield break;

            if ((value & flag) == flag)
                yield return (TFlag) (object) flag;
        }
    }

    public static IEnumerable<KeyValuePair<TFlag, bool>> GetFlagBools<TFlag>(TFlag flags)
        where TFlag : Enum
    {
        var value = (flags as IConvertible).ToUInt64(null);
        var allFlags = GetAllFlags<TFlag>();
        for (var i = 0; i < allFlags.Length; i++)
        {
            var flag = allFlags[i];
            yield return KeyValuePair.Create((TFlag) (object) flag, flag <= value && (value & flag) == flag);
        }
    }

    internal static ulong[] GetAllFlags<TFlag>()
        where TFlag : Enum
        => FlagsCache.GetOrAdd(typeof(TFlag), type =>
        {
            var flags = (TFlag[]) Enum.GetValues(type);
            return flags.Select(x => (x as IConvertible).ToUInt64(null)).Where(x => x != 0).ToArray();
        });

    internal static readonly ISynchronizedDictionary<Type, ulong[]> FlagsCache = new SynchronizedDictionary<Type, ulong[]>();
}