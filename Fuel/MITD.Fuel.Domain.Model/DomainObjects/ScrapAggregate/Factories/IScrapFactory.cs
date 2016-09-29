using System;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.Factories;

namespace MITD.Fuel.Domain.Model.DomainObjects.Factories
{
    public interface IScrapFactory : IFactory
    {
        Scrap CreateScrap(VesselInCompany vesselInCompany, Company secondParty, DateTime scrapDate, List<ScrapDetail> scrapDetails, long userId);

        Scrap CreateScrap(VesselInCompany vesselInCompany, Company secondParty, DateTime scrapDate, long userId);

        ScrapDetail CreateScrapDetail(Scrap scrap, double rob, double price, Currency currency, Good good, GoodUnit unit, Tank tank);

    }
}