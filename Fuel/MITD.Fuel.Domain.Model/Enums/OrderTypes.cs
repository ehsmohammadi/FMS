namespace MITD.Fuel.Domain.Model.Enums
{
    public enum OrderTypes
    {
        None = 0,
        Purchase = 1,
        Transfer = 2,
        PurchaseWithTransferOperations = 3,
        InternalTransfer = 4,
        PurchaseForVessel = 5,
        SupplyForDeliveredVessel = 6,
    }
}