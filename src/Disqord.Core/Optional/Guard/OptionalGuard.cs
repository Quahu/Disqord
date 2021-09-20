using System;
using System.Runtime.CompilerServices;

namespace Disqord
{
    public static partial class OptionalGuard
    {
        /// <summary>
        ///     Asserts that the input optional has a value.
        /// </summary>
        /// <typeparam name="T"> The type of reference value type being tested. </typeparam>
        /// <param name="optional"> The input optional to test. </param>
        /// <param name="name"> The name of the input parameter being tested. </param>
        /// <exception cref="ArgumentException"> Thrown if <paramref name="optional"/> has no value. </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasValue<T>(Optional<T> optional, [CallerArgumentExpression("optional")] string name = null)
            where T : class
        {
            if (optional.HasValue)
                return;

            Throw.ArgumentExceptionForHasValue(optional, name);
        }

        /// <summary>
        ///     Asserts that the input optional has no value.
        /// </summary>
        /// <typeparam name="T"> The type of reference value type being tested. </typeparam>
        /// <param name="optional"> The input optional to test. </param>
        /// <param name="name"> The name of the input parameter being tested. </param>
        /// <exception cref="ArgumentException"> Thrown if <paramref name="optional"/> has a value. </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasNoValue<T>(Optional<T> optional, [CallerArgumentExpression("optional")] string name = null)
            where T : class
        {
            if (!optional.HasValue)
                return;

            Throw.ArgumentExceptionForHasNoValue(optional, name);
        }

        /// <summary>
        ///     Asserts that the value of the input optional passes inner assertions.
        /// </summary>
        /// <typeparam name="T"> The type of reference value type being tested. </typeparam>
        /// <param name="optional"> The input optional to test. </param>
        /// <param name="assert"> The action invoking inner assertions. </param>
        /// <param name="name"> The name of the input parameter being tested. </param>
        /// <exception cref="ArgumentException"> Thrown if <paramref name="optional"/> has a value but it failed to pass assertions. </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckValue<T>(Optional<T> optional, Action<T> assert, [CallerArgumentExpression("optional")] string name = null)
            where T : class
        {
            if (!optional.HasValue)
                return;

            try
            {
                assert(optional.Value);
            }
            catch (Exception ex)
            {
                Throw.ArgumentExceptionForCheckValue(optional, name, ex);
            }
        }
    }
}
