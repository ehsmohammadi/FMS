using System;
using MITD.Fuel.ACL.Contracts.ServiceWrappers;
using MITD.Fuel.Infrastructure.Service;
using MITD.Services.AntiCorruption;
using MITD.Services.Connectivity;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.ServiceWrappers
{
    public class GoodAssignmentAntiCorruptionServiceWrapper : BaseAntiCorruptionServiceWrapper<GoodDto>, IGoodPartyAssignmentAntiCorruptionServiceWrapper
    {
        ExternalHostAddressHelper AddressHelper { get; set; }
        public GoodAssignmentAntiCorruptionServiceWrapper(IWebClientHelper webClientHelper, ExternalHostAddressHelper
            addressHelper)
            : base(webClientHelper)
        {
            this.AddressHelper = addressHelper;
        }

        public bool IsNotOverMaximumOrder(double quantity, int company,int goodId, int wareHouseId)
        {
           
    
           var address = string.Concat(this.AddressHelper.GetBaseAddress(HostNameEnum.StorageSpace),
               "basicinfo/GoodAssignment/IsNotOverMaximumOrder?quantity=" + quantity + "&company="+company+"&GoodId=" + goodId + "&wareHouseId=" + wareHouseId);
           var uri = new Uri(address);
           var result = this.WebClientHelper.Get<bool>(uri);
            return result;

        }


        public bool IsEqualFixOrder(double quantity, int company, int goodId, int wareHouseId)
        {
            var address = string.Concat(this.AddressHelper.GetBaseAddress(HostNameEnum.StorageSpace),
                "basicinfo/GoodAssignment/IsEqualFixOrder?quantity=" + quantity + "&company=" + company + "&GoodId=" + goodId + "&wareHouseId=" + wareHouseId);
            var uri = new Uri(address);
            var result = this.WebClientHelper.Get<bool>(uri);
            return result;
        }


        public bool CanBeOrderWithReOrderLevelCheck(int company, int goodId, int wareHouseId)
        {
            var address = string.Concat(this.AddressHelper.GetBaseAddress(HostNameEnum.StorageSpace),
                 "basicinfo/GoodAssignment/CanBeOrderWithReOrderLevelCheck?company=" + company + "&GoodId=" + goodId + "&wareHouseId=" + wareHouseId);
            var uri = new Uri(address);
            var result = this.WebClientHelper.Get<bool>(uri);
            return result;
        }
    }
}