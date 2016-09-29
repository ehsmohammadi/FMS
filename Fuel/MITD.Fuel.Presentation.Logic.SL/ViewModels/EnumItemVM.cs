using System.ComponentModel;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class EnumItemVM<TEnum> : ViewModelBase

    {
        private int enumValue;

        public int EnumValue
        {
            get { return enumValue; }
            set { this.SetField(p => p.EnumValue, ref enumValue, value); }
        }


        public string Code
        {
            get { return Name; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { this.SetField(p => p.Name, ref name, value); }
        }

        TEnum enumName;
        public TEnum EnumName
        {
            get { return enumName; }
            set { this.SetField(p => p.EnumName, ref enumName, value); }
        }
    }
}