using System;
using System.Collections.ObjectModel;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class CharterPreparedData
    {
        public Company Charterer { get; set; }

        public Company Owner { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public VesselInCompany VesselInCompany { get; set; }

        public ObservableCollection<CharterPreparedDataItem> CharterItems { get; set; }

        public CharterType CharterState { get; set; }

        public CharteringType CharteringType { get; set; }
    }

    public class CharterPreparedDataItem
    {
        public decimal Rob { get; set; }

        public Good Good { get; set; }

        public GoodUnit Unit { get; set; }

        public Tank Tank { get; set; }

        public long TankId { get; set; }

    }
}
