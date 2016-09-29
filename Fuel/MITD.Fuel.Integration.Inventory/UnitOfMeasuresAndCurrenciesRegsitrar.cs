using MITD.CurrencyAndMeasurement.Domain.Model.UnitOfMeasures.Parsers;
using MITD.CurrencyAndMeasurement.Persistance.NH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBA.Data.NH;
using TIBA.Domain.Model;
using TIBA.Domain.Repository;

namespace MITD.Fuel.Integration.Inventory
{
    public class UnitOfMeasuresAndCurrenciesRegsitrar
    {
        public static void ReloadData()
        {
            var session = CurrencyAndMeasurementSession.GetSession();
            var uow = new NHUnitOfWork(session);
            var transaction = new NHTransactionHandler();
            var configurator = new AggregateRootConfigurator(new TIBA.Core.EventPublisher());
            //create an uomType before create uom            
            var nhExMap = new NHRepositoryExceptionMapper(new List<System.Tuple<DBActionType, string, Exception>>());

            var target = new UnitOfMeasureRepository(transaction, uow, nhExMap, configurator);

            UnitOfMeasureParserHelper.RegisterParsersFromRepository(target);
        }
    }
}
