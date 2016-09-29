using System;
using System.Collections.Generic;
using MITD.Fuel.ACL.Contracts.ServiceWrappers;
using MITD.Fuel.Infrastructure.Service;
using MITD.Services.AntiCorruption;
using MITD.Services.AntiCorruption.Contracts;
using MITD.Services.Connectivity;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.ServiceWrappers
{
    public class CompanyAntiCorruptionServiceWrapper : BaseAntiCorruptionServiceWrapper<EnterprisePartyDto>, ICompanyAntiCorruptionServiceWrapper
    {
        #region props

        private ExternalHostAddressHelper AddressHelper { get; set; }

        #endregion

        #region ctor

        public CompanyAntiCorruptionServiceWrapper(IWebClientHelper webClientHelper, ExternalHostAddressHelper
                                                                                                 addressHelper)
            : base(webClientHelper)
        {
            this.AddressHelper = addressHelper;
        }

        #endregion

        #region methods

        public override EnterprisePartyDto Get(int id)
        {
            var address = string.Concat(AddressHelper.GetBaseAddress(HostNameEnum.StorageSpace),
                                        "BasicInfo/Company/Get");
            var result = this.WebClientHelper.Get<EnterprisePartyDto>(new Uri(address));

            return result;
        }

        public override List<EnterprisePartyDto> Get(IList<int> IDs)
        {
            var address = string.Concat(this.AddressHelper.GetBaseAddress(HostNameEnum.StorageSpace),
                                        "basicinfo/Company/Get");

            var uri = new Uri(address);
            var result = this.WebClientHelper.Post<List<EnterprisePartyDto>, IList<int>>(uri, IDs);

            return result;
        }

        public override List<EnterprisePartyDto> GetAll()
        {
            var result = base.GetAll(new HostData()
                                         {
                                             Host = this.AddressHelper.GetBaseAddress(HostNameEnum.StorageSpace),
                                             Area = "BasicInfo"
                                         });

            return result;
        }

         public bool CanBePoGood(int goodId, int companyId)
         {
             var address = string.Concat(AddressHelper.GetBaseAddress(HostNameEnum.StorageSpace),
                                        "BasicInfo/Company/CanBePoGood?goodId=" + goodId + "&companyId=" + companyId);
             var result = this.WebClientHelper.Get<bool>(new Uri(address));

             return result;
         }

        #endregion

        

    }
}