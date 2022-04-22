using System;
using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs
{
    public readonly struct AuditLogChange<T>
    {
        public Optional<T> OldValue { get; }

        public Optional<T> NewValue { get; }

        public bool WasChanged => OldValue.HasValue || NewValue.HasValue;

        public AuditLogChange(Optional<T> oldValue, Optional<T> newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        internal static AuditLogChange<T> Convert(AuditLogChangeJsonModel model)
        {
            var oldValue = Optional.Convert(model.OldValue, model => model.ToType<T>());
            var newValue = Optional.Convert(model.NewValue, model => model.ToType<T>());
            return new(oldValue, newValue);
        }

        internal static AuditLogChange<T> Convert<TOld>(AuditLogChangeJsonModel model, Converter<TOld, T> converter)
        {
            var oldValue = model.OldValue.HasValue
                ? converter(model.OldValue.Value.ToType<TOld>())
                : Optional<T>.Empty;

            var newValue = model.NewValue.HasValue
                ? converter(model.NewValue.Value.ToType<TOld>())
                : Optional<T>.Empty;

            return new(oldValue, newValue);
        }

        internal static AuditLogChange<T> Convert<TOld, TState>(AuditLogChangeJsonModel model, TState state, Func<TOld, TState, T> converter)
        {
            var oldValue = model.OldValue.HasValue
                ? converter(model.OldValue.Value.ToType<TOld>(), state)
                : Optional<T>.Empty;

            var newValue = model.NewValue.HasValue
                ? converter(model.NewValue.Value.ToType<TOld>(), state)
                : Optional<T>.Empty;

            return new(oldValue, newValue);
        }

        public override string ToString()
            => $"{OldValue} -> {NewValue}";
    }
}
