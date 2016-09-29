using System;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace MITD.Fuel.Presentation.Contracts.SL.Infrastructure
{
    public class ApiServiceAddressHelper
    {
        public static String GetServiceAddress(AreaEnum areaOfService, ApiServicesEnum apiService)
        {
            return String.Format("{0}://{1}:{2}/apiarea/" + areaOfService.ToString() + "/" + apiService.ToString() + "/",
           Application.Current.Host.Source.Scheme,
           Application.Current.Host.Source.Host,
           Application.Current.Host.Source.Port);
        }

        public static Uri BuildUri(string url)
        {
            return new Uri(url, UriKind.Absolute);
        }

        public static Uri BuildUri(StringBuilder sbUrl)
        {
            return BuildUri(sbUrl.ToString());
        }
    }
}
