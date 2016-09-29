using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IJournalEntryToDtoMapper : IFacadeMapper<JournalEntry, JournalEntryDto>
    {
        JournalEntryDto MapToDtoModel(JournalEntry journalEntry);
        List<JournalEntryDto> MapToDtoModels(List<JournalEntry> journalEntries);
    }
}
