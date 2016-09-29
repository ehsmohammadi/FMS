using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.StorageSpace.OffhireService;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Integration.Offhire;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices
{
    public class OffhireManagementSystemDomainService : IOffhireManagementSystemDomainService
    {
        private readonly OffhireService.IOffhireManagementService offhireManagementService = new OffhireManagementServiceClient();

        private readonly IRepository<ActivityLocation> activityLocationRepository;
        //private readonly IActivityLocationDomainService activityLocationDomainService;
        private readonly IGoodDomainService goodDomainService;
        private readonly IRepository<Good> goodRepository;
        private readonly IRepository<GoodUnit> companyGoodUnitRepository;
        private readonly IGoodUnitDomainService goodUnitDomainService;
        private readonly ITankDomainService tankDomainService;
        private readonly IVesselInCompanyDomainService vesselDomainService;

        public OffhireManagementSystemDomainService(
            IRepository<ActivityLocation> activityLocationRepository,
            //IActivityLocationDomainService activityLocationDomainService,
            IGoodDomainService goodDomainService,
            IGoodUnitDomainService goodUnitDomainService,
            ITankDomainService tankDomainService,
            IVesselInCompanyDomainService vesselDomainService, IRepository<Good> goodRepository, IRepository<GoodUnit> companyGoodUnitRepository)
        {
            //this.activityLocationDomainService = activityLocationDomainService;
            this.activityLocationRepository = activityLocationRepository;
            this.goodDomainService = goodDomainService;
            this.goodUnitDomainService = goodUnitDomainService;
            this.tankDomainService = tankDomainService;
            this.vesselDomainService = vesselDomainService;
            this.goodRepository = goodRepository;
            this.companyGoodUnitRepository = companyGoodUnitRepository;
        }

        public OffhireManagementSystemEntity GetFinalizedOffhire(long referenceNumber, long companyId)
        {
            var offhireData = offhireManagementService.GetFinalizedOffhire(referenceNumber);

            var offhireSystemEntity = mapOffhireDataToOffhireSystemEntity(offhireData, companyId);

            return offhireSystemEntity;
        }

        private OffhireManagementSystemEntity mapOffhireDataToOffhireSystemEntity(OffhireService.OffhireData offhireData, long companyId)
        {
            //Hard Coded
            //offhireData.StartDateTime = new DateTime(2014, 7, 16, 12,0,0);
            //offhireData.EndDateTime = new DateTime(2014, 7, 16, 13,0,0);

            var location = activityLocationRepository.First(l => l.Code == offhireData.Location);
            if (location == null)
            {
                //It must be logged that the specified location was not found.
                return null;
            }

            var vessleInCompany = vesselDomainService.GetVesselInCompany(companyId, offhireData.VesselCode);

            if (vessleInCompany == null)
            {
                //It must be logged that the specified VesselCode In Company was not found.
                return null;
            }

            var entity = new OffhireManagementSystemEntity()
                        {
                            VesselInCompany = vessleInCompany,
                            Location = location,
                            StartDateTime = offhireData.StartDateTime,
                            EndDateTime = offhireData.EndDateTime,
                            HasVoucher = offhireData.HasVoucher,
                            ReferenceNumber = offhireData.ReferenceNumber
                        };

            entity.OffhireDetails = new List<OffhireManagementSystemEntityDetail>();

            foreach (var offhireDataItem in offhireData.OffhireDetails)
            {
                if (offhireDataItem.Quantity == 0) continue;

                var entityDetail = mapOffhireDataDetailToOffhireManagementSystemEntityDetail(offhireDataItem, companyId, vessleInCompany);

                if (entityDetail == null)
                {
                    //It must be logged that the offhireDataItem could not be mapped.

                    return null;
                }

                entity.OffhireDetails.Add(entityDetail);
            }

            return entity;
        }

        private OffhireManagementSystemEntityDetail mapOffhireDataDetailToOffhireManagementSystemEntityDetail(OffhireService.OffhireDataItem dataItem, long companyId, VesselInCompany vesselInCompany)
        {
            Good good = mapOffhireDataGoodToOffhireSystemEntity(dataItem.FuelType, companyId);

            if (good == null)
            {
                //It must be logged that the specified good was not found.
                return null;
            }


            GoodUnit goodUnit = mapOffhireDataMeasureUnitToOffhireSystemEntityMeasureUnit(good, dataItem.UnitTypeCode);

            if (goodUnit == null)
            {
                //It must be logged that the specified good unit was not found.

                return null;
            }

            var detail = new OffhireManagementSystemEntityDetail
                  {
                      Good = good,
                      Unit = goodUnit,
                      Tank = vesselInCompany.Tanks[0],
                      QuantityAmount = dataItem.Quantity
                  };

            return detail;
        }

        private GoodUnit mapOffhireDataMeasureUnitToOffhireSystemEntityMeasureUnit(Good good, FuelUnitType fuelUnitType)
        {
            var unitTypeCode = OffhireAssetsToFuelAssetsMapper.GetFuelMeasureCode(fuelUnitType.ToString());

            var goodUnit = good.GoodUnits.Single(gu => gu.Abbreviation == unitTypeCode);

            return goodUnit;
        }

        private Good mapOffhireDataGoodToOffhireSystemEntity(FuelType fuelType, long companyId)
        {
            var goodCode = OffhireAssetsToFuelAssetsMapper.GetFuelGoodCode(fuelType.ToString());

            var good = goodRepository.Single(g => g.CompanyId == companyId && g.Code == goodCode);

            return good;
        }

        //public Offhire GetFinalizedOffhireWithVoucherRegistered(string referenceNumber)
        //{
        //    return this.offhires.FirstOrDefault(o => o.ReferenceNumber == referenceNumber);
        //}

        public List<OffhireManagementSystemEntity> GetFinalizedOffhires(long companyId, long vesselInCompanyId, DateTime? fromDate, DateTime? toDate)
        {
            var vesselInCompany = vesselDomainService.Get(vesselInCompanyId);

            if (vesselInCompany == null)
            {
                //It must be logged that the specified VesselCode In Company was not found.
                return null;
            }

            var finalizedOffhires = offhireManagementService.GetFinalizedOffhires(vesselInCompany.Code, fromDate, toDate);

            var result = new List<OffhireManagementSystemEntity>();

            foreach (var finalizedOffhire in finalizedOffhires)
            {
                var offhireSystemEntity = mapOffhireDataToOffhireSystemEntity(finalizedOffhire, companyId);
                if (offhireSystemEntity != null)
                {
                    result.Add(offhireSystemEntity);
                }
            }

            return result;
        }
    }
}
