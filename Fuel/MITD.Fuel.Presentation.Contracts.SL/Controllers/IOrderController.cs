using System.Collections.Generic;
using System.Collections.ObjectModel;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface IOrderController
    {
        void Add(List<CompanyDto> dtos, List<VesselInCompanyDto> vesselInCompanyDtos);
        void Edit(OrderDto dto, List<CompanyDto> dtos, List<VesselInCompanyDto> vesselInCompanyDtos);
        void ShowList();
        void ViewAssignedReferences(OrderAssignementReferenceTypeEnum destinationType, OrderDto orderDto);
    }
}
