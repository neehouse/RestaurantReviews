﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CMMI.Business.Generics
{
    internal class CompiledPropertyAccessor<TEntityType> : PropertyAccessor<TEntityType> where TEntityType : class
    {
        private Action<TEntityType, object> _setter;
        private Func<TEntityType, object> _getter;

        public CompiledPropertyAccessor(PropertyInfo property)
            : base(property)
        {
            _setter = MakeSetter(Property);
            _getter = MakeGetter(Property);
        }

        public override object GetValue(TEntityType entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            return _getter(entity);
        }

        public override void SetValue(TEntityType entity, object value)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _setter(entity, value);
        }

        private static Action<TEntityType, object> MakeSetter(PropertyInfo property)
        {
            Type type = typeof(TEntityType);
            ParameterExpression entityParameter = Expression.Parameter(type);
            ParameterExpression objectParameter = Expression.Parameter(typeof(Object));
            MemberExpression toProperty = Expression.Property(Expression.TypeAs(entityParameter, property.DeclaringType), property);
            UnaryExpression fromValue = Expression.Convert(objectParameter, property.PropertyType);
            BinaryExpression assignment = Expression.Assign(toProperty, fromValue);
            Expression<Action<TEntityType, object>> lambda = Expression.Lambda<Action<TEntityType, object>>(assignment, entityParameter, objectParameter);
            return lambda.Compile();
        }

        private static Func<TEntityType, object> MakeGetter(PropertyInfo property)
        {
            Type type = typeof(TEntityType);
            ParameterExpression entityParameter = Expression.Parameter(type);
            MemberExpression fromProperty = Expression.Property(Expression.TypeAs(entityParameter, property.DeclaringType), property);
            UnaryExpression convert = Expression.Convert(fromProperty, typeof(Object));
            Expression<Func<TEntityType, object>> lambda = Expression.Lambda<Func<TEntityType, object>>(convert, entityParameter);
            return lambda.Compile();
        }
    }
}