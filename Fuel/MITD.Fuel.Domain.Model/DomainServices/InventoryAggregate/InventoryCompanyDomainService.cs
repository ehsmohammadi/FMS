using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Factories;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.Specifications;

namespace MITD.Fuel.Domain.Model.DomainServices
{
    public class InventoryCompanyDomainService : IInventoryCompanyDomainService
    {
        private readonly IRepository<Inventory_Company> companyRepository;
        private readonly IRepository<Inventory_Warehouse> warehouseRepository;

        public InventoryCompanyDomainService(IRepository<Inventory_Company> companyRepository,
            IRepository<Inventory_Warehouse> warehouseRepository)
        {
            this.companyRepository = companyRepository;
            this.warehouseRepository = warehouseRepository;

        }

        //================================================================================

        public Inventory_Company Get(long id)
        {
            var company = companyRepository.Single(e => e.Id == id);

            if (company == null)
                throw new ObjectNotFound("company", id);

            return company;
        }

        //================================================================================

        public List<Inventory_Company> Get()
        {
            //var fetchStrategy = new ListFetchStrategy<Inventory_Company>()
            //    .Include(t => t.Inventory_Warehouse);

            //companyRepository.GetAll(fetchStrategy);

            //return fetchStrategy.
            var company = companyRepository.GetAll();

            return company.ToList();
        }

        public List<Inventory_Company> GetCurrentUserCompanies()
        {
            var userCompayIds = FuelUserDomainService.GetCurrentUserCompanyIds();
            return companyRepository.Find(c => userCompayIds.Any(uc=>uc == c.Id)).ToList();// GetAll().ToList();
        }

        //================================================================================

        public Inventory_Warehouse Get(long companyId, long warehouseId)
        {
            var warehouse = warehouseRepository.Single(e => e.CompanyId == companyId && e.Id == warehouseId);

            if (warehouse == null)
                throw new ObjectNotFound("warehouse", warehouseId);

            return warehouse;
        }

        //================================================================================

        public List<Inventory_Warehouse> GetWarehouse(long companyId)
        {

            var warehouse = warehouseRepository.Find(w => w.CompanyId == companyId && w.Inventory_Transaction.Any());

            return warehouse.ToList();
        }

        //================================================================================
    }
}