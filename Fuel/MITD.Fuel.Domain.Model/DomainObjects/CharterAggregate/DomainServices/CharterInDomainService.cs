using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.DomainServices
{
    public class CharterInDomainService : ICharterInDomainService
    {

        #region Prop

        private ICharterInRepository _charterInRepository;
        private ICharterItemHistoryRepository _charterItemHistoryRepository;
        private IVoyageRepository _voyageRepository;
        private IRepository<CharterItem> _charterItemRepository;
        private IVesselInCompanyRepository _vesselInCompanyRepository;

        #endregion

        #region ctor

        public CharterInDomainService(ICharterInRepository charterInRepository,
            IRepository<CharterItem> charterItemRepository,
            IVoyageRepository voyageRepository,IVesselInCompanyRepository vesselInCompanyRepository,ICharterItemHistoryRepository itemHistoryRepository)
        {
            this._voyageRepository = voyageRepository;
            this._charterInRepository = charterInRepository;
            this._charterItemRepository = charterItemRepository;
            this._vesselInCompanyRepository = vesselInCompanyRepository;
            this._charterItemHistoryRepository = itemHistoryRepository;
        }

        #endregion


        #region Method
        public bool ExistCharterInHeader(long charterId)
        {
            
            return this._charterInRepository.GetById(charterId)!=null;
        }

        public bool HasCharterEnd(long charterId)
        {
            
            return this._charterInRepository.GetCharterEnd(charterId)!=null;
        }
        #endregion



        public DateTime GetCharterStartDate(long vesselInCompanyId,long chartereId)
        {
            return this._charterInRepository.GetCharterStart(vesselInCompanyId, chartereId).ActionDate;
        }

        public CharterIn GetCharterEnd(long charterstartId)
        {
            return  this._charterInRepository.GetCharterEnd(charterstartId);
            
        }

        public States GetCharterState(long id)
        {
            return _charterInRepository.GetById(id).CurrentState;
        }

        public bool HasCharterItem(long charterId)
        {
            return this._charterInRepository.GetById(charterId).CharterItems.Count > 0;
        }


        public bool HasVesselCharterInStart(long vesselInCompanyId)
        {
            var res = _charterInRepository.GetAll().OfType<CharterIn>().Where(c => c.VesselInCompanyId == vesselInCompanyId
                                                            && c.CharterType == CharterType.Start
                                                            && c.CurrentState != States.Submitted);
            return (res.Count() >=1);
        }

        public bool HasVesselCharterInStart(long vesselInCompanyId,long chatererId)
        {
            var res = this._charterInRepository.GetCharterStart(vesselInCompanyId, chatererId);
            return (!(res == null || res.CurrentState != States.Submitted));
        }


        public bool HasVesselCharterInEnd(long vesselInCompanyId)
        {
            var res = _charterInRepository.GetAll().OfType<CharterIn>().Where(c => c.VesselInCompanyId == vesselInCompanyId
                                                            && c.CharterType == CharterType.End
                                                            && c.CurrentState != States.Submitted
                                                          ).SingleOrDefault();
         
            return (res != null);
        }



        public bool IsLastCharter(long vesselInCompanyId,long id)
        {
            var res = _charterInRepository.GetAll().OfType<CharterIn>().Where(c => c.CharterType == CharterType.End && c.VesselInCompanyId == vesselInCompanyId)
                .OrderBy(c => c.ActionDate).Last();
            return (id == res.Id);
        }

        public CharterIn GetCharterInPrevCharterOut(long vesselInCompanyId,DateTime dateCharterOutStart)
        {
            var res = _charterInRepository.GetAll().OfType<CharterIn>()
                .Where(c => c.VesselInCompanyId == vesselInCompanyId
                         && c.ActionDate < dateCharterOutStart
                         && c.CurrentState == States.Submitted)
                             .OrderByDescending(c => c.ActionDate).FirstOrDefault();
            return res;
        }

       public bool IsCharterStartDateGreaterThanPrevCharterEndDate(long vesselInCompanyId,DateTime dateTime)
       {
           var res = _charterInRepository.GetAll().OfType<CharterIn>().Where(c => c.VesselInCompanyId == vesselInCompanyId
                                                                         && c.CurrentState == States.Submitted
                                                                         && c.CharterType==CharterType.End)
                .OrderByDescending(c => c.ActionDate).FirstOrDefault();
           return (res == null) ? true : (res.ActionDate < dateTime);
           ;
       }



       public CharterIn GetCharterInStart(long vesselInCompanyId, long ownerId, DateTime charterOutStartDateTime)
       {
           var outVessel = _vesselInCompanyRepository.FindByKey(vesselInCompanyId);
           var inVessel =
               _vesselInCompanyRepository.Single(c => c.Vessel.Code == outVessel.Vessel.Code && c.Company.Id == ownerId);

           var res = _charterInRepository.GetAll().OfType<CharterIn>().SingleOrDefault(c => c.VesselInCompanyId == inVessel.Vessel.Id
                                                                                              && c.OwnerId == ownerId
                                                                                              && c.CurrentState == States.Submitted
                                                                                              && c.CharterType == CharterType.Start
                                                                                              && c.ActionDate >= charterOutStartDateTime);
           return res;
       }


       public CharterIn GetCharterInStart(long Id)
       {
           var res = _charterInRepository.GetAll().OfType<CharterIn>().SingleOrDefault(c => c.Id == Id);
           return res;
       }


       public States GetCharterStartState(long vesselInCompanyId, long chartereId)
       {
           return this._charterInRepository.GetCharterStart(vesselInCompanyId, chartereId).CurrentState;
       }


       public void DeleteCharterItem(long id)
       {
           var res = this._charterItemRepository.Find(c => c.Id == id).SingleOrDefault();
           if (res!=null)
           {
               this._charterItemRepository.Delete(res);
           }
       }

        public Voyage GetVoyageCharterInStart(long companyId,long vesselInCompanyId,DateTime dateTime)
        {
          var res=  _voyageRepository.Find(
                c => c.CompanyId == companyId && c.VesselInCompanyId == vesselInCompanyId && c.StartDate < dateTime)
                .OrderByDescending(d => d.Id).FirstOrDefault();
            return res;
        }

        
       
        public Voyage GetVoyageCharterInEnd(long companyId, long vesselInCompanyId, DateTime dateTime)
        {
            var res = _voyageRepository.Find(
                 c => c.CompanyId == companyId && c.VesselInCompanyId == vesselInCompanyId && c.EndDate < dateTime)
                 .OrderByDescending(d => d.Id).FirstOrDefault();
            return res;
        }


        public bool IsLastCharterInStartWithOutChaterInEnd(long charterId,long companyId, long vesselInCompanyId)
        {
           
            var fetchstartegy =
                new ListFetchStrategy<Charter>(MITD.Domain.Repository.Enums.FetchInUnitOfWorkOption.NoTracking).OrderByDescending(
                    c => c.ActionDate);

           

            var res = _charterInRepository.GetAll(fetchstartegy).OfType<CharterIn>().
                FirstOrDefault(c => c.Id !=  charterId &&
                      c.CharterType==CharterType.Start &&
                      c.VesselInCompanyId==vesselInCompanyId &&
                      c.ChartererId==companyId);
            if (res != null) return false;
            var res1 = _charterInRepository.GetCharterEnd(charterId);
            return res1 == null;
        }


        public void AddCharterItemHistory(CharterItemHistory charterItemHistory)
        {
            _charterItemHistoryRepository.Add(charterItemHistory);
        }

        public List<CharterItemHistory> GetCharterItemHistories(long charterId, States states)
        {

            //CharterIn charterIn=new CharterIn();
            //var listHistory = new List<CharterItemHistory>();

            
         var res=   _charterItemHistoryRepository.GetAll()
                .Where(
                    c =>
                        c.CharterId == charterId && c.CharterStateId == (int)states).Select(d => d);


            //charterIn.CharterItems.ForEach(d => listHistory.AddRange(_charterItemHistoryRepository.GetAll()
            //    .Where(c => c.CharterId == charterId && c.CharterItemId == d.Id).Select(e=>e)));

            //var submitedListHistory =
            //    listHistory.OrderBy(c => c.Id)
            //        .LastOrDefault(c => c.CharterStateId == 1);
           


            //listHistory.ForEach(d => res.Add( _charterItemHistoryRepository.GetAll()
            //    .Where(
            //        c =>
            //            c.CharterId == d.CharterId && c.CharterItemId == d.CharterItemId && c.Id > d.Id &&
            //            c.CharterStateId == 4).OrderBy(c=>c.Id).LastOrDefault()));

            return res.ToList();


        }

      


        public bool UpdateCharterItemHistory(long charterId, long charterItemId, DateTime date)
        {
            var res = _charterItemHistoryRepository.GetAll().SingleOrDefault(c => c.CharterId == charterId &&
                                                                                  c.CharterItemId == charterItemId &&
                                                                                  c.DateRegisterd == date);
            if(res != null) res.CharterStateId = 1;
            return res != null;
        }
    }
}
