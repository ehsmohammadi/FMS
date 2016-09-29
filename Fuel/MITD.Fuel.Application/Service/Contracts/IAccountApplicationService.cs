using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Services.Application;

namespace MITD.Fuel.Application.Service.Contracts
{
    public interface IAccountApplicationService:IApplicationService
    {
        void Add(Account account);
    }
}
