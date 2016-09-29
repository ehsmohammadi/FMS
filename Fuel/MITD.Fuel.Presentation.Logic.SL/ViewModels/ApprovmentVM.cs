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
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Contracts.SL.Views;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels

{
    public class ApprovmentVM : WorkspaceViewModel
    {


        IFuelController mainController; 
        private IApprovalFlowServiceWrapper serviceWrapper;
        private CommandViewModel saveCommand;
        private ApprovmentDto entity;
        private IApprovmentView _view;
        IApproveRejectVM _actionOfApproveReject;
        public IApproveRejectVM ActionOfApproveReject
        {
            get
            {
                return _actionOfApproveReject;
            }
            set
            {
                _actionOfApproveReject = value;
            }
        }


           //<TDto> where TDto : DTOBase

        public ApprovmentDto Entity
        {
            get { return entity; }
            set
            {
                this.SetField(p => p.Entity, ref entity, value);

            }
        }
     

        public ApprovmentVM()
        {
        }


        public ApprovmentVM(IFuelController mainController, IApprovalFlowServiceWrapper serviceWrapper)
            : this()
        {
            this.mainController = mainController;
            this.serviceWrapper = serviceWrapper;
            Entity = new ApprovmentDto();
            DisplayName = "تأیید/لغو عملیات ";

            this.RequestClose += ApprovmentVM_RequestClose;
        }

        public new IApprovmentView View
        {
            get { return _view; }
            set
            {
                _view = value;
            }
        }

        int selectedEntityId;
        public int SelectedEntityId
        {
            get { return selectedEntityId; }
            set
            {
                this.SetField(p => p.selectedEntityId, ref selectedEntityId, value);

            }
        }

        #region methods

     

        void ApprovmentVM_RequestClose(object sender, EventArgs e)
        {
            this.mainController.Close(this);
        }
        #endregion
    }
}
