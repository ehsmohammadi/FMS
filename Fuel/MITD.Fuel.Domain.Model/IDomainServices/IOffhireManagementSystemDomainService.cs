using System;
using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IOffhireManagementSystemDomainService : IDomainService
    {
        OffhireManagementSystemEntity GetFinalizedOffhire(long referenceNumber, long companyId);
        //Offhire GetFinalizedOffhireWithVoucherRegistered(string referenceNumber);
        List<OffhireManagementSystemEntity> GetFinalizedOffhires(long companyId, long vesselInCompanyId, DateTime? fromDate, DateTime? toDate);
    }
}