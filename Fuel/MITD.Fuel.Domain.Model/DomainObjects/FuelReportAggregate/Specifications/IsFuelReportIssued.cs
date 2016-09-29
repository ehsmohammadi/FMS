using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class IsFuelReportIssued : SpecificationBase<FuelReport>
    {
        public IsFuelReportIssued(IInventoryOperationDomainService inventoryOperationDomainService)
            : base(
                fuelReport =>
                    fuelReport.State == States.Submitted &&
                    (inventoryOperationDomainService.
                        GetFuelReportInventoryOperations(fuelReport).
                            Count(inv => inv.ActionType == InventoryActionType.Issue) > 0) 
            )
        {

        }
    }
}
