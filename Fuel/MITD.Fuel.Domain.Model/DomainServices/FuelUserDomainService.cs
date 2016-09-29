using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.DomainServices
{
    public class FuelUserDomainService : IFuelUserDomainService
    {
        private readonly IFuelUserRepository fuelUserRepository;

        public FuelUserDomainService(IFuelUserRepository fuelUserRepository)
        {
            this.fuelUserRepository = fuelUserRepository;
        }

        public long GetCurrentFuelUserId()
        {
            var currentUserId = GetCurrentUserId();
            var currentCompanyId = GetCurrentUserCompanyId();

            return fuelUserRepository.Single(fu => fu.IdentityId == currentUserId && fu.CompanyId == currentCompanyId).Id;
        }

        public static long GetCurrentUserId()
        {
            return long.Parse(ClaimsPrincipal.Current.Claims.First(c => c.Type == "CurrentUserId").Value);
        }

        public static long GetCurrentUserCompanyId()
        {
            return long.Parse(ClaimsPrincipal.Current.Claims.First(c => c.Type == "CurrentUserCompanyId").Value);
        }

        public static List<long> GetCurrentUserCompanyIds()
        {
            return new List<long>()
                   {
                       long.Parse(ClaimsPrincipal.Current.Claims.First(c => c.Type == "CurrentUserCompanyId").Value)
                   };
        }
    }
}
