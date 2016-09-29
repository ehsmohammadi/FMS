namespace MITD.Fuel.Domain.Model.Enums.Inventory
{
    
    /// <summary>
    /// Enumerates the valid Operations in Inventory.
    /// </summary>

    public enum InventoryOperationType :byte
    {
        Receipt = 1,
        Issue = 2,
        Pricing = 3
    }
}