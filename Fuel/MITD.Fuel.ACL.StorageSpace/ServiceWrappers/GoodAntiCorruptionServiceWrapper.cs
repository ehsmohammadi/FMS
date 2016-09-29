using System;
using System.Collections.Generic;
using MITD.Fuel.ACL.Contracts.ServiceWrappers;
using MITD.Fuel.Infrastructure.Service;
using MITD.Services.AntiCorruption;
using MITD.Services.Connectivity;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.ServiceWrappers
{
    public class GoodAntiCorruptionServiceWrapper : BaseAntiCorruptionServiceWrapper<GoodDto>, IGoodAntiCorruptionServiceWrapper
    {

        ExternalHostAddressHelper AddressHelper { get; set; }

        public GoodAntiCorruptionServiceWrapper(IWebClientHelper webClientHelper, ExternalHostAddressHelper
            addressHelper)
            : base(webClientHelper)
        {
            this.AddressHelper = addressHelper;
        }
        
       public override List<GoodDto> GetAll()
       {
           var address = string.Concat(this.AddressHelper.GetBaseAddress(HostNameEnum.StorageSpace),
               "basicinfo/Good/GetAll");
           var uri = new Uri(address);
           var result = this.WebClientHelper.Get<List<GoodDto>>(uri);

           return result;
       }

       public  List<GoodDto> GetAll(int companyId)
       {
           var address = string.Concat(this.AddressHelper.GetBaseAddress(HostNameEnum.StorageSpace),
               "basicinfo/Good/GetAll?companyId=" + companyId);
           var uri = new Uri(address);
           var result = this.WebClientHelper.Get<List<GoodDto>>(uri);

           return result;
       }
      
    }
}