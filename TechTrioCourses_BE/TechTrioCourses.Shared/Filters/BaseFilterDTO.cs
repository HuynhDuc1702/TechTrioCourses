using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TechTrioCourses.Shared.Filters
{
    public abstract class BaseFilterDTO
    {
        public Expression<Func<T, bool>> BuildExpression<T>()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression finalExpression = null;

            var properties = this.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var attribute = prop.GetCustomAttribute<FilterFieldAttribute>();
                if (attribute == null) continue;

                var value = prop.GetValue(this);
                if (value == null) continue;

                // Handle string empty checks if needed, but usually empty search means "all" or specific value
                if (value is string str && string.IsNullOrEmpty(str)) continue;

                var targetField = attribute.TargetField ?? prop.Name;
                var memberExpression = GetMemberExpression(parameter, targetField);
                var constantExpression = Expression.Constant(value);

                // Handle nullable types mapping if necessary
                 if (memberExpression.Type != constantExpression.Type)
                {
                    // If target is nullable and value is not, convert value
                    if (Nullable.GetUnderlyingType(memberExpression.Type) != null && constantExpression.Type == Nullable.GetUnderlyingType(memberExpression.Type))
                    {
                         constantExpression = Expression.Constant(value, memberExpression.Type);
                    }
                     // If value is nullable and target is not... usually not the case for filters coming from DTO
                }

                Expression comparison = null;

                switch (attribute.Operation)
                {
                    case FilterOperationEnum.Equal:
                        comparison = Expression.Equal(memberExpression, constantExpression);
                        break;
                    case FilterOperationEnum.NotEqual:
                        comparison = Expression.NotEqual(memberExpression, constantExpression);
                        break;
                    case FilterOperationEnum.Contains:
                        // Ensure it's a string for Contains
                        if (memberExpression.Type == typeof(string))
                        {
                            var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                            comparison = Expression.Call(memberExpression, method, constantExpression);
                        }
                        break;
                    case FilterOperationEnum.StartsWith:
                         if (memberExpression.Type == typeof(string))
                        {
                            var method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                            comparison = Expression.Call(memberExpression, method, constantExpression);
                        }
                        break;
                     case FilterOperationEnum.EndsWith:
                         if (memberExpression.Type == typeof(string))
                        {
                            var method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                            comparison = Expression.Call(memberExpression, method, constantExpression);
                        }
                        break;
                    case FilterOperationEnum.GreaterThan:
                        comparison = Expression.GreaterThan(memberExpression, constantExpression);
                        break;
                    case FilterOperationEnum.GreaterThanOrEqual:
                        comparison = Expression.GreaterThanOrEqual(memberExpression, constantExpression);
                        break;
                    case FilterOperationEnum.LessThan:
                        comparison = Expression.LessThan(memberExpression, constantExpression);
                        break;
                    case FilterOperationEnum.LessThanOrEqual:
                        comparison = Expression.LessThanOrEqual(memberExpression, constantExpression);
                        break;
                }

                if (comparison != null)
                {
                    if (finalExpression == null)
                    {
                        finalExpression = comparison;
                    }
                    else
                    {
                        finalExpression = Expression.AndAlso(finalExpression, comparison);
                    }
                }
            }

            if (finalExpression == null)
            {
                // If no filters are applied, return true for all
                return x => true;
            }

            return Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
        }

        private MemberExpression GetMemberExpression(Expression param, string propertyName)
        {
            // Handles nested properties like "Category.Name"
            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }
            return (MemberExpression)body;
        }
    }
}
