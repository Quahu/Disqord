using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Qommon;

namespace Disqord
{
    public static partial class OptionalGuard
    {
        private static class Throw
        {
            [Pure]
            private static string GetNameString(string name)
            {
                if (name == null)
                    return "provided";

                if (name.StartsWith('"'))
                    return name;

                return $"\"{name}\"";
            }

            [Pure]
            private static string GetValueString(object value)
            {
                return value switch
                {
                    string _ => $"\"{value}\"",
                    null => "null",
                    _ => $"<{value}>"
                };
            }

            [DoesNotReturn]
            public static void ArgumentExceptionForHasValue<T>(Optional<T> optional, string message, string name)
            {
                throw new ArgumentException(message != null
                    ? $"Parameter {GetNameString(name)} ({typeof(Optional<T>).ToTypeString()}) must have a value: {GetValueString(message)}"
                    : $"Parameter {GetNameString(name)} ({typeof(Optional<T>).ToTypeString()}) must have a value.", name);
            }

            [DoesNotReturn]
            public static void ArgumentExceptionForHasNoValue<T>(Optional<T> optional, string message, string name)
            {
                throw new ArgumentException(message != null
                    ? $"Parameter {GetNameString(name)} ({typeof(Optional<T>).ToTypeString()}) must not have a value: {GetValueString(message)}"
                    : $"Parameter {GetNameString(name)} ({typeof(Optional<T>).ToTypeString()}) must not have a value.", name);
            }

            [DoesNotReturn]
            public static void ArgumentExceptionForCheckValue<T>(Optional<T> optional, string name, Exception exception)
            {
                throw new ArgumentException($"Parameter {GetNameString(name)} ({typeof(Optional<T>).ToTypeString()}) has a value which failed to pass an inner assertion.", name, exception);
            }
        }
    }
}
