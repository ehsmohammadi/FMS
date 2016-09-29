using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Services.Facade;
using Omu.ValueInjecter;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
{
    public class InvoiceItemToDtoMapper : BaseFacadeMapper<InvoiceItem, InvoiceItemDto>, IInvoiceItemToDtoMapper
    {
        private readonly IGoodToGoodDtoMapper _goodMapper;

        public InvoiceItemToDtoMapper(IGoodToGoodDtoMapper goodMapper)
        {
            _goodMapper = goodMapper;
        }

        public InvoiceItemDto MapEntityToDto(InvoiceItem invoiceItem)
        {


            GoodDto goodDto = _goodMapper.MapEntityToDtoWithUnits(invoiceItem.Good);

            goodDto.Unit = new GoodUnitDto {Id = invoiceItem.MeasuringUnit.Id, Name = invoiceItem.MeasuringUnit.Name};

            var dto = new InvoiceItemDto();
            dto.InjectFrom<FlatLoopValueInjection>(invoiceItem);
            return dto;
        }


        public IEnumerable<InvoiceItemDto> MapEntityToDto(IEnumerable<InvoiceItem> entities)
        {
            return entities.Select(MapEntityToDto);
        }

        public List<InvoiceItemCommand> MapModelToCommand(ObservableCollection<InvoiceItemDto> invoiceItems)
        {
            return invoiceItems.Select(MapDtoToCommand).ToList();
        }

        public InvoiceItemCommand MapDtoToCommand(InvoiceItemDto item)
        {
            var cmd = new InvoiceItemCommand(item.Quantity, item.Fee, item.GoodId, item.MeasuringUnitId, item.DivisionPrice,item.Description);
            return cmd;
        }
    }
}