namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums
{
    public enum FuelReportTypeEnum:int
    {
        NoonReport = 1,
        EndofVoyage = 2,
        ArrivalReport = 3,
        DepartureReport = 4,
        EndOfYear = 5,
        EndOfMonth = 6,
        CharterInEnd = 7,
        CharterOutStart = 8,
        DryDock = 9,
        BeginOfOffHire = 10,
        BeginOfLayUp = 11,
        EndOfOffhire = 12,
        BeginOfPassage = 13,
        EndOfPassage = 14,
        Bunkering = 15,
        Debunkering = 16,
    }
}
