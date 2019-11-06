//using System;
//using Disqord.Models;

//namespace Disqord.Rest.AuditLogs
//{
//    public readonly struct AuditLogValue<T>
//    {
//        public Optional<T> OldValue { get; }

//        public Optional<T> NewValue { get; }

//        public bool WasModified => OldValue.HasValue || NewValue.HasValue;

//        internal AuditLogValue(Optional<T> oldValue, Optional<T> newValue)
//        {
//            OldValue = oldValue;
//            NewValue = newValue;
//        }

//        internal AuditLogValue(AuditLogChangeModel model)
//        {
//            OldValue = model.OldValue.HasValue
//                ? (T) model.OldValue.Value
//                : Optional<T>.Empty;

//            NewValue = model.NewValue.HasValue
//                ? (T) model.NewValue.Value
//                : Optional<T>.Empty;
//        }

//        internal AuditLogValue(AuditLogChangeModel model, Func<object, T> func)
//        {
//            OldValue = model.OldValue.HasValue
//                ? func(model.OldValue.Value)
//                : Optional<T>.Empty;

//            NewValue = model.NewValue.HasValue
//                ? func(model.NewValue.Value)
//                : Optional<T>.Empty;
//        }

//        public override string ToString()
//            => $"{OldValue} | {NewValue}";
//    }
//}
