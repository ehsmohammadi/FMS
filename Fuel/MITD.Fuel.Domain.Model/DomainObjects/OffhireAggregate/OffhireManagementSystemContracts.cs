using System;
using System.Collections.Generic;
namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class OffhireData
    {
        /// <summary>
        /// Offhire Refrence for future tracking.
        /// </summary>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Port Code
        /// </summary>
        public string Location { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// 4 digits Vessel Code
        /// </summary>
        public string VesselCode { get; set; }

        /// <summary>
        /// Is any voucher registered for mentioned offhire.
        /// </summary>
        public bool HasVoucher { get; set; }

        public List<OffhireDataItem> OffhireDetails { get; set; }
    }

    public class OffhireDataItem
    {
        public decimal Quantity { get; set; }
        public string FuelTypeCode { get; set; }
        public string UnitTypeCode { get; set; }

        /// <summary>
        /// Not mandatory.
        /// </summary>
        public string TankCode { get; set; }
    }

    public interface IOffhireManagementSystemData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceNumber">Offhire Refrence Number</param>
        /// <returns>Single Result.</returns>
        OffhireData GetFinalizedOffhire(string referenceNumber);

        /// <summary>
        /// List of offhires filtered by Vessel Code and Given Dates (Optional).
        /// </summary>
        /// <param name="vesselCode">Required</param>
        /// <param name="fromDate">Optional</param>
        /// <param name="toDate">Optional</param>
        /// <returns></returns>
        List<OffhireData> GetFinalizedOffhires(string vesselCode, DateTime? fromDate, DateTime? toDate);
    }
}