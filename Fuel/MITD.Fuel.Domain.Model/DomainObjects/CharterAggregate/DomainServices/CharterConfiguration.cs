using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.CharterTypes;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.DomainServices
{


    public class CharterConfiguration : IEntityConfigurator<Charter>
    {
        private ICharterStateFactory _charterStateFactory;

        private ICharterInDomainService _charterInDomainService;
        private ICharterOutDomainService _charterOutDomainService;
        private IEventPublisher _eventPublisher;
        private readonly IVesselInCompanyDomainService vesselInCompanyDomainService;
        private IInventoryOperationNotifier inventoryOperationNotifier;

        public CharterConfiguration(ICharterStateFactory charterStateFactory,
            ICharterInDomainService charterInDomainService,
            ICharterOutDomainService charterOutDomainService
            , IEventPublisher eventPublisher, IVesselInCompanyDomainService vesselInCompanyDomainService, IInventoryOperationNotifier inventoryOperationNotifier)
        {
            _charterStateFactory = charterStateFactory;
            _charterInDomainService = charterInDomainService;
            _charterOutDomainService = charterOutDomainService;
            _eventPublisher = eventPublisher;
            this.vesselInCompanyDomainService = vesselInCompanyDomainService;
            this.inventoryOperationNotifier = inventoryOperationNotifier;
        }


        public Charter Configure(Charter charter)
        {
            if (charter != null)
            {
                charter.Resolve(_charterInDomainService, _charterOutDomainService, _eventPublisher,vesselInCompanyDomainService, inventoryOperationNotifier);
                charter.SetStateType(_charterStateFactory.CreateState(charter.CurrentState));
            }

            return charter;
        }


    }

}
