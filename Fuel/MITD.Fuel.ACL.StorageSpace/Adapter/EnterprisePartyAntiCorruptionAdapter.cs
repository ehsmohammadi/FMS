using MITD.Fuel.ACL.Contracts.Adapters;
using MITD.Fuel.ACL.Contracts.ServiceWrappers;
using MITD.Services.AntiCorruption;
using MITD.Services.AntiCorruption.Contracts;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.Adapter
{
    public class CompanyAntiCorruptionAdapter : BaseAntiCorruptionAdapter<Domain.Model.DomainObjects.Company, EnterprisePartyDto>, ICompanyAntiCorruptionAdapter
    {
        public ICompanyAntiCorruptionServiceWrapper CompanyServiceWrapper { get; set; }

        public CompanyAntiCorruptionAdapter(ICompanyAntiCorruptionServiceWrapper serviceWrapper, IAntiCorruptionMapper<Domain.Model.DomainObjects.Company, EnterprisePartyDto> mapper)
            : base(serviceWrapper, mapper)
        {
            this.CompanyServiceWrapper = serviceWrapper;
        }
        
        public bool CanBePoGood(int goodId, int companyId)
        {
           return CompanyServiceWrapper.CanBePoGood(goodId, companyId);
        }

      
    }
}