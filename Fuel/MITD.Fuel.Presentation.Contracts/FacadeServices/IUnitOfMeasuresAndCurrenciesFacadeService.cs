using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
    public interface IUnitOfMeasuresAndCurrenciesFacadeService : IFacadeService
    {
        void Reload();
    }
}
