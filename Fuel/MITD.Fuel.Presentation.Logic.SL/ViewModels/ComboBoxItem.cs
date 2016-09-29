using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class ComboBoxItm : ViewModelBase
    {
        private long id;
        private string name;
        public long Id
        {
            get { return id; }
            set
            {
                this.SetField(p => p.Id, ref id, value);
            }
        }
        public string Name
        {
            get { return name; }
            set { this.SetField(p => p.Name, ref name, value); }
        }
    }
}
