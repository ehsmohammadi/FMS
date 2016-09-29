using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.Infrastructure
{
    public static class Extensions
    {
        public static void SetValues<T>(this T target, T source) where  T: DTOBase
        {
            var properties = source.GetType().GetProperties();

            properties.ToList().ForEach(pi =>
            {
                if (pi.MemberType == MemberTypes.Property && pi.CanWrite)
                {
                    var destProperty = target.GetType().GetProperty(pi.Name, BindingFlags.Public | BindingFlags.Instance);
                    if (destProperty != null && destProperty.PropertyType.IsAssignableFrom(pi.PropertyType))
                        destProperty.SetValue(target, pi.GetValue(source, new object[] { }), new object[] { });
                }
            });
        }

        public static IDictionary<string, object> GetPropertiesValuesDictionary(this object source)
        {
            var properties = source.GetType().GetProperties();

            var result = new Dictionary<string, object>();

            properties.Where(p => p.MemberType == MemberTypes.Property && p.CanRead).ToList().ForEach(i => result.Add(i.Name, i.GetValue(source, new object[] { })));

            return result;
        }
    }
}
