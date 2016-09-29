using System;

namespace MITD.Fuel.Infrastructure.Service
{
    public enum HostNameEnum
    {
        StorageSpace
    }

    public class ExternalHostAddressHelper
    {
         public string GetBaseAddress(HostNameEnum host)
         {
             if (host == HostNameEnum.StorageSpace)
                 return System.Configuration.ConfigurationManager.AppSettings["WebApiStorageSpace"].ToString()+"apiarea/";
             
             throw new NotImplementedException();
         }
    }
}