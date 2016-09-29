namespace MITD.Fuel.Domain.Model.Enums
{
    public enum WorkflowEntities
    {
        Order = 1,
        FuelReport,
        Invoice,
        Scrap,
        CharterInStart,
        CharterInEnd,
        CharterOutStart,
        CharterOutEnd,
        Offhire,
    }

    public enum WorkflowActionEntityType
    {
        Order = 101,
        FuelReport = 102,
        Invoice = 103,
        Scrap = 104,
        CharterIn = 105,
        CharterOut = 106,
        Offhire = 107,
    }
}