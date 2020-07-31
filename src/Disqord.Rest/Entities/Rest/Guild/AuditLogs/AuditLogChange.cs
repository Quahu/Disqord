using System;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.AuditLogs
{
    public readonly struct AuditLogChange<T>
    {
        public Optional<T> OldValue { get; }

        public Optional<T> NewValue { get; }

        public bool WasChanged => OldValue.HasValue || NewValue.HasValue;

        internal AuditLogChange(Optional<T> oldValue, Optional<T> newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        internal static AuditLogChange<T> Convert(AuditLogChangeModel model)
        {
            var oldValue = model.OldValue.HasValue
                ? model.OldValue.Value.ToType<T>()
                : Optional<T>.Empty;

            var newValue = model.NewValue.HasValue
                ? model.NewValue.Value.ToType<T>()
                : Optional<T>.Empty;

            return new AuditLogChange<T>(oldValue, newValue);
        }

        internal static AuditLogChange<T> Convert<TIn>(AuditLogChangeModel model, Converter<TIn, T> converter)
        {
            var oldValue = model.OldValue.HasValue
                ? converter(model.OldValue.Value.ToType<TIn>())
                : Optional<T>.Empty;

            var newValue = model.NewValue.HasValue
                ? converter(model.NewValue.Value.ToType<TIn>())
                : Optional<T>.Empty;

            return new AuditLogChange<T>(oldValue, newValue);
        }

        public override string ToString()
            => $"{OldValue} | {NewValue}";
    }
}
