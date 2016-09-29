using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class IsFuelReportDetailFuelTypeValid : SpecificationBase<FuelReport>
    {
        public IsFuelReportDetailFuelTypeValid(IGoodDomainService goodDomainService)
            : base(
            frd =>
                goodDomainService.
                    GetMandatoryVesselFuels(frd.FuelReport.VesselId, frd.FuelReport.ReportDate).
                        Any(g => g.Id == frd.FuelTypeId))
        {

        }
    }
}
