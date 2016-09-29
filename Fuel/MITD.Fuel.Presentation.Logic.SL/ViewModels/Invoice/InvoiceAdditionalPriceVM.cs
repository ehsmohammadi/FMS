#region

using System;
using System.Collections.ObjectModel;
using System.Linq;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;

#endregion

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels.Invoice
{
    public class InvoiceAdditionalPriceVM : WorkspaceViewModel
    {
        #region Prop

        private readonly IFuelController mainController;
        private CommandViewModel cancelCommand;
        private InvoiceAdditionalPriceDto entity;
        private IInvoiceServiceWrapper serviceWrapper;
        public Guid UniqId { get; set; }

        private CommandViewModel submitCommand;

        public CommandViewModel SubmitCommand
        {
            get
            {
                return submitCommand ?? (submitCommand = new CommandViewModel("ذخیره",
                    new DelegateCommand(Save)));
            }
        }

        public CommandViewModel CancelCommand
        {
            get { return cancelCommand ?? (cancelCommand = new CommandViewModel("خروج", new DelegateCommand(() => mainController.Close(this)))); }
        }


        public InvoiceAdditionalPriceDto Entity
        {
            get { return entity; }
            set { this.SetField(p => p.Entity, ref entity, value); }
        }
        ObservableCollection<EffectiveFactorDto> effectiveFactors;

        public ObservableCollection<EffectiveFactorDto> EffectiveFactors
        {
            get
            {
                return effectiveFactors;
            }
            set { this.SetField(p => p.EffectiveFactors, ref effectiveFactors, value); }
        }

        long selectedCurrencyId;
        private long selectedEffectiveFactorId;

        public long SelectedEffectiveFactorId
        {
            get
            {
                selectedEffectiveFactorId = Entity.EffectiveFactorId;
                return selectedEffectiveFactorId;
            }
            set
            {
                entity.EffectiveFactorId = value;

                var effectiveFactorDto = EffectiveFactors.SingleOrDefault(c => c.Id == value);
                if (effectiveFactorDto != null)
                {
                    entity.EffectiveFactorName = effectiveFactorDto.Name;
                    entity.EffectiveFactorType = effectiveFactorDto.EffectiveFactorType;
                }

                this.SetField(p => p.SelectedEffectiveFactorId, ref selectedEffectiveFactorId, value);
            }
        }

        #endregion

        #region ctor

        public InvoiceAdditionalPriceVM()
        {
        }

        public InvoiceAdditionalPriceVM(IFuelController appController,
                                        IInvoiceServiceWrapper invoiceServiceWrapper)
        {
            mainController = appController;
            serviceWrapper = invoiceServiceWrapper;
            Entity = new InvoiceAdditionalPriceDto();
            DisplayName = "ویرایش عوامل تاثیر گذار ";

        }

        #endregion

        #region Method

        private void Save()
        {
            if (!entity.Validate())
                return;

            if (!editMode)
                mainController.Publish(new InvoiceAdditionalPriceEditedArg {InvoiceAdditionalPrice = Entity,UniqId = UniqId});
            mainController.Close(this);
        }

        public void Load(InvoiceAdditionalPriceDto invoiceAdditionalPrice, ObservableCollection<EffectiveFactorDto> factors, decimal currencyToMainCurrencyRate)
        {
            EffectiveFactors = factors;
            Entity = invoiceAdditionalPrice;
            SelectedEffectiveFactorId = invoiceAdditionalPrice.EffectiveFactorId;
            editMode = true;
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            mainController.Close(this);
        }

        #endregion

        private bool editMode = false;
        public void SetCollection(ObservableCollection<EffectiveFactorDto> factors, decimal currencyToMainCurrencyRate, Guid uniqId)
        {
            Entity = new InvoiceAdditionalPriceDto();
            EffectiveFactors = factors;
            Entity.CurrencyToMainCurrencyRate = currencyToMainCurrencyRate;
            UniqId = uniqId;

        }
    }
}