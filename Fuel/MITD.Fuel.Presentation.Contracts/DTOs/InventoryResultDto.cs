using System.Collections.ObjectModel;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class InventoryResultDto
    {
        private long id;
        private string number;
        private InventoryResultDtoActionType actionType;
        private ObservableCollection<InventoryResultItemDto> inventoryResultItems;

        public long Id
        {
            get { return this.id; }
            set { this.SetField(p => p.Id, ref this.id, value); }
        }

        public string Number
        {
            get { return this.number; }
            set { this.SetField(p => p.Number, ref this.number, value); }
        }

        public InventoryResultDtoActionType ActionType
        {
            get { return this.actionType; }
            set { this.SetField(p => p.ActionType, ref this.actionType, value); }
        }

        public ObservableCollection<InventoryResultItemDto> InventoryResultItems
        {
            get { return this.inventoryResultItems; }
            set { this.SetField(p => p.InventoryResultItems, ref this.inventoryResultItems, value); }
        }
    }
}
