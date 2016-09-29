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
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class CharterVM : WorkspaceViewModel
    {
        #region Prop
        CharterType CurrentCharterType { get; set; }




        private CharterStartVM charterStartVm;
        public CharterStartVM CharterStartVm
        {
            get { return charterStartVm; }
            set
            {
                this.SetField(p => p.CharterStartVm, ref charterStartVm, value);

            }
        }

        private CharterEndVM _charterEndVm;
        public CharterEndVM CharterEndVm
        {
            get { return _charterEndVm; }
            set
            {
                this.SetField(p => p.CharterEndVm, ref _charterEndVm, value);

            }
        }

       

        private long charterId;

        public long CharterId
        {
            get { return charterId; }
            set
            {
                this.SetField(p => p.CharterId, ref charterId, value);

            }
        }

        #endregion

        #region Ctor

        public CharterVM()
        {

            CharterStartVm = ServiceLocator.Current.GetInstance<CharterStartVM>();
            charterStartVm.UploaderVm = ServiceLocator.Current.GetInstance<UploaderVM>();
            CharterEndVm = ServiceLocator.Current.GetInstance<CharterEndVM>();
            CharterEndVm.UploaderVm = ServiceLocator.Current.GetInstance<UploaderVM>();
            this.DisplayName = "شروع / پایان چارتر";
        }
        #endregion


        #region Method
        public void SetCharterType(CharterType charterType)
        {
            if (charterType == CharterType.In)
            {
                this.DisplayName = "چارتر In";
               
            }
            else
            {
                this.DisplayName = "چارتر Out";
            }
            CurrentCharterType = charterType;
        }

        public void Load(long charterId)
        {
            if (charterId > 0)
            {
                CharterStartVm.SetCharterType(CurrentCharterType);
                CharterStartVm.CharterId = charterId;
                CharterEndVm.SetCharterType(CurrentCharterType);
                CharterEndVm.CharterId = charterId;
              
            }
            else
            {
                
                CharterStartVm.SetCharterType(CurrentCharterType);
                
                CharterStartVm.LoadGeneralItems();

                CharterStartVm.Entity.StartDate = DateTime.Now.Date;
                CharterEndVm.SetCharterType(CurrentCharterType);
                CharterEndVm.LoadGeneralItems();
                CharterEndVm.Entity.EndDate = DateTime.Now.Date;
            }

        }

        #endregion
    }
}
