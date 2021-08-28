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
                            x.GetNullabilityInfo().ReadState == NullabilityState.NotNull);
            foreach (var property in properties)
            {
                var ruleBuilderInitial = validator.RuleFor(property);
                ruleBuilderInitial.NotNull();
            }
        }

        public static IRuleBuilderInitial<TTarget, object> RuleFor<TTarget>(this AbstractValidator<TTarget> validator, PropertyInfo property)
        {
            return validator.RuleFor<TTarget, object>(property);
        }

        public static IRuleBuilderInitial<TTarget, TProperty> RuleFor<TTarget, TProperty>(this AbstractValidator<TTarget> validator, PropertyInfo property)
        {
            var param = Expression.Parameter(typeof(TTarget));
            var body = Expression.Property(param, property);
            var converted = Expression.Convert(body, typeof(TProperty));
            var expression = Expression.Lambda<Func<TTarget, TProperty>>(converted, param);
            return validator.RuleFor(expression);
        }
    }
}