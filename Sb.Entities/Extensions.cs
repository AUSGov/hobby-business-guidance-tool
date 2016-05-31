using System;
using System.Linq.Expressions;
using Sb.Interfaces.Models;

namespace Sb.Entities
{
    public static class Extensions
    {
        public static Func<T, bool> CompileRule<T>(this IRule rule)
        {
            /*
                http://stackoverflow.com/questions/6488034/how-to-implement-a-rule-engine
                https://msdn.microsoft.com/en-us/library/system.linq.expressions.binaryexpression(v=vs.110).aspx
            */

            var visitorParameter = Expression.Parameter(typeof(T));
            var doesPropertyExist = typeof(T).GetProperty(rule.MemberName) != null;
            var expression = doesPropertyExist ? BuildExpressionForProperty<T>(rule, visitorParameter) : BuildExpressionForIndexer(rule, visitorParameter);
            return Expression.Lambda<Func<T, bool>>(expression, visitorParameter).Compile();
        }

        private static Expression BuildExpressionForProperty<T>(IRule rule, Expression visitorParameter)
        {
            var left = Expression.Property(visitorParameter, rule.MemberName);
            var propertyType = typeof(T).GetProperty(rule.MemberName).PropertyType;
            ExpressionType binaryType;
            if (Enum.TryParse(rule.Operator, out binaryType))
            {
                var right = Expression.Constant(Convert.ChangeType(rule.TargetValue, propertyType));
                return Expression.MakeBinary(binaryType, left, right);
            }
            else if (rule.Operator == "Contains")
            {
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var right = Expression.Constant(rule.TargetValue, typeof(string));
                return Expression.Call(left, method, right);
            }
            else if (rule.Operator == "NotContains")
            {
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var right = Expression.Constant(rule.TargetValue, typeof(string));
                return Expression.Not(Expression.Call(left, method, right));
            }
            else
            {
                var method = propertyType.GetMethod(rule.Operator);
                var parameterType = method.GetParameters()[0].ParameterType;
                var right = Expression.Constant(Convert.ChangeType(rule.TargetValue, parameterType));
                return Expression.Call(left, method, right);
            }
        }

        private static Expression BuildExpressionForIndexer(IRule rule, Expression visitorParameter)
        {
            var left = Expression.Property(visitorParameter, "Item", Expression.Constant(rule.MemberName));
            var propertyType = typeof(object);
            ExpressionType binaryType;

            if (Enum.TryParse(rule.Operator, out binaryType))
            {
                var right = Expression.Constant(Convert.ChangeType(rule.TargetValue, propertyType));
                return Expression.MakeBinary(binaryType, left, right);
            }
            else if (rule.Operator == "Contains")
            {
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var right = Expression.Constant(rule.TargetValue, typeof(string));
                return Expression.Call(left, method, right);
            }
            else if (rule.Operator == "NotContains")
            {
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var right = Expression.Constant(rule.TargetValue, typeof(string));
                return Expression.Not(Expression.Call(left, method, right));
            }
            else
            {
                var method = propertyType.GetMethod(rule.Operator);
                var parameterType = method.GetParameters()[0].ParameterType;
                var right = Expression.Constant(Convert.ChangeType(rule.TargetValue, parameterType));
                return Expression.Call(left, method, right);
            }
        }
    }
}