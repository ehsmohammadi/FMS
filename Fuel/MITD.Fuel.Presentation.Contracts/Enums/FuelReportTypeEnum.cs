using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Presentation.Contracts.Enums
{
    public enum FuelReportTypeEnum
    {
        None = 0,
        NoonReport = 1,
        EndOfVoyage = 2,
        ArrivalReport = 3,
        DepartureReport = 4,
        Berthing = 5,
        Unberthing = 6,
        CharterInEnd = 7,
        CharterOutStart = 8,
        BeginOfDryDock = 9,
        BeginOfOffHire = 10,
        BeginOfLayUp = 11,
        EndOfOffhire = 12,
        BunkeringCommenced = 13,
        DebunkeringCommenced = 14,
        Anchoring = 15,
        HeavingAnchor = 16,
        EndOfDryDock = 17,
        BunkeringCompleted = 18,
        DebunkeringCompleted = 19,
    }
}
