/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE Fuel.Voyage
	DROP CONSTRAINT FK_Voyage_VesselInCompanyId_VesselInCompany_Id
GO
ALTER TABLE Fuel.VesselInCompany SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE Fuel.Tmp_Voyage
	(
	Id bigint NOT NULL IDENTITY (1, 1),
	VoyageNumber nvarchar(200) NOT NULL,
	Description nvarchar(200) NOT NULL,
	VesselInCompanyId bigint NOT NULL,
	CompanyId bigint NOT NULL,
	StartDate datetime NOT NULL,
	EndDate datetime NULL,
	IsActive bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE Fuel.Tmp_Voyage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT Fuel.Tmp_Voyage ON
GO
IF EXISTS(SELECT * FROM Fuel.Voyage)
	 EXEC('INSERT INTO Fuel.Tmp_Voyage (Id, VoyageNumber, Description, VesselInCompanyId, CompanyId, StartDate, EndDate, IsActive)
		SELECT Id, VoyageNumber, Description, VesselInCompanyId, CompanyId, StartDate, EndDate, IsActive FROM Fuel.Voyage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT Fuel.Tmp_Voyage OFF
GO
DROP TABLE Fuel.Voyage
GO
EXECUTE sp_rename N'Fuel.Tmp_Voyage', N'Voyage', 'OBJECT' 
GO
ALTER TABLE Fuel.Voyage ADD CONSTRAINT
	PK_Voyage PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_Voyage_VesselInCompanyId ON Fuel.Voyage
	(
	VesselInCompanyId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_Voyage_CompanyId ON Fuel.Voyage
	(
	CompanyId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE Fuel.Voyage ADD CONSTRAINT
	FK_Voyage_VesselInCompanyId_VesselInCompany_Id FOREIGN KEY
	(
	VesselInCompanyId
	) REFERENCES Fuel.VesselInCompany
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
