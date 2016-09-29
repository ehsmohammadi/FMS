#region

using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Domain.Model.FakeDomainServices;

#endregion

namespace MITD.Fuel.Data.EF.Migrations
{
  

    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

//    #endregion
//
//    internal sealed class Configuration : DbMigrationsConfiguration<DataContainer>
//    {
//        public Configuration()
//        {
//            AutomaticMigrationsEnabled = false;
//        }
//
//        protected override void Seed(DataContainer context)
//        {
//            context.Users.AddOrUpdate(FakeDomainService.GetUsers().ToArray());
//
//            context.Currencies.AddOrUpdate(FakeDomainService.GetCurrencies().ToArray());
//
//            context.EnterpriseParties.AddOrUpdate(FakeDomainService.GetEnterpriseParties().ToArray());
//
//            context.Vessels.AddOrUpdate(FakeDomainService.GetVessels().ToArray());
//
//            context.Voyages.AddOrUpdate(FakeDomainService.GetVoyages().ToArray());
//
//            context.GoodUnits.AddOrUpdate(FakeDomainService.GetGoodUnits().ToArray());
//
//            context.Goods.AddOrUpdate(FakeDomainService.GetGoods().ToArray());
//
//            context.GoodPartyAssignments.AddOrUpdate(FakeDomainService.GetGoodPartyAssignments().ToArray());
//
//            context.Tanks.AddOrUpdate(FakeDomainService.GetTanks().ToArray());
//
//            context.FuelReports.AddOrUpdate(FakeDomainService.GetFuelReports().ToArray());
//
//            context.FuelReportDetails.AddOrUpdate(FakeDomainService.GetFuelReportDetails().ToArray());
//
//            context.SaveChanges();
//
//            #region Order Add Query
//
//            context.Database.ExecuteSqlCommand(
//                "Delete From  [Fuel].[Order]");
//
//            context.Database.ExecuteSqlCommand(
//                "Delete From  [Fuel].[OrderItems]");
//            context.Database.ExecuteSqlCommand(
//                " SET IDENTITY_INSERT[Fuel].[Order] ON "
//                +
//                " INSERT [Fuel].[Order]([Id],[Code], [Description], [SupplierId], [ReceiverId], [TransporterId], [OwnerId], [OrderType], [OrderDate], [UserCreatedById], [VesselFromId], [VesselToId], [ActionType], [ActionUserId]) " +
//                " VALUES (1,1,'code1', 3, NULL, NULL, 1, 1, '2013-11-25', NULL, NULL, NULL, 0, NULL);" +
//                " INSERT [Fuel].[Order]([Id],[Code], [Description], [SupplierId], [ReceiverId], [TransporterId], [OwnerId], [OrderType], [OrderDate], [UserCreatedById], [VesselFromId], [VesselToId], [ActionType], [ActionUserId]) " +
//                " VALUES (2,2,'code1', 3, NULL, NULL, 1, 1, '2013-11-25', NULL, NULL, NULL, 0, NULL)" +
//                " INSERT [Fuel].[Order]([Id],[Code], [Description], [SupplierId], [ReceiverId], [TransporterId], [OwnerId], [OrderType], [OrderDate], [UserCreatedById], [VesselFromId], [VesselToId], [ActionType], [ActionUserId]) " +
//                " VALUES (3,3, 'code2', 2, NULL,3, 1, 3,'2013-11-25' , NULL, NULL, NULL, 0, NULL)" +
//                " INSERT [Fuel].[Order]([Id],[Code], [Description], [SupplierId], [ReceiverId], [TransporterId], [OwnerId], [OrderType], [OrderDate], [UserCreatedById], [VesselFromId], [VesselToId], [ActionType], [ActionUserId]) " +
//                " VALUES (4,4, 'code3',  NULL, 1, 3,2, 2,'2013-11-25', NULL, 5,1, 0, NULL)" +
//                " INSERT [Fuel].[Order]([Id],[Code], [Description], [SupplierId], [ReceiverId], [TransporterId], [OwnerId], [OrderType], [OrderDate], [UserCreatedById], [VesselFromId], [VesselToId], [ActionType], [ActionUserId])" +
//                " VALUES (5,5, 'code4', NULL, NULL, NULL, 1, 4, '2013-11-25', NULL, 1, 2,0, NULL)" +
//                " SET IDENTITY_INSERT[Fuel].[Order] Off " +
//                " SET IDENTITY_INSERT [Fuel].[OrderItems] ON " +
//                " INSERT [Fuel].[OrderItems]([Id],[Description], [Quantity], [OrderId], [GoodId], [GoodUnitId], [GoodPartyAssignmentId])  " +
//                "VALUES (1,'desc', 2,1,1,1,1)" +
//                " INSERT [Fuel].[OrderItems]([Id],[Description], [Quantity], [OrderId], [GoodId], [GoodUnitId], [GoodPartyAssignmentId])  " +
//                "VALUES (2,'desc', 2,2,1,1,1)" +
//                " INSERT [Fuel].[OrderItems]([Id],[Description], [Quantity], [OrderId], [GoodId], [GoodUnitId], [GoodPartyAssignmentId]) " +
//                " VALUES (3,'desc', 3,3,1,1,1) " +
//                " SET IDENTITY_INSERT [Fuel].[OrderItems] Off ");
//
//            #endregion
//        }
//    }


