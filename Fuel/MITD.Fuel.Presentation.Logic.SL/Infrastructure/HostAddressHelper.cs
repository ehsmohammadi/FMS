using System;
using System.Windows.Interop;

namespace MITD.Fuel.Presentation.Logic.SL.Infrastructure
{
    public static class HostAddressHelper
    {
        public static string GetHostingSiteBaseAddress(this Uri uri)
        {
            //var hostingSiteBaseAddress = string.Format("{0}://{1}:{2}", uri.Scheme, uri.Host, uri.Port);
            var hostingSiteBaseAddress = uri.AbsoluteUri.Replace(uri.AbsolutePath, string.Empty).Replace(uri.Query, string.Empty);
            return hostingSiteBaseAddress;
        }
    }
}