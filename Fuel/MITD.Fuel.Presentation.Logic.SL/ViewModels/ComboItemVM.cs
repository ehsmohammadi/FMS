using System;
using System.ComponentModel;
using System.Linq.Expressions;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class ComboItemVM<T>:INotifyPropertyChanged where T:ViewModelBase
    {
        public ComboItemVM(T dto, Expression<Func<T, object>> key, Expression<Func<T, object>> value)
        {
            this.Dto = dto;
            this.KeyFieldName = dto.GetPropertyName(key);
            this.ValueFieldName = dto.GetPropertyName(value);
        }

        public T Dto { get;private set; }

        public string KeyFieldName { get;private set; }

        public string ValueFieldName { get;private set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}