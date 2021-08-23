using System.Collections.Concurrent;

namespace System.Reflection
{
    /// <summary>
    /// Static and thread safe wrapper around <see cref="NullabilityInfoContext"/>.
    /// </summary>
    internal static partial class NullabilityInfoExtensions
    {
        static ConcurrentDictionary<ParameterInfo, NullabilityInfo> parameterCache = new();
        static ConcurrentDictionary<PropertyInfo, NullabilityInfo> propertyCache = new();
        static ConcurrentDictionary<EventInfo, NullabilityInfo> eventCache = new();
        static ConcurrentDictionary<FieldInfo, NullabilityInfo> fieldCache = new();

        internal static NullabilityInfo GetNullabilityInfo(this FieldInfo info)
        {
            return fieldCache.GetOrAdd(info, inner =>
            {
                var nullabilityContext = new NullabilityInfoContext();
                return nullabilityContext.Create(inner);
            });
        }

        internal static NullabilityState GetNullability(this FieldInfo info)
        {
            return info.GetNullabilityInfo().ReadState;
        }

        internal static bool IsNullable(this FieldInfo info)
        {
            var nullability = info.GetNullabilityInfo();
            return IsNullable(info.Name, nullability);
        }

        internal static NullabilityInfo GetNullabilityInfo(this EventInfo info)
        {
            return eventCache.GetOrAdd(info, inner =>
            {
                var nullabilityContext = new NullabilityInfoContext();
                return nullabilityContext.Create(inner);
            });
        }

        internal static NullabilityState GetNullability(this EventInfo info)
        {
            return info.GetNullabilityInfo().ReadState;
        }

        internal static bool IsNullable(this EventInfo info)
        {
            var nullability = info.GetNullabilityInfo();
            return IsNullable(info.Name, nullability);
        }

        internal static NullabilityInfo GetNullabilityInfo(this PropertyInfo info)
        {
            return propertyCache.GetOrAdd(info, inner =>
            {
                var nullabilityContext = new NullabilityInfoContext();
                return nullabilityContext.Create(inner);
            });
        }

        internal static NullabilityState GetNullability(this PropertyInfo info)
        {
            return info.GetNullabilityInfo().ReadState;
        }

        internal static bool IsNullable(this PropertyInfo info)
        {
            var nullability = info.GetNullabilityInfo();
            return IsNullable(info.Name, nullability);
        }

        internal static NullabilityInfo GetNullabilityInfo(this ParameterInfo info)
        {
            return parameterCache.GetOrAdd(info, inner =>
            {
                var nullabilityContext = new NullabilityInfoContext();
                return nullabilityContext.Create(inner);
            });
        }

        internal static NullabilityState GetNullability(this ParameterInfo info)
        {
            return info.GetNullabilityInfo().ReadState;
        }

        internal static bool IsNullable(this ParameterInfo info)
        {
            var nullability = info.GetNullabilityInfo();
            return IsNullable(info.Name!, nullability);
        }

        static bool IsNullable(string name, NullabilityInfo nullability)
        {
            var readState = nullability.ReadState;
            if (readState == NullabilityState.Nullable)
            {
                return true;
            }

            if (readState == NullabilityState.NotNull)
            {
                return false;
            }

            var writeState = nullability.WriteState;
            if (writeState == NullabilityState.Nullable)
            {
                return true;
            }

            if (writeState == NullabilityState.NotNull)
            {
                return false;
            }

            throw new($"The nullability of '{name}' is unknown.");
        }
    }
}