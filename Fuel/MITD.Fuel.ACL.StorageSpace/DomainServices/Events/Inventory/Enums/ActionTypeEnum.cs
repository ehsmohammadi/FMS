namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums
{
    public enum ActionTypeEnum
    {
        Undefined = 0,
        Created = 1,
        Approved = 2,
        FinalApproved = 3,
        Rejected = 4
    }

    public enum ActionEntityTypeEnum
    {
        Order = 101,
        FuelReport = 102,
        Invoice = 103,
        Scrap = 104,
        Offhire = 107,
        CharterIn = 105,
        CharterOut = 106
    }

    public enum DecisionTypeEnum
    {
        Approved,
        Rejected,
        Canceled
    }
}
