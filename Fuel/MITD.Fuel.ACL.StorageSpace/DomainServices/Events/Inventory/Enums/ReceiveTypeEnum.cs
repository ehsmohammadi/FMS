namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums
{
    public enum ReceiveTypeEnum : int
    {
        //[Description(" ")]
        NotDefined = 0,

        //[Description("امانی")]
        Trust = 1,

        //[Description("انتقال داخلی")]
        InternalTransfer = 2,

        //[Description("خرید انتقالی")]
        TransferPurchase = 3,

        //[Description("خرید")]
        Purchase = 4,
    }
}
