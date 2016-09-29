using System.ComponentModel;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class DataGridCommandViewModel : ViewModelBase
    {
        private CommandViewModel commandViewModel;
        private string icon;
        private long count;

        public long Count 
        { 
            get { return count; }
            set
            {
                if (count == value) return;
                count = value;
                OnPropertyChanged("Count");
            }
        }

        public CommandViewModel CommandViewModel
        {
            get { return commandViewModel; }
            set { this.SetField(p => p.CommandViewModel, ref commandViewModel, value); }
        }
        public string Icon
        {
            get { return icon; }
            set
            {
                if (icon == value) return;
                icon = value;
                OnPropertyChanged("Icon");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
