using MITD.Fuel.Integration.Inventory;
using MITD.Fuel.Presentation.Contracts.FacadeServices;

namespace MITD.Fuel.Application.Facade
{
    public class UnitOfMeasuresAndCurrenciesFacadeService : IUnitOfMeasuresAndCurrenciesFacadeService
    {
        public void Reload()
        {
            UnitOfMeasuresAndCurrenciesRegsitrar.ReloadData();
        }
    }
}
