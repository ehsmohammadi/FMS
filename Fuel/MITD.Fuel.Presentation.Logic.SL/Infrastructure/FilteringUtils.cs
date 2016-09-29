using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Logic.SL.Infrastructure
{
    public class FilteringUtils
    {
        public static readonly CompanyDto EmptyCompanyDto;
        public static readonly VesselInCompanyDto EmptyVesselDto;
        public static readonly UserDto EmptyUserDto;

        static FilteringUtils()
        {
            EmptyVesselDto = new VesselInCompanyDto()
            {
                Id = -1,
                Code = string.Empty,
                Name = "-",
                Description = "-",
            };

            EmptyCompanyDto = new CompanyDto()
                                   {
                                       Id = -1,
                                       Code = string.Empty,
                                       Name = "-",
                                       VesselInCompanies = new List<VesselInCompanyDto>() { EmptyVesselDto }
                                   };

            EmptyUserDto = new UserDto()
                           {
                               Id = -1,
                               Code = string.Empty,
                               CompanyDto = EmptyCompanyDto,
                               FirstName = "-",
                               LastName = "-",
                               UserName = "-"
                           };
        }

    }
}
