
using System.Data;
using System.Data.SqlClient;
using FluentMigrator;
using MITD.Fuel.Data.EF.Migrations;

namespace MITD.Data.EF.Migrations
{
    [Migration(39)]
    public class Migration_V39 : Migration
    {
        public override void Down()
        {

        }

        public override void Up()
        {

            alterRotationVoyagesView();
        }

        private void alterRotationVoyagesView()
        {
            this.Execute.Sql(@"
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [BasicInfo].[RotationVoyagesView]
AS
SELECT   CAST(VoyagesQuery.Id AS BIGINT) AS Id, VoyagesQuery.VoyageNumber COLLATE Arabic_CI_AS AS VoyageNumber, 
                VoyagesQuery.VoyageNumber COLLATE Arabic_CI_AS AS Description, CAST(VoyagesQuery.CompanyId AS BIGINT) AS CompanyId, 
                Fuel.VesselInCompany.Id AS VesselInCompanyId, VoyagesQuery.StartDateTime AS StartDate, VoyagesQuery.EndDateTime AS EndDate, VoyagesQuery.IsActive
FROM      (SELECT   Id, VesselCode, 1 AS ShipOwnerId, VoyageNumber, StartDateTime, EndDateTime, IsActive, CASE WHEN [TripType] IN (5, 6, 7, 10) 
                                 THEN 1 /* As ShipOwner*/ ELSE
                                     (SELECT   TOP 1 Id
                                      FROM      [BasicInfo].[CompanyView]
                                      WHERE   NAME = 'SAPID') /*As Operation Company*/ END AS CompanyId
                 FROM      dbo.SAPIDVoyagesView
                 WHERE   EndDateTime IS NULL OR
                                 EndDateTime > '2014-01-01 00:00:00.000'
                 UNION
                 SELECT   10000000 + Id AS Id, VesselCode, 1 AS ShipOwnerId, VoyageNumber, StartDateTime, EndDateTime, IsActive, CASE WHEN [TripType] IN (5, 6, 7, 10) 
                                 THEN 1 /* As ShipOwner*/ ELSE
                                     (SELECT   TOP 1 Id
                                      FROM      [BasicInfo].[CompanyView]
                                      WHERE   NAME = 'HAFIZ') /*As Operation Company*/ END AS CompanyId
                 FROM      dbo.HAFIZVoyagesView
                 WHERE   EndDateTime IS NULL OR
                                 EndDateTime > '2014-01-01 00:00:00.000'
                 UNION
                 SELECT   20000000 + Id AS Id, VesselCode, 1 AS ShipOwnerId, VoyageNumber, StartDateTime, EndDateTime, IsActive, CASE WHEN [TripType] IN (5, 6, 7, 10) 
                                 THEN 1 /* As ShipOwner*/ ELSE
                                     (SELECT   TOP 1 Id
                                      FROM      [BasicInfo].[CompanyView]
                                      WHERE   NAME = 'HAFEZ') END AS CompanyId
                 FROM      dbo.[HAFEZVoyagesView]
                 WHERE   EndDateTime IS NULL OR
                                 EndDateTime > '2014-01-01 00:00:00.000') AS VoyagesQuery INNER JOIN
                Fuel.Vessel ON VoyagesQuery.VesselCode = Fuel.Vessel.Code COLLATE SQL_Latin1_General_CP1256_CI_AS INNER JOIN
                Fuel.VesselInCompany ON Fuel.VesselInCompany.VesselId = Fuel.Vessel.Id AND Fuel.VesselInCompany.CompanyId = VoyagesQuery.CompanyId
UNION
SELECT   Id, VoyageNumber, Description, CompanyId, VesselInCompanyId, StartDate, EndDate, IsActive
FROM      BasicInfo.IRISLVoyages
");
        }
    }
}
