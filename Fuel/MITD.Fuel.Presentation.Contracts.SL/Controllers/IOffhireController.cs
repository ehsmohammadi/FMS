using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface IOffhireController
    {
        void ShowList();
        void ShowOffhireManagementSystemList();
        void Import(long referenceNumber);
        void Edit(OffhireDto offhireDto);
        void EditOffhireDetail(OffhireDto offhireDto, OffhireDetailDto offhireDetailDto);
    }
}
