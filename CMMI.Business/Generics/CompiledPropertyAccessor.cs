using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CMMI.Business.Generics
{
    internal class CompiledPropertyAccessor<TEntityType> : PropertyAccessor<TEntityType> where TEntityType : class
    {
        private readonly Func<TEntityType, object> _getter;
        private readonly Action<TEntityType, object> _setter;

        public CompiledPropertyAccessor(PropertyInfo property) : base(property)
        {
            _setter = MakeSetter(Property);
            _getter = MakeGetter(Property);
        }

        public override object GetValue(TEntityType entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            return _getter(entity);
        }

        public override void SetValue(TEntityType entity, object value)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _setter(entity, value);
        }


        private static Action<TEntityType, object> MakeSetter(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var type = typeof(TEntityType);
            var entityParameter = Expression.Parameter(type);
            var objectParameter = Expression.Parameter(typeof(object));
            var toProperty = Expression.Property(Expression.TypeAs(entityParameter, property.DeclaringType), property);
            var fromValue = Expression.Convert(objectParameter, property.PropertyType);
            var assignment = Expression.Assign(toProperty, fromValue);
            var lambda = Expression.Lambda<Action<TEntityType, object>>(assignment, entityParameter, objectParameter);
            return lambda.Compile();
        }

        private static Func<TEntityType, object> MakeGetter(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var type = typeof(TEntityType);
            var entityParameter = Expression.Parameter(type);
            var fromProperty = Expression.Property(Expression.TypeAs(entityParameter, property.DeclaringType), property);
            var convert = Expression.Convert(fromProperty, typeof(object));
            var lambda = Expression.Lambda<Func<TEntityType, object>>(convert, entityParameter);
            return lambda.Compile();
        }
    }
}