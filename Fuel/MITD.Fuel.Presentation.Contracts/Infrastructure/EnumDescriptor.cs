using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Presentation.Contracts
{
    public static class EnumDescriptor
    {
        public static string GetDescription(this Enum value)
        {
            try
            {
                var fieldInfo = value.GetType().GetField(value.ToString());

                var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static List<EnumX> GetListOfEnumKeyValue(this Type enumType)
        {
            var kvPairList = new List<EnumX>();

            foreach (System.Enum enumValue in System.Enum.GetValues(enumType))
            {
                kvPairList.Add(new EnumX(Convert.ToInt32(enumValue), GetDescription(enumValue), enumValue.ToString()));
            }

            return kvPairList;
        }

       
    }
    public class EnumX

{
        public EnumX(int id, string name, string enumName)
        {
            Id = id;
            Name = name;
            EnumName = enumName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string EnumName { get; set; }
}
}
