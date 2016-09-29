namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums
{
    public enum OrderTypeEnum
    {
        //[Description("همه")]
        None = 0,
        
        //[Description("خرید")]
        Purchase = 1,
        
        //[Description("انتقال")]
        Transfer = 2,
        
        //[Description("خرید انتفالی")]
        PurchaseWithTransfer = 3,
        
        //[Description("انتقال داخلی")]
        InternalTransfer = 4
    }
}