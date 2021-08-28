using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentValidation
{
    public static class NullableValidatorExtensions
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

        public static void AddNullableRules<T>(this AbstractValidator<T> validator)
        {
            var type = typeof(T);
            var properties = type.GetProperties(flags)
                .Where(x => x.GetMethod != null && 
                            NullabilityInfoExtensions.GetNullabilityInfo((PropertyInfo)x).ReadState == NullabilityState.NotNull);
            foreach (var property in properties)
            {
                var param = Expression.Parameter(type);
                var body = Expression.Property(param, property);
                var converted = Expression.Convert(body, typeof(object));
                var expression = Expression.Lambda<Func<T, object>>(converted, param);
                validator.RuleFor(expression).NotNull();
            }
        }
    }
}