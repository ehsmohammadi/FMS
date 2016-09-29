using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.DomainServices
{
    public class CharterOutDomainService : ICharterOutDomainService
    {

        #region Prop

        private ICharterOutRepository _charterOutRepository;
        private IVoyageRepository _voyageRepository;
        private IRepository<CharterItem> _charterItemRepository;
        private IVesselInCompanyRepository _vesselInCompanyRepository;
        #endregion

        #region ctor

        public CharterOutDomainService(ICharterOutRepository charterOutRepository, 
                                       IRepository<CharterItem> charterItemRepository,
                                       IVoyageRepository voyageRepository,
            IVesselInCompanyRepository vesselInCompanyRepository)
        {
            this._charterOutRepository = charterOutRepository;
            this._charterItemRepository = charterItemRepository;
            this._voyageRepository = voyageRepository;
            this._vesselInCompanyRepository = vesselInCompanyRepository;
        }

        #endregion


        #region Method
        public bool ExistCharterOutHeader(long charterId)
        {
            return this._charterOutRepository.GetById(charterId) != null;
        }

        public bool HasCharterEnd(long charterId)
        {
            return this._charterOutRepository.GetCharterEnd(charterId) != null;

        }
        #endregion

        public CharterOut GetCharterStartDateInternal(long id, long vesselInCompanyId, long ownerId)
        {
            return this._charterOutRepository.GetCharterStart(0, vesselInCompanyId, ownerId);
        }

        public CharterOut GetCharterStartDate(long id,long vesselInCompanyId, long ownerId)
        {
            var inVesel = _vesselInCompanyRepository.FindByKey(vesselInCompanyId);
            var outVessel =
                _vesselInCompanyRepository.Single(c => c.Vessel.Code == inVesel.Vessel.Code && c.Company.Id == ownerId);

            return this._charterOutRepository.GetCharterStart(id,outVessel.Id, ownerId);
        }
        public States GetCharterStartState(long id,long vesselInCompanyId, long ownerId)
        {
            return this._charterOutRepository.GetCharterStart(id,vesselInCompanyId, ownerId).CurrentState;
        }
        public bool CheckPeriodCharterInValid(CharterOut charterOut, long vesselInCompanyId, long ownerId, ICharterInRepository charterInRepository)
        {
            //    var res = charterInRepository.GetAll().Where(c => c.VesselInCompanyId == vesselInCompanyId
            //                                                      && c.OwnerId == ownerId).OrderBy(c => c.ActionDate).Last();
            //    if (b)
            //    {

            //    }
            return false;
        }

        public bool CheckNextCharterStartDate(long id,long vesselInCompanyId, long ownerId, DateTime actionDate)
        {
            if (_charterOutRepository.GetCharterStart(id,vesselInCompanyId, ownerId).CurrentState == States.Submitted)
                return true;
            
            var res = _charterOutRepository.GetAll().Where(c => c.CharterType == CharterType.Start
                                                                && c.VesselInCompanyId == vesselInCompanyId
                                                                && c.OwnerId == ownerId
                                                                && c.ActionDate > actionDate)
                .OrderBy(c => c.ActionDate)
                .FirstOrDefault();
            return (res != null);

        }

        public CharterOut GetNextCharterStart(long vesselInCompanyId, long ownerId, DateTime actionDate)
        {
           
            var res = _charterOutRepository.GetAll().OfType<CharterOut>().Where(
                                                                   c => c.CharterType == CharterType.Start
                                                                && c.VesselInCompanyId == vesselInCompanyId
                                                                && c.OwnerId == ownerId
                                                                && c.ActionDate > actionDate)
                .OrderBy(c => c.ActionDate)
                .LastOrDefault();
            return res ;

        }


        public bool HasCharterItem(long charterId)
        {
            return this._charterOutRepository.GetById(charterId).CharterItems.Count > 0;
        }


        public States GetCharterState(long id)
        {
            return _charterOutRepository.GetById(id).CurrentState;
        }
        public CharterOut GetCharterEnd(long charterstartId)
        {
            return this._charterOutRepository.GetCharterEnd(charterstartId);
        }

        public bool HasVesselCharterOutStart(long vesselInCompanyId)
        {
            var res = _charterOutRepository.GetAll().OfType<CharterOut>().Where(c => c.VesselInCompanyId == vesselInCompanyId
                                                            && c.CharterType == CharterType.Start
                                                            && c.CurrentState != States.Submitted).SingleOrDefault();
            return (res != null);
        }
        public bool HasVesselCharterOutStart(long id, long vesselInCompanyId, long ownerId)
        {
            var res = this._charterOutRepository.GetCharterStart(id,vesselInCompanyId, ownerId);
            return (!(res == null || res.CurrentState != States.Submitted));
        }

        public bool HasVesselCharterOutEnd(long vesselInCompanyId)
        {
            var res = _charterOutRepository.GetAll().OfType<CharterOut>().Where(c => c.VesselInCompanyId == vesselInCompanyId
                                                            && c.CharterType == CharterType.End
                                                            && c.CurrentState != States.Submitted).SingleOrDefault();
            return (res != null);
        }


        public bool IsLastCharter(long vesselInCompanyId, long id)
        {
            var res = _charterOutRepository.GetAll().OfType<CharterOut>().Where(c => c.CharterType == CharterType.End && c.VesselInCompanyId == vesselInCompanyId)
               .OrderByDescending(c => c.ActionDate).FirstOrDefault();

            return (id == res.Id);
        }

        public CharterOut GetDateEndLast(long vesselInCompanyId)
        {
            var res =
                _charterOutRepository.GetAll().OfType<CharterOut>().Where(
                  c => c.VesselInCompanyId == vesselInCompanyId
                    && c.CurrentState == States.Submitted
                    && c.CharterType == CharterType.End).
                    OrderByDescending(c => c.ActionDate).FirstOrDefault();
            return res;
        }
        public bool IsCharterStartDateGreaterThanPrevCharterEndDate(long vesselInCompanyId, DateTime dateTime)
        {
            var res = _charterOutRepository.GetAll().OfType<CharterOut>().Where(c => c.VesselInCompanyId == vesselInCompanyId
                                                                          && c.CurrentState == States.Submitted
                                                                          && c.CharterType == CharterType.End)
                 .OrderByDescending(c => c.ActionDate).FirstOrDefault();
            return (res == null) ? true : (res.ActionDate < dateTime);
            ;
        }

        public CharterOut GetCharterOutStart(long vesselInCompanyId, long ownerId, DateTime charterInStartDateTime)
        {

            var inVesel = _vesselInCompanyRepository.FindByKey(vesselInCompanyId);
            var outVessel =
                _vesselInCompanyRepository.Single(c => c.Vessel.Code == inVesel.Vessel.Code && c.Company.Id == ownerId);


           
            
            var res = _charterOutRepository.GetAll().OfType<CharterOut>().OrderByDescending(c=>c.ActionDate).FirstOrDefault(c =>
                                                                                                  
                                                                                                  c.VesselInCompanyId == outVessel.Id
                                                                                               && c.OwnerId == ownerId
                                                                                               && c.CurrentState == States.Submitted
                                                                                               && c.CharterType == CharterType.Start
                                                                                               && c.ActionDate <= charterInStartDateTime);
            return res;
        }



        public CharterOut GetCharterInStart(long Id)
        {
            var res = _charterOutRepository.GetAll().OfType<CharterOut>().SingleOrDefault(c => c.Id == Id);
            return res;
        }

        public void DeleteCharterItem(long id)
        {
            var res = this._charterItemRepository.Find(c => c.Id == id).SingleOrDefault();
            if (res != null)
            {
                this._charterItemRepository.Delete(res);
            }
        }


        public Voyage GetVoyageCharterInStart(long companyId, long vesselInCompanyId, DateTime dateTime)
        {
            var res = _voyageRepository.Find(
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






      
    }
}
