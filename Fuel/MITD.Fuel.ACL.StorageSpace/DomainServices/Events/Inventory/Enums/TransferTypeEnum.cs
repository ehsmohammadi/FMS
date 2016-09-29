namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums
{
    public enum TransferTypeEnum : int
    {
        //[Description(" ")]
        NotDefined = 0,

        //[Description("امانی")]
        //Trust = 1,

        //[Description("انتقال داخلی")]
        InternalTransfer = 2,

        //[Description("فروش انتقالی")]
        TransferSale = 3,

        //[Description("انتقال مرجوعی")]
        Rejected = 4,
    }
}
