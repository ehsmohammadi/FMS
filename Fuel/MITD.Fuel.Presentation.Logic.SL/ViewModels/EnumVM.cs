using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts;
namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class EnumVM<TEnum> : ViewModelBase
    {

        public ObservableCollection<EnumItemVM<TEnum>> Items { get; set; }

        private EnumItemVM<TEnum> selectedItem;
        public EnumItemVM<TEnum> SelectedItem
        {
            get { return selectedItem; }
            set
            {

                this.SetField(d => d.SelectedItem, ref selectedItem, value);

            }
        }
        public void SetSelected(TEnum value)
        {
            var name = value.ToString();
            var item = this.Items.FirstOrDefault(i=>i.Name==name);
            this.selectedItem = item;
        }

        public EnumVM()
            : base()
        {
            var enumType = typeof(TEnum);
            var vmItems = this.CreateItems(enumType);

            this.Items = new ObservableCollection<EnumItemVM<TEnum>>(vmItems);
            this.SelectedItem = null;

        }

        private List<EnumItemVM<TEnum>> CreateItems(Type enumType)
        {
            var data = //Enum.GetNames(enumType);
            enumType.GetListOfEnumKeyValue();
            var result = new List<EnumItemVM<TEnum>>();

            foreach (var value in data)
            {

                //var enumResult = (TEnum) Enum.Parse(enumType, value, true);
                var vm = new EnumItemVM<TEnum>
                             {
                                 EnumValue = Convert.ToInt32(value.Id),
                                 Name = value.Name,
                                 EnumName   = (TEnum) Enum.Parse(enumType, value.EnumName, true)
                             };

                result.Add(vm);
            }

            return result;
        }
//         private List<EnumItemVM<TEnum>> CreateItems(Type enumType)
//        {
//            var data = Enum.GetNames(enumType);
//            var result = new List<EnumItemVM<TEnum>>();
//
//            foreach (var value in data)
//            {
//
//                var enumResult = (TEnum) Enum.Parse(enumType, value, true);
//                var vm = new EnumItemVM<TEnum>
//                             {
//                                 EnumValue = Convert.ToInt32(enumResult),
//                                 Name = value,
//                                 EnumName = enumResult
//                             };
//
//                result.Add(vm);
//            }
//
//            return result;
//        }
    }
}