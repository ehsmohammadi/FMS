using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core.Mapping;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class JournalEntryToDtoMapper : BaseFacadeMapper<JournalEntry, JournalEntryDto>, IJournalEntryToDtoMapper
    {

        public JournalEntryDto MapToDtoModel(JournalEntry journalEntry)
        {
            var res = new JournalEntryDto()
                   {
                       Id = journalEntry.Id,
                       VoucherId = journalEntry.VoucherId,
                       Typ = journalEntry.Typ,
                       Description = journalEntry.Description,
                       VoucherRef = journalEntry.VoucherRef,
                       AccountNo = journalEntry.AccountNo,
                       IrrAmount = journalEntry.IrrAmount,
                       ForeignAmount = journalEntry.ForeignAmount,
                       CurrencyDto = new CurrencyDto()
                                     {
                                         Id = journalEntry.CurrencyId,
                                         Abbreviation = journalEntry.Currency.Abbreviation,
                                         Name = journalEntry.Currency.Name
                                     },


                   };
            if (journalEntry.Segments != null)
                journalEntry.Segments.ForEach(c => { res.SegmentCode += c.Code + ","; });
            return res;
        }

        public List<JournalEntryDto> MapToDtoModels(List<JournalEntry> journalEntries)
        {
            var res = new List<JournalEntryDto>();
            journalEntries.ForEach(c => res.Add(MapToDtoModel(c)));

            return res;

        }
    }
}
