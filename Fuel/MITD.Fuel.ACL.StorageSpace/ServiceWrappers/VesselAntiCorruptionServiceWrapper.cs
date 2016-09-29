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
    public class VesselAntiCorruptionServiceWrapper : BaseAntiCorruptionServiceWrapper<WarehouseDto>, IVesselAntiCorruptionServiceWrapper
    {
        #region props

        private ExternalHostAddressHelper AddressHelper { get; set; }

        #endregion

        #region ctor

        public VesselAntiCorruptionServiceWrapper(IWebClientHelper webClientHelper, ExternalHostAddressHelper
                                                                                                 addressHelper)
            : base(webClientHelper)
        {
            this.AddressHelper = addressHelper;
        }

        #endregion

        #region methods

        public override WarehouseDto Get(int id)
        {
            var address = string.Concat(AddressHelper.GetBaseAddress(HostNameEnum.StorageSpace),
                                        "BasicInfo/Warehouse/Get");
            var result = this.WebClientHelper.Get<WarehouseDto>(new Uri(address));

            return result;
        }

        public override List<WarehouseDto> Get(IList<int> IDs)
        {
            var address = string.Concat(this.AddressHelper.GetBaseAddress(HostNameEnum.StorageSpace),
                                        "BasicInfo/Warehouse/Get");

            var uri = new Uri(address);
            var result = this.WebClientHelper.Post<List<WarehouseDto>, IList<int>>(uri, IDs);

            return result;
        }

        public override List<WarehouseDto> GetAll()
        {
            var result = base.GetAll(new HostData()
                                         {
                                             Host = this.AddressHelper.GetBaseAddress(HostNameEnum.StorageSpace),
                                             Area = "BasicInfo"
                                         });

            return result;
        }

        #endregion

        

    }
}
