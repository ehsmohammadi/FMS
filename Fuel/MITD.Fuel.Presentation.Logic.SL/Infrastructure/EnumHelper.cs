using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.Logic.SL.Infrastructure
{
    public static class EnumHelper
    {
        public static List<ComboBoxItm> GetItems(Type tenum)
        {
            var data = Enum.GetNames(tenum);
            var result = new List<ComboBoxItm>();

            foreach (var value in data)
            {

                var enumResult = Enum.Parse(tenum, value, true);
                var vm = new ComboBoxItm() { Id = Convert.ToInt32(enumResult), Name = value };

                result.Add(vm);
            }

            return result;
        }
    }
}
