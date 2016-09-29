using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class OffhireDto
    {
        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        private long referenceNumber;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "ReferenceNumber must be set.")]
        public long ReferenceNumber
        {
            get { return referenceNumber; }
            set { this.SetField(p => p.ReferenceNumber, ref this.referenceNumber, value); }
        }

        private DateTime startDateTime;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "StartDateTime must be set.")]
        //[DataMember(IsRequired = true)]
        public DateTime StartDateTime
        {
            get { return startDateTime; }
            set { this.SetField(p => p.StartDateTime, ref this.startDateTime, value); }
        }

        private DateTime endDateTime;
        //[System.ComponentModel.DataAnnotations.(AllowEmptyStrings = false, ErrorMessage = "EndDateTime must be set.")]
        //[DataMember(IsRequired = true)]
        public DateTime EndDateTime
        {
            get { return endDateTime; }
            set { this.SetField(p => p.EndDateTime, ref this.endDateTime, value); }
        }

        private CharteringPartyType introducerType;
        public CharteringPartyType IntroducerType
        {
            get { return introducerType; }
            set { this.SetField(p => p.IntroducerType, ref this.introducerType, value); }
        }

        private CompanyDto introducer;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Introducer must be set.")]
        public CompanyDto Introducer
        {
            get { return introducer; }
            set { this.SetField(p => p.Introducer, ref this.introducer, value); }
        }

        private VesselInCompanyDto vesselInCompany;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Vessel must be set.")]
        public VesselInCompanyDto VesselInCompany
        {
            get { return vesselInCompany; }
            set { this.SetField(p => p.VesselInCompany, ref this.vesselInCompany, value); }
        }

        private VoyageDto voyage;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Voyage must be set.")]
        public VoyageDto Voyage
        {
            get { return voyage; }
            set { this.SetField(p => p.Voyage, ref this.voyage, value); }
        }

        private ActivityLocationDto offhireLocation;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "OffhireLocation must be set.")]
        public ActivityLocationDto OffhireLocation
        {
            get { return offhireLocation; }
            set { this.SetField(p => p.OffhireLocation, ref this.offhireLocation, value); }
        }

        private DateTime? voucherDate;
        //[Range(typeof(DateTime), "2000/01/01 00:00:00", "2100/01/01 00:00:00", ErrorMessage = "VoucherDate is incorrect.")]
        public DateTime? VoucherDate
        {
            get { return voucherDate; }
            set { this.SetField(p => p.VoucherDate, ref this.voucherDate, value); }
        }

        private CurrencyDto voucherCurrency;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "VoucherCurrency must be selected.")]
        public CurrencyDto VoucherCurrency
        {
            get { return voucherCurrency; }
            set { this.SetField(p => p.VoucherCurrency, ref this.voucherCurrency, value); }
        }

        private ObservableCollection<OffhireDetailDto> offhireDetails;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "OffhireDetails must be set.")]
        public ObservableCollection<OffhireDetailDto> OffhireDetails
        {
            get { return this.offhireDetails; }
            set { this.SetField(p => p.OffhireDetails, ref this.offhireDetails, value); }
        }

        private UserDto userInCharge;
        public UserDto UserInCharge
        {
            get { return userInCharge; }
            set { this.SetField(p => p.UserInCharge, ref userInCharge, value); }
        }

        private string currentState;
        public string CurrentState
        {
            get { return currentState; }
            set { this.SetField(p => p.CurrentState, ref currentState, value); }
        }

        private List<PricingValueDto> pricingValuesInMainCurrency;
        public List<PricingValueDto> PricingValuesInMainCurrency
        {
            get { return pricingValuesInMainCurrency; }
            set { this.SetField(p => p.PricingValuesInMainCurrency, ref pricingValuesInMainCurrency, value); }
        }

        private bool isOffhireEditPermitted;
        public bool IsOffhireEditPermitted
        {
            get { return isOffhireEditPermitted; }
            set { this.SetField(p => p.IsOffhireEditPermitted, ref isOffhireEditPermitted, value); }
        }

        private bool isOffhireDeletePermitted;
        public bool IsOffhireDeletePermitted
        {
            get { return isOffhireDeletePermitted; }
            set { this.SetField(p => p.IsOffhireDeletePermitted, ref isOffhireDeletePermitted, value); }
        }
    }
}