    internal class DropCreateDatabaseWhenChanges : DropCreateDatabaseIfModelChanges<DataContainer>
    {
        protected override void Seed(DataContainer context)
        {
            context.Users.AddOrUpdate(FakeDomainService.GetUsers().ToArray());

            context.Currencies.AddOrUpdate(FakeDomainService.GetCurrencies().ToArray());

            context.EnterpriseParties.AddOrUpdate(FakeDomainService.GetEnterpriseParties().ToArray());

            context.Vessels.AddOrUpdate(FakeDomainService.GetVessels().ToArray());

            context.Voyages.AddOrUpdate(FakeDomainService.GetVoyages().ToArray());

            context.GoodUnits.AddOrUpdate(FakeDomainService.GetGoodUnits().ToArray());

            context.Goods.AddOrUpdate(FakeDomainService.GetGoods().ToArray());

            context.GoodPartyAssignments.AddOrUpdate(FakeDomainService.GetGoodPartyAssignments().ToArray());

            context.Tanks.AddOrUpdate(FakeDomainService.GetTanks().ToArray());

            context.FuelReports.AddOrUpdate(FakeDomainService.GetFuelReports().ToArray());

            context.FuelReportDetails.AddOrUpdate(FakeDomainService.GetFuelReportDetails().ToArray());

            context.SaveChanges();

            #region Order Add Query

            context.Database.ExecuteSqlCommand(
                "Delete From  [Fuel].[Order]");

            context.Database.ExecuteSqlCommand(
                "Delete From  [Fuel].[OrderItems]");
            context.Database.ExecuteSqlCommand(
                " SET IDENTITY_INSERT[Fuel].[Order] ON "
                +
                " INSERT [Fuel].[Order]([Id],[Code], [Description], [SupplierId], [ReceiverId], [TransporterId], [OwnerId], [OrderType], [OrderDate], [UserCreatedById], [VesselFromId], [VesselToId], [ActionType], [ActionUserId]) " +
                " VALUES (1,1,'code1', 3, NULL, NULL, 1, 1, '2013-11-25', NULL, NULL, NULL, 0, NULL);" +
                " INSERT [Fuel].[Order]([Id],[Code], [Description], [SupplierId], [ReceiverId], [TransporterId], [OwnerId], [OrderType], [OrderDate], [UserCreatedById], [VesselFromId], [VesselToId], [ActionType], [ActionUserId]) " +
                " VALUES (2,2,'code1', 3, NULL, NULL, 1, 1, '2013-11-25', NULL, NULL, NULL, 0, NULL)" +
                " INSERT [Fuel].[Order]([Id],[Code], [Description], [SupplierId], [ReceiverId], [TransporterId], [OwnerId], [OrderType], [OrderDate], [UserCreatedById], [VesselFromId], [VesselToId], [ActionType], [ActionUserId]) " +
                " VALUES (3,3, 'code2', 2, NULL,3, 1, 3,'2013-11-25' , NULL, NULL, NULL, 0, NULL)" +
                " INSERT [Fuel].[Order]([Id],[Code], [Description], [SupplierId], [ReceiverId], [TransporterId], [OwnerId], [OrderType], [OrderDate], [UserCreatedById], [VesselFromId], [VesselToId], [ActionType], [ActionUserId]) " +
                " VALUES (4,4, 'code3',  NULL, 1, 3,2, 2,'2013-11-25', NULL, 5,1, 0, NULL)" +
                " INSERT [Fuel].[Order]([Id],[Code], [Description], [SupplierId], [ReceiverId], [TransporterId], [OwnerId], [OrderType], [OrderDate], [UserCreatedById], [VesselFromId], [VesselToId], [ActionType], [ActionUserId])" +
                " VALUES (5,5, 'code4', NULL, NULL, NULL, 1, 4, '2013-11-25', NULL, 1, 2,0, NULL)" +
                " SET IDENTITY_INSERT[Fuel].[Order] Off " +
                " SET IDENTITY_INSERT [Fuel].[OrderItems] ON " +
                " INSERT [Fuel].[OrderItems]([Id],[Description], [Quantity], [OrderId], [GoodId], [GoodUnitId], [GoodPartyAssignmentId])  " +
                "VALUES (1,'desc', 2,1,1,1,1)" +
                " INSERT [Fuel].[OrderItems]([Id],[Description], [Quantity], [OrderId], [GoodId], [GoodUnitId], [GoodPartyAssignmentId])  " +
                "VALUES (2,'desc', 2,2,1,1,1)" +
                " INSERT [Fuel].[OrderItems]([Id],[Description], [Quantity], [OrderId], [GoodId], [GoodUnitId], [GoodPartyAssignmentId]) " +
                " VALUES (3,'desc', 3,3,1,1,1) " +
                " SET IDENTITY_INSERT [Fuel].[OrderItems] Off ");

            #endregion
        }
    }
}