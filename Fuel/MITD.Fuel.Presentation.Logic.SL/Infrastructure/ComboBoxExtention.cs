using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.Presentation.Contracts;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.Logic.SL.Infrastructure
{
    public static class ComboBoxExtention
    {
        public static List<ComboBoxItm> ToComboItemList(this Type enumType)
        {
            return
                (from Enum enumValue in System.Enum.GetValues(enumType) 
                 select new ComboBoxItm() { Id = Convert.ToInt32(enumValue), Name = enumValue.GetDescription() }).ToList();
        }
    }
}