using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Integration.Offhire.Data;

namespace MITD.Fuel.Integration.Offhire
{
    public class OffhireAssetsToFuelAssetsMapper
    {
        private static readonly OffhireSystemToFuelSystemMappingDataContext context = new OffhireSystemToFuelSystemMappingDataContext();

        public static string GetFuelGoodCode(string offhireFuelTypeCode)
        {
            var mapping = context.OffhireFuelTypeFuelGoodCodes.FirstOrDefault(
                m => m.OffhireFuelType == offhireFuelTypeCode &&
                    (m.ActiveFrom == null || m.ActiveFrom <= DateTime.Now) &&
                    (m.ActiveTo == null || m.ActiveTo >= DateTime.Now));

            if (mapping != null)
                return mapping.FuelGoodCode;

            return null;
        }

        public static string GetFuelMeasureCode(string offhireMeasureTypeCode)
        {
            var mapping = context.OffhireMeasureTypeFuelMeasureCodes.FirstOrDefault(
                m => m.OffhireMeasureType == offhireMeasureTypeCode &&
                    (m.ActiveFrom == null || m.ActiveFrom <= DateTime.Now) &&
                    (m.ActiveTo == null || m.ActiveTo >= DateTime.Now));

            if (mapping != null)
                return mapping.FuelMeasureCode;

            return null;
        }
    }
}
