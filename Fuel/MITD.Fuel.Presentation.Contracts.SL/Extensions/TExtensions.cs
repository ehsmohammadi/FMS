using System;
using System.Linq.Expressions;
using System.Windows.Media;
using MITD.Fuel.Presentation.Contracts.Enums;

namespace MITD.Fuel.Presentation.Contracts.SL.Extensions
{
    public static class TExtensions
    {
        public static string GetPropertyName<T>(this T ownerParam, Expression<Func<T, object>> expressionParam) where T : class
        {
            var propName = string.Empty;

            if (expressionParam.Body is UnaryExpression)
            {
                if ((expressionParam.Body as UnaryExpression).Operand is MemberExpression)
                {
                    propName = ((expressionParam.Body as UnaryExpression).Operand as MemberExpression).Member.Name;
                    return propName;
                }
            }
            else if (!(expressionParam.Body is MemberExpression))
                return string.Empty;

            propName = (expressionParam.Body as MemberExpression).Member.Name;
            return propName;
        }
    }
}