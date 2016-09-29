using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace MITD.Fuel.Data.EF.Extensions
{
    public static class Extensions
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


      
            public static PrimitivePropertyConfiguration IsUnique(this PrimitivePropertyConfiguration source)
            {
                try
                {
                    var uniqueIndexAttribute = new IndexAttribute { IsUnique = true };

                    source.HasColumnAnnotation("Index", new IndexAnnotation(uniqueIndexAttribute));
                }
                catch
                {
                }

                return source;
            }

            public static PrimitivePropertyConfiguration IsUnique(this PrimitivePropertyConfiguration source, string indexName, int order = 1)
            {
                try
                {
                    var uniqueIndexAttribute = new IndexAttribute(indexName, order) { IsUnique = true };

                    source.HasColumnAnnotation("Index", new IndexAnnotation(uniqueIndexAttribute));
                }
                catch
                {
                }

                return source;
            }
 
    }

    public class Product
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class Test
    {
        public void Get()
        {
            var ent = new Product();
            var name = ent.GetPropertyName(d => d.Name);
        }
    }

}