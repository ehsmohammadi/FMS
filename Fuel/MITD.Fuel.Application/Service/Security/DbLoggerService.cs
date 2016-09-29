using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using MITD.Domain.Repository;
using MITD.FuelSecurity.Domain.Model;
using MITD.FuelSecurity.Domain.Model.Repository;
using MITD.FuelSecurity.Domain.Model.Service;

namespace MITD.Fuel.Application.Service.Security
{
  public  class DbLoggerService:ILoggerService
    {
      private readonly ILogRepository logRepository;

      private IUnitOfWorkScope _unitOfWorkScope;

      public DbLoggerService(ILogRepository logRepository, IUnitOfWorkScope unitOfWorkScope)
        {
            this.logRepository = logRepository;
          this._unitOfWorkScope = unitOfWorkScope;
        }

        public void AddLog(Log log)
        {
            using (var scope = new TransactionScope())
            {
              
                logRepository.Add(log);
                _unitOfWorkScope.Commit();
                scope.Complete();
            }
        }

        public IList<Log> GetAll()
        {
            return new List<Log>(); //logRepository.GetAllLogs();
        }

        public Log GetLogById(long logId)
        {

            throw new Exception();
          //  return logRepository.GetLogById(logId);
        }

        public void DeleteLog(Log log)
        {
            throw new Exception();
            //using (var scope = new TransactionScope())
            //{
            //    logRepository.Delete(log);
            //    scope.Complete();
            //}
        }
    }
}
