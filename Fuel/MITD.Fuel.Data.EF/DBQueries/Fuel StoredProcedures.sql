IF OBJECT_ID (N'[Fuel].[GetFuelOriginalData]',N'P') IS NOT NULL
   DROP PROCEDURE [Fuel].[GetFuelOriginalData];
GO

CREATE PROCEDURE [Fuel].[GetFuelOriginalData]
(
	@Code NVARCHAR(50)
)
AS
BEGIN

	--DECLARE @FuelReportTypeNamesTable AS TABLE ([Type] INT, [Name] NVARCHAR(100))

	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (1 , 'Noon Report')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (2 , 'End Of Voyage')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (3 , 'Arrival Report')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (4 , 'Departure Report')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (5 , 'End Of Year')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (6 , 'End Of Month')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (7 , 'Charter-In End')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (8 , 'Charter-Out Start')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (9 , 'Dry Dock')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (10 , 'Begin Of Off-Hire')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (11 , 'Lay-Up')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (12 , 'End Of Off-Hire')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (13 , 'Begin Of Passage')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (14 , 'End Of Passage')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (15 , 'Bunkering')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (16 , 'Debunkering')
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()

	--DECLARE @LocationTypeTable AS TABLE ([Type] INT, [Name] NVARCHAR(100))

	--INSERT INTO @LocationTypeTable([Type],NAME) VALUES (1 , 'At Sea')
	--INSERT INTO @LocationTypeTable([Type],NAME) VALUES (2 , 'In Port')
	--INSERT INTO @LocationTypeTable([Type],NAME) VALUES (3 , 'At Anchorage')

	SELECT er.*, frtnt.Name AS FuelReportTypeName, ltt.Name AS LocationTypeName, 
		CorrectionTypeHo + ' ' + CAST(er.CorrectionHo AS NVARCHAR(50)) AS CorrectionHoValue, 
		CorrectionTypeDo + ' ' + CAST(er.CorrectionDo AS NVARCHAR(50)) AS CorrectionDoValue, 
		CorrectionTypeFW + ' ' + CAST(er.CorrectionFW AS NVARCHAR(50)) AS CorrectionFWValue, 
		CorrectionTypeMGOLS + ' ' + CAST(er.CorrectionMGOLS AS NVARCHAR(50)) AS CorrectionMGOLSValue
	FROM [EventReport].[dbo].[EventReport] er
		LEFT JOIN  [EventReport].[dbo].[EventReportTypes] frtnt ON frtnt.[Type] = er.[FuelReportType] 
		LEFT JOIN  [EventReport].[dbo].[LocationTypes] ltt ON er.InPortOrAtSea = ltt.[Type]
	WHERE CAST(er.[ID]  AS NVARCHAR(50)) = @Code

END

GO

GRANT EXECUTE ON [Fuel].[GetFuelOriginalData] TO [public]
GO

----------------------------------------------------------------------------------------

IF OBJECT_ID (N'[Fuel].[GetVesselReportData]',N'P') IS NOT NULL
   DROP PROCEDURE [Fuel].[GetVesselReportData];
GO

CREATE PROCEDURE [Fuel].[GetVesselReportData]
(
	@ShipCode NVARCHAR(10) = NULL,
	@VoyageNo NVARCHAR(50) = NULL,
	@FromDate DATETIME = NULL,
	@ToDate DATETIME = NULL,
	@PortTime FLOAT = NULL,
	@PortTimeMOL FLOAT = NULL,
	@LocationType NVARCHAR(10) = NULL
)
AS
BEGIN

	IF(@FromDate IS NULL) 
		SET @FromDate = '1753-01-01 00:00:00.000'

	IF(@ToDate IS NULL)
		SET @ToDate = '9999-12-31 23:59:59.997'


	DECLARE @FromYear  int,@FromMonth int, @FromDay int
	DECLARE @ToYear  int,@ToMonth int, @ToDay int
	
	SET @FromYear = DATEPART(YEAR, @FromDate)
	SET @FromMonth = DATEPART(MONTH, @FromDate)
	SET @FromDay = DATEPART(DAY, @FromDate)

	SET @ToYear = DATEPART(YEAR, @ToDate)
	SET @ToMonth = DATEPART(MONTH, @ToDate)
	SET @ToDay = DATEPART(DAY, @ToDate)

	DECLARE @MinPortTime FLOAT, @MaxPortTime FLOAT 

	SET @MinPortTime = @PortTime - ABS(ISNULL(@PortTimeMOL, 0))
	SET @MaxPortTime = @PortTime + ABS(ISNULL(@PortTimeMOL, 0))

	SET @MinPortTime = ISNULL(@MinPortTime, -100000)
	SET @MaxPortTime = ISNULL(@MaxPortTime, 100000)



	--DECLARE @FuelReportTypeNamesTable AS TABLE ([Type] INT, [Name] NVARCHAR(100))

	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (1 , 'Noon Report')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (2 , 'End Of Voyage')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (3 , 'Arrival Report')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (4 , 'Departure Report')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (5 , 'End Of Year')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (6 , 'End Of Month')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (7 , 'Charter-In End')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (8 , 'Charter-Out Start')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (9 , 'Dry Dock')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (10 , 'Begin Of Off-Hire')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (11 , 'Lay-Up')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (12 , 'End Of Off-Hire')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (13 , 'Begin Of Passage')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (14 , 'End Of Passage')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (15 , 'Bunkering')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (16 , 'Debunkering')
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()

	--DECLARE @LocationTypeTable AS TABLE ([Type] INT, [Name] NVARCHAR(100))

	--INSERT INTO @LocationTypeTable([Type],NAME) VALUES (1 , 'At Sea')
	--INSERT INTO @LocationTypeTable([Type],NAME) VALUES (2 , 'In Port')
	--INSERT INTO @LocationTypeTable([Type],NAME) VALUES (3 , 'At Anchorage')

	SELECT er.*, frtnt.Name AS FuelReportTypeName, ltt.Name AS LocationTypeName, 
		CorrectionTypeHo + ' ' + CAST(er.CorrectionHo AS NVARCHAR(50)) AS CorrectionHoValue, 
		CorrectionTypeDo + ' ' + CAST(er.CorrectionDo AS NVARCHAR(50)) AS CorrectionDoValue, 
		CorrectionTypeFW + ' ' + CAST(er.CorrectionFW AS NVARCHAR(50)) AS CorrectionFWValue, 
		CorrectionTypeMGOLS + ' ' + CAST(er.CorrectionMGOLS AS NVARCHAR(50)) AS CorrectionMGOLSValue
	FROM [EventReport].[dbo].[EventReport] er
		LEFT JOIN  [EventReport].[dbo].[EventReportTypes] frtnt ON frtnt.[Type] = er.[FuelReportType]
		LEFT JOIN  [EventReport].[dbo].[LocationTypes] ltt ON er.InPortOrAtSea = ltt.[Type]
	WHERE (@ShipCode IS NULL OR RIGHT('0000' + UPPER(er.ShipCode), 4) = @ShipCode) AND 
		(@VoyageNo IS NULL OR UPPER(er.[VoyageNo]) = UPPER(@VoyageNo)) AND
		(@FromYear <= er.[Year] AND er.[Year] <= @ToYear) AND
		(@FromMonth <= er.[Month] AND er.[Month] <= @ToMonth) AND
		(@FromDay <= er.[Day] AND er.[Day] <= @ToDay) AND 
		(er.PortTime BETWEEN @MinPortTime AND @MaxPortTime) AND
		(@LocationType IS NULL OR @LocationType LIKE '%' + er.InPortOrAtSea + '%')
	ORDER BY [ShipCode],[Year],[Month],[Day],[Time]

END

GO

GRANT EXECUTE ON [Fuel].[GetVesselReportData] TO [public]
GO

----------------------------------------------------------------------------------------

IF OBJECT_ID (N'[Fuel].[GetVesselReportLocationTypeData]',N'P') IS NOT NULL
   DROP PROCEDURE [Fuel].[GetVesselReportLocationTypeData];
GO

--CREATE PROCEDURE [Fuel].[GetVesselReportLocationTypeData]
--AS
--BEGIN
 
--	DECLARE @LocationTypeTable AS TABLE ([Type] INT, [Name] NVARCHAR(100))

--	INSERT INTO @LocationTypeTable([Type],NAME) VALUES (1 , 'At Sea')
--	INSERT INTO @LocationTypeTable([Type],NAME) VALUES (2 , 'In Port')
--	INSERT INTO @LocationTypeTable([Type],NAME) VALUES (3 , 'At Anchorage')

--	SELECT * FROM @LocationTypeTable;
--END
--GO

--GRANT EXECUTE ON [Fuel].[GetVesselReportLocationTypeData] TO [public]
--GO

----------------------------------------------------------------------------------------

IF OBJECT_ID (N'[Fuel].[GetVesselReportVoyageData]',N'P') IS NOT NULL
   DROP PROCEDURE [Fuel].[GetVesselReportVoyageData];
GO

CREATE PROCEDURE [Fuel].[GetVesselReportVoyageData]
(
	@ShipCode NVARCHAR(50) = NULL
)
AS
BEGIN
 
 	DECLARE @ResultTable AS TABLE (VoyageNo NVARCHAR(50), ShipCode NVARCHAR(200))

	INSERT INTO @ResultTable VALUES ('All', NULL)

	INSERT INTO @ResultTable
		SELECT DISTINCT UPPER(er.VoyageNo) AS VoyageNo, RIGHT('0000' + UPPER(er.ShipCode), 4) AS ShipCode 
		FROM [EventReport].[dbo].[EventReport] er
		WHERE (@ShipCode IS NULL OR RIGHT('0000' + UPPER(er.ShipCode), 4) = @ShipCode)


	SELECT * FROM @ResultTable

END
GO

GRANT EXECUTE ON [Fuel].[GetVesselReportVoyageData] TO [public]
GO

----------------------------------------------------------------------------------------

IF OBJECT_ID (N'[Fuel].[GetVesselReportShipNameData]',N'P') IS NOT NULL
   DROP PROCEDURE [Fuel].[GetVesselReportShipNameData];
GO

CREATE PROCEDURE [Fuel].[GetVesselReportShipNameData]
AS
BEGIN
 
	DECLARE @ResultTable AS TABLE (ShipCode NVARCHAR(50), ShipName NVARCHAR(200))

	INSERT INTO @ResultTable VALUES (NULL, 'All')

	INSERT INTO @ResultTable
	SELECT DISTINCT  RIGHT('0000' + UPPER(er.ShipCode), 4) AS ShipCode, (SELECT TOP 1 UPPER(ier.ShipName) 
		FROM [EventReport].[dbo].[EventReport] ier WHERE RIGHT('0000' + UPPER(ier.ShipCode), 4) = RIGHT('0000' + UPPER(er.ShipCode), 4)) AS ShipName 
	FROM [EventReport].[dbo].[EventReport] er 

	SELECT * FROM @ResultTable
END
GO

GRANT EXECUTE ON [Fuel].[GetVesselReportShipNameData] TO [public]
GO
----------------------------------------------------------------------
IF OBJECT_ID ( '[Fuel].[PeriodicalFuelStatistics]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Fuel].[PeriodicalFuelStatistics];
GO
CREATE PROCEDURE [Fuel].[PeriodicalFuelStatistics]
(
	 @CompanyId BIGINT=NULL
    ,@WarehouseId BIGINT=NULL
    ,@QuantityUnitId BIGINT
    ,@GoodId BIGINT=NULL
    ,@From DATETIME
    ,@To DATETIME
)
--WITH ENCRYPTION
AS
BEGIN
	
DECLARE  @StatrtFiscalYear DATETIME
        ,@MainCurrencyUnit NVARCHAR(256)

SET @CompanyId = ISNULL(@CompanyId, (SELECT TOP 1 w.CompanyId FROM Inventory.Warehouse w WHERE w.Id = @WarehouseId))

SET @StatrtFiscalYear = (SELECT TOP(1) fy.StartDate
							FROM Inventory.FinancialYear fy 
							WHERE fy.StartDate<=@From AND fy.EndDate>=@To)

DECLARE @Cardex TABLE
(
	[Type] NVARCHAR(256),
    GoodId BIGINT,
    GoodCode NVARCHAR(256),
    GoodName NVARCHAR(256),
    QuantityUnit NVARCHAR(256),
    MainCurrencyUnit NVARCHAR(256),
    QuantityAmount DECIMAL(20,3),
    TotalPrice DECIMAL(20,3),
    CompanyId   BIGINT,
    WarehouseId BIGINT,
    Status NVARCHAR(50)
)

DECLARE @Template TABLE
(
	[Type] NVARCHAR(256),
    --GoodId BIGINT,
    --GoodCode NVARCHAR(256),
    --GoodName NVARCHAR(256),
    --QuantityUnit NVARCHAR(256),
    MainCurrencyUnit NVARCHAR(256),
    QuantityAmount DECIMAL(20,3),
    TotalPrice DECIMAL(20,3),
    CompanyId   BIGINT,
    WarehouseId BIGINT,
    Status NVARCHAR(50)
)
SET @MainCurrencyUnit=(SELECT TOP(1)u.Name NAME
                         FROM Inventory.Units u WHERE u.IsCurrency=1 AND u.IsBaseCurrency=1)
  
INSERT INTO @Template(	[Type], MainCurrencyUnit, QuantityAmount,	TotalPrice,	CompanyId, WarehouseId,	[Status])
	SELECT 	N'Receipt', @MainCurrencyUnit, 0, 0, w.CompanyId,w.Id, N'Total'
	FROM Inventory.Warehouse w   
	WHERE (@WarehouseId IS NULL OR w.Id=@WarehouseId) AND (@CompanyId IS NULL OR w.CompanyId=@CompanyId) 
      
INSERT INTO @Template(	[Type], MainCurrencyUnit, QuantityAmount,	TotalPrice,	CompanyId, WarehouseId,	[Status])
SELECT 	N'Receipt', @MainCurrencyUnit, 0, 0, w.CompanyId,w.Id, N'Priced'
	FROM Inventory.Warehouse w   
	WHERE (@WarehouseId IS NULL OR w.Id=@WarehouseId) AND (@CompanyId IS NULL OR w.CompanyId=@CompanyId) 

INSERT INTO @Template(	[Type], MainCurrencyUnit, QuantityAmount,	TotalPrice,	CompanyId, WarehouseId,	[Status])
SELECT 	N'Receipt', @MainCurrencyUnit, 0, 0, w.CompanyId,w.Id, N'Not Priced'
	FROM Inventory.Warehouse w   
	WHERE (@WarehouseId IS NULL OR w.Id=@WarehouseId) AND (@CompanyId IS NULL OR w.CompanyId=@CompanyId) 

INSERT INTO @Template(	[Type], MainCurrencyUnit, QuantityAmount,	TotalPrice,	CompanyId, WarehouseId,	[Status])
SELECT 	N'Issue', @MainCurrencyUnit, 0, 0, w.CompanyId,w.Id, N'Total'
	FROM Inventory.Warehouse w   
	WHERE (@WarehouseId IS NULL OR w.Id=@WarehouseId) AND (@CompanyId IS NULL OR w.CompanyId=@CompanyId) 

INSERT INTO @Template(	[Type], MainCurrencyUnit, QuantityAmount,	TotalPrice,	CompanyId, WarehouseId,	[Status])
SELECT 	N'Issue', @MainCurrencyUnit, 0, 0, w.CompanyId,w.Id, N'Priced'
	FROM Inventory.Warehouse w   
	WHERE (@WarehouseId IS NULL OR w.Id=@WarehouseId) AND (@CompanyId IS NULL OR w.CompanyId=@CompanyId) 

INSERT INTO @Template(	[Type], MainCurrencyUnit, QuantityAmount,	TotalPrice,	CompanyId, WarehouseId,	[Status])
SELECT 	N'Issue', @MainCurrencyUnit, 0, 0, w.CompanyId,w.Id, N'Not Priced'
	FROM Inventory.Warehouse w   
	WHERE (@WarehouseId IS NULL OR w.Id=@WarehouseId) AND (@CompanyId IS NULL OR w.CompanyId=@CompanyId) 

INSERT INTO @Template(	[Type], MainCurrencyUnit, QuantityAmount,	TotalPrice,	CompanyId, WarehouseId,	[Status])
SELECT 	N'InventoryStockStart', @MainCurrencyUnit, 0, 0, w.CompanyId,w.Id, N'Total'
	FROM Inventory.Warehouse w   
	WHERE (@WarehouseId IS NULL OR w.Id=@WarehouseId) AND (@CompanyId IS NULL OR w.CompanyId=@CompanyId) 

INSERT INTO @Template(	[Type], MainCurrencyUnit, QuantityAmount,	TotalPrice,	CompanyId, WarehouseId,	[Status])
SELECT 	N'InventoryStockStart', @MainCurrencyUnit, 0, 0, w.CompanyId,w.Id, N'Priced'
	FROM Inventory.Warehouse w   
	WHERE (@WarehouseId IS NULL OR w.Id=@WarehouseId) AND (@CompanyId IS NULL OR w.CompanyId=@CompanyId) 

INSERT INTO @Template(	[Type], MainCurrencyUnit, QuantityAmount,	TotalPrice,	CompanyId, WarehouseId,	[Status])
SELECT 	N'InventoryStockStart', @MainCurrencyUnit, 0, 0, w.CompanyId,w.Id, N'Not Priced'
	FROM Inventory.Warehouse w   
	WHERE (@WarehouseId IS NULL OR w.Id=@WarehouseId) AND (@CompanyId IS NULL OR w.CompanyId=@CompanyId) 


INSERT INTO @Template(	[Type], MainCurrencyUnit, QuantityAmount,	TotalPrice,	CompanyId, WarehouseId,	[Status])
	SELECT 	N'InventoryStockStart', @MainCurrencyUnit, 0, 0, w.CompanyId,w.Id, N'SuspendedIssue'
	FROM Inventory.Warehouse w   
	WHERE (@WarehouseId IS NULL OR w.Id=@WarehouseId) AND (@CompanyId IS NULL OR w.CompanyId=@CompanyId) 
	
INSERT INTO @Template(	[Type], MainCurrencyUnit, QuantityAmount,	TotalPrice,	CompanyId, WarehouseId,	[Status])
SELECT 	N'InventoryStockEnd', @MainCurrencyUnit, 0, 0, w.CompanyId,w.Id, N'Total'
	FROM Inventory.Warehouse w   
	WHERE (@WarehouseId IS NULL OR w.Id=@WarehouseId) AND (@CompanyId IS NULL OR w.CompanyId=@CompanyId) 

INSERT INTO @Template(	[Type], MainCurrencyUnit, QuantityAmount,	TotalPrice,	CompanyId, WarehouseId,	[Status])
SELECT 	N'InventoryStockEnd', @MainCurrencyUnit, 0, 0, w.CompanyId,w.Id, N'Priced'
	FROM Inventory.Warehouse w   
	WHERE (@WarehouseId IS NULL OR w.Id=@WarehouseId) AND (@CompanyId IS NULL OR w.CompanyId=@CompanyId) 

INSERT INTO @Template(	[Type], MainCurrencyUnit, QuantityAmount,	TotalPrice,	CompanyId, WarehouseId,	[Status])
SELECT 	N'InventoryStockEnd', @MainCurrencyUnit, 0, 0, w.CompanyId,w.Id, N'Not Priced'
	FROM Inventory.Warehouse w   
	WHERE (@WarehouseId IS NULL OR w.Id=@WarehouseId) AND (@CompanyId IS NULL OR w.CompanyId=@CompanyId) 

INSERT INTO @Template(	[Type], MainCurrencyUnit, QuantityAmount,	TotalPrice,	CompanyId, WarehouseId,	[Status])
	SELECT 	N'InventoryStockEnd', @MainCurrencyUnit, 0, 0, w.CompanyId,w.Id, N'SuspendedIssue'
	FROM Inventory.Warehouse w   
	WHERE (@WarehouseId IS NULL OR w.Id=@WarehouseId) AND (@CompanyId IS NULL OR w.CompanyId=@CompanyId) 

--************************** Total Quantity TransactionItems    RECEIPT
INSERT INTO @Cardex
(
	[Type],
	GoodId,
	GoodCode,
	GoodName,
	QuantityUnit,
	MainCurrencyUnit,
	QuantityAmount,
	TotalPrice,
	CompanyId,  
	WarehouseId,
	Status
)
SELECT N'Receipt'                      AS [Type],
       g.Id                     AS GoodId,
       g.Code,
       g.Name,
       u.Name                   AS QuantityUnit,
       NULL,
       SUM(ti.QuantityAmount) AS TotalQuantityAmount,
       NULL,
       c.Id AS CompanyId,
       w.Id AS WarehouseId,
       --@CompanyId,
       --@WarehouseId,
       N'Total'
FROM   Inventory.Transactions t
       INNER JOIN Inventory.TransactionItems ti
            ON  ti.TransactionId = t.Id
       INNER JOIN Inventory.Warehouse w ON w.Id = t.WarehouseId AND (@WarehouseId IS NULL OR w.Id=@WarehouseId)
       INNER JOIN Inventory.Companies c ON c.Id = w.CompanyId AND (@CompanyId IS NULL OR c.Id=@CompanyId)
       INNER JOIN Inventory.Goods g
            ON  g.Id = ti.GoodId
       INNER JOIN Inventory.Units u
            ON  u.Id = ti.QuantityUnitId
            AND u.IsCurrency = 0 AND u.Id = @QuantityUnitId
WHERE t.[Action] = 1 
      AND (@GoodId IS NULL OR ti.GoodId=@GoodId)
      AND t.RegistrationDate BETWEEN @From AND @To
GROUP BY
       g.Id,
       g.Code,
       g.Name,
       u.Name,
       c.Id,
	   w.Id
--************************** Total Quantity TransactionItems       RECEIPT
--IF(NOT EXISTS(SELECT * FROM @Cardex c WHERE c.[Type] = N'Receipt'))
--BEGIN
	
--END

--************************** Quantity & Priced TransactionItems    RECEIPT
INSERT INTO @Cardex
(
	[Type],
	GoodId,
	GoodCode,
	GoodName,
	QuantityUnit,
	MainCurrencyUnit,
	QuantityAmount,
	TotalPrice,
	CompanyId,  
	WarehouseId,
	Status
)
SELECT N'Receipt'                      AS [Type],
       g.Id                     AS GoodId,
       g.Code,
       g.Name,
       u.Name                   AS QuantityUnit,
       u2.Name                  AS MainCurrencyUnit,
       SUM(tip.QuantityAmount)  AS QuantityAmount,
       SUM(tip.FeeInMainCurrency * tip.QuantityAmount) AS TotalPrice,
	   c.Id AS CompanyId,
       w.Id AS WarehouseId,
       --@CompanyId,
       --@WarehouseId,
       N'Priced'
FROM   Inventory.Transactions t
       INNER JOIN Inventory.TransactionItems ti
            ON  ti.TransactionId = t.Id
       INNER JOIN Inventory.TransactionItemPrices tip
            ON  tip.TransactionItemId = ti.Id
       INNER JOIN Inventory.Warehouse w ON w.Id = t.WarehouseId AND (@WarehouseId IS NULL OR w.Id=@WarehouseId)
       INNER JOIN Inventory.Companies c ON c.Id = w.CompanyId AND (@CompanyId IS NULL OR c.Id=@CompanyId)
       INNER JOIN Inventory.Goods g
            ON  g.Id = ti.GoodId
       INNER JOIN Inventory.Units u
            ON  u.Id = ti.QuantityUnitId
            AND u.IsCurrency = 0 AND u.Id = @QuantityUnitId
       INNER JOIN Inventory.Units u2
            ON  u2.Id = tip.MainCurrencyUnitId
            AND u2.IsCurrency = 1
WHERE t.[Action] = 1 
      AND (@GoodId IS NULL OR ti.GoodId=@GoodId)
      AND t.RegistrationDate BETWEEN @From AND @To
GROUP BY
       g.Id,
       g.Code,
       g.Name,
       u.Name,
       u2.Name,
       c.Id,
	   w.Id
--************************** Quantity & Priced TransactionItems    RECEIPT

--************************** Quantity & Not Priced TransactionItems     RECEIPT
INSERT INTO @Cardex
(
	[Type],
	GoodId,
	GoodCode,
	GoodName,
	QuantityUnit,
	MainCurrencyUnit,
	QuantityAmount,
	TotalPrice,
	CompanyId,  
	WarehouseId,
	Status
)
SELECT
	c.[Type],
	c.GoodId,
	c.GoodCode,
	c.GoodName,
	c.QuantityUnit,
	NULL,
	SUM(c.QuantityAmount * CASE 
	                           WHEN c.[Status]=N'Total' THEN 1
	                           WHEN c.[Status]=N'Priced' THEN -1
	                           ELSE 0 
	                       END) ,
	NULL,
	c.CompanyId,  
	c.WarehouseId,
	N'Not Priced'
FROM @Cardex c
WHERE c.[Type]= N'Receipt'
GROUP BY	
    c.[Type],
	c.GoodId,
	c.GoodCode,
	c.GoodName,
	c.QuantityUnit,
	c.CompanyId,  
	c.WarehouseId
--************************** Quantity & Not Priced TransactionItems    RECEIPT

--************************** Total Quantity TransactionItems     ISSUE
INSERT INTO @Cardex
(
	[Type],
	GoodId,
	GoodCode,
	GoodName,
	QuantityUnit,
	MainCurrencyUnit,
	QuantityAmount,
	TotalPrice,
	CompanyId,  
	WarehouseId,
	Status
)
SELECT N'Issue'                      AS [Type],
       g.Id                     AS GoodId,
       g.Code,
       g.Name,
       u.Name                   AS QuantityUnit,
       NULL,
       SUM(ti.QuantityAmount) AS TotalQuantityAmount,
       NULL,
      c.Id AS CompanyId,
       w.Id AS WarehouseId,
       --@CompanyId,
       --@WarehouseId,
       N'Total'
FROM   Inventory.Transactions t
       INNER JOIN Inventory.TransactionItems ti
            ON  ti.TransactionId = t.Id
       INNER JOIN Inventory.Warehouse w ON w.Id = t.WarehouseId AND (@WarehouseId IS NULL OR w.Id=@WarehouseId)
       INNER JOIN Inventory.Companies c ON c.Id = w.CompanyId AND (@CompanyId IS NULL OR c.Id=@CompanyId)
       INNER JOIN Inventory.Goods g
            ON  g.Id = ti.GoodId
       INNER JOIN Inventory.Units u
            ON  u.Id = ti.QuantityUnitId
            AND u.IsCurrency = 0 AND u.Id = @QuantityUnitId
WHERE t.[Action] = 2
      AND (@GoodId IS NULL OR ti.GoodId=@GoodId)
      AND t.RegistrationDate BETWEEN @From AND @To
GROUP BY
       g.Id,
       g.Code,
       g.Name,
       u.Name,
       c.Id,
	   w.Id
--************************** Total Quantity TransactionItems      ISSUE
--************************** Quantity & Priced TransactionItems   ISSUE
INSERT INTO @Cardex
(
	[Type],
	GoodId,
	GoodCode,
	GoodName,
	QuantityUnit,
	MainCurrencyUnit,
	QuantityAmount,
	TotalPrice,
	CompanyId,  
	WarehouseId,
	Status
)
SELECT N'Issue'                      AS [Type],
       g.Id                     AS GoodId,
       g.Code,
       g.Name,
       u.Name                   AS QuantityUnit,
       u2.Name                  AS MainCurrencyUnit,
       SUM(tip.QuantityAmount)  AS QuantityAmount,
       SUM(tip.FeeInMainCurrency * tip.QuantityAmount) AS TotalPrice,
      c.Id AS CompanyId,
       w.Id AS WarehouseId,
       --@CompanyId,
       --@WarehouseId,
       N'Priced'
FROM   Inventory.Transactions t
       INNER JOIN Inventory.TransactionItems ti
            ON  ti.TransactionId = t.Id
       INNER JOIN Inventory.TransactionItemPrices tip
            ON  tip.TransactionItemId = ti.Id
       INNER JOIN Inventory.Warehouse w ON w.Id = t.WarehouseId AND (@WarehouseId IS NULL OR w.Id=@WarehouseId)
       INNER JOIN Inventory.Companies c ON c.Id = w.CompanyId AND (@CompanyId IS NULL OR c.Id=@CompanyId)
       INNER JOIN Inventory.Goods g
            ON  g.Id = ti.GoodId
       INNER JOIN Inventory.Units u
            ON  u.Id = ti.QuantityUnitId
            AND u.IsCurrency = 0  AND u.Id = @QuantityUnitId
       INNER JOIN Inventory.Units u2
            ON  u2.Id = tip.MainCurrencyUnitId
            AND u2.IsCurrency = 1
WHERE t.[Action] = 2 
      AND (@GoodId IS NULL OR ti.GoodId=@GoodId)
      AND t.RegistrationDate BETWEEN @From AND @To
GROUP BY
       g.Id,
       g.Code,
       g.Name,
       u.Name,
       u2.Name,
       c.Id,
	   w.Id
--************************** Quantity & Priced TransactionItems   ISSUE
--************************** Quantity & Not Priced TransactionItems   ISSUE
INSERT INTO @Cardex
(
	[Type],
	GoodId,
	GoodCode,
	GoodName,
	QuantityUnit,
	MainCurrencyUnit,
	QuantityAmount,
	TotalPrice,
	CompanyId,  
	WarehouseId,
	Status
)
SELECT
	c.[Type],
	c.GoodId,
	c.GoodCode,
	c.GoodName,
	c.QuantityUnit,
	NULL,
	SUM(c.QuantityAmount * CASE 
	                           WHEN c.[Status]=N'Total' THEN 1
	                           WHEN c.[Status]=N'Priced' THEN -1
	                           ELSE 0 
	                       END) ,
	NULL,
	c.CompanyId,  
	c.WarehouseId,
	N'Not Priced'
FROM @Cardex c
WHERE c.[Type]= N'Issue'
GROUP BY	
    c.[Type],
	c.GoodId,
	c.GoodCode,
	c.GoodName,
	c.QuantityUnit,
	c.CompanyId,  
	c.WarehouseId
--************************** Quantity & Not Priced TransactionItems  ISSUE

--************************** InventoryStock Start Total 
INSERT INTO @Cardex
(
	[Type],
	GoodId,
	GoodCode,
	GoodName,
	QuantityUnit,
	MainCurrencyUnit,
	QuantityAmount,
	TotalPrice,
	CompanyId,  
	WarehouseId,
	Status
)
SELECT N'InventoryStockStart' AS [Type],
       g.Id                     AS GoodId,
       g.Code,
       g.Name,
       u.Name                   AS QuantityUnit,
       NULL,
       SUM(ISNULL(ti.QuantityAmount*(CASE WHEN t.[Action]=1 THEN 1 WHEN t.[Action]=2 THEN -1 END),0))   AS QuantityAmount,
       NULL,
       c.Id AS CompanyId,
       w.Id AS WarehouseId,
       --@CompanyId,
       --@WarehouseId,
       --N'InventoryStockTotal'
       N'Total'
FROM   Inventory.Transactions t
       INNER JOIN Inventory.TransactionItems ti
            ON  ti.TransactionId = t.Id
       INNER JOIN Inventory.Warehouse w ON w.Id = t.WarehouseId AND (@WarehouseId IS NULL OR w.Id=@WarehouseId)
       INNER JOIN Inventory.Companies c ON c.Id = w.CompanyId AND (@CompanyId IS NULL OR c.Id=@CompanyId)
       INNER JOIN Inventory.Goods g
            ON  g.Id = ti.GoodId
       INNER JOIN Inventory.Units u
            ON  u.Id = ti.QuantityUnitId
            AND u.IsCurrency = 0 AND u.Id = @QuantityUnitId
WHERE (@GoodId IS NULL OR ti.GoodId=@GoodId)
      AND t.RegistrationDate BETWEEN @StatrtFiscalYear AND DATEADD(SECOND,-1,@From)
GROUP BY
       g.Id,
       g.Code,
       g.Name,
       u.Name,
       c.Id,
	   w.Id
--************************** InventoryStock Start Total 
--************************** InventoryStock Start Priced 
INSERT INTO @Cardex
(
	[Type],
	GoodId,
	GoodCode,
	GoodName,
	QuantityUnit,
	MainCurrencyUnit,
	QuantityAmount,
	TotalPrice,
	CompanyId,  
	WarehouseId,
	Status
)
SELECT N'InventoryStockStart' AS [Type],
   g.Id                     AS GoodId,
       g.Code,
       g.Name,
       u.Name                   AS QuantityUnit,
       u2.Name                  AS MainCurrencyUnit,
       SUM(ISNULL(tip.QuantityAmount*(CASE WHEN t.[Action]=1 THEN 1 WHEN t.[Action]=2 THEN -1 END),0))   AS QuantityAmount,
       SUM(ISNULL(tip.FeeInMainCurrency,0)*ISNULL(tip.QuantityAmount,0)*(CASE WHEN t.[Action]=1 THEN 1 WHEN t.[Action]=2 THEN -1 END)) AS TotalPrice,
       c.Id AS CompanyId,
       w.Id AS WarehouseId,
       --@CompanyId,
       --@WarehouseId,
       --N'InventoryStockPriced'
       N'Priced'
FROM   Inventory.Transactions t
       INNER JOIN Inventory.TransactionItems ti
            ON  ti.TransactionId = t.Id
       INNER JOIN Inventory.TransactionItemPrices tip
            ON  tip.TransactionItemId = ti.Id
       INNER JOIN Inventory.Warehouse w ON w.Id = t.WarehouseId AND (@WarehouseId IS NULL OR w.Id=@WarehouseId)
       INNER JOIN Inventory.Companies c ON c.Id = w.CompanyId AND (@CompanyId IS NULL OR c.Id=@CompanyId)
       INNER JOIN Inventory.Goods g
            ON  g.Id = ti.GoodId
       INNER JOIN Inventory.Units u
            ON  u.Id = ti.QuantityUnitId
            AND u.IsCurrency = 0 AND u.Id = @QuantityUnitId
       INNER JOIN Inventory.Units u2
            ON  u2.Id = tip.MainCurrencyUnitId
            AND u2.IsCurrency = 1
WHERE (@GoodId IS NULL OR ti.GoodId=@GoodId)
      AND t.RegistrationDate BETWEEN @StatrtFiscalYear AND DATEADD(SECOND,-1,@From)
GROUP BY
       g.Id,
       g.Code,
       g.Name,
       u.Name,
       u2.Name,
       c.Id,
	   w.Id
--************************** InventoryStock Start Priced 
--************************** InventoryStock Start Not Priced 
INSERT INTO @Cardex
(
	[Type],
	GoodId,
	GoodCode,
	GoodName,
	QuantityUnit,
	MainCurrencyUnit,
	QuantityAmount,
	TotalPrice,
	CompanyId,  
	WarehouseId,
	Status
)
SELECT
	c.[Type],
	c.GoodId,
	c.GoodCode,
	c.GoodName,
	c.QuantityUnit,
	NULL,
	SUM(c.QuantityAmount * CASE 
	                           --WHEN c.[Status]=N'InventoryStockTotal' THEN 1
	                           --WHEN c.[Status]=N'InventoryStockPriced' THEN -1
	                            WHEN c.[Status]=N'Total' THEN 1
	                           WHEN c.[Status]=N'Priced' THEN -1
	                           ELSE 0 
	                       END) ,
	NULL,
	c.CompanyId,  
	c.WarehouseId,
	--N'InventoryStock Not Priced'
	N'Not Priced'
FROM @Cardex c
WHERE c.[Type]= N'InventoryStockStart' 
GROUP BY	
    c.[Type],
	c.GoodId,
	c.GoodCode,
	c.GoodName,
	c.QuantityUnit,
	c.CompanyId,  
	c.WarehouseId
--************************** InventoryStock Start Not Priced 

--************************** InventoryStock End Total 
INSERT INTO @Cardex
(
	[Type],
	GoodId,
	GoodCode,
	GoodName,
	QuantityUnit,
	MainCurrencyUnit,
	QuantityAmount,
	TotalPrice,
	CompanyId,  
	WarehouseId,
	Status
)
SELECT N'InventoryStockEnd' AS [Type],
       g.Id                     AS GoodId,
       g.Code,
       g.Name,
       u.Name                   AS QuantityUnit,
       NULL,
       SUM(ISNULL(ti.QuantityAmount*(CASE WHEN t.[Action]=1 THEN 1 WHEN t.[Action]=2 THEN -1 END),0))   AS QuantityAmount,
       NULL,
       c.Id AS CompanyId,
       w.Id AS WarehouseId,
       --@CompanyId,
       --@WarehouseId,
       --N'InventoryStockTotal'
       N'Total'
FROM   Inventory.Transactions t
       INNER JOIN Inventory.TransactionItems ti
            ON  ti.TransactionId = t.Id
       INNER JOIN Inventory.Warehouse w ON w.Id = t.WarehouseId AND (@WarehouseId IS NULL OR w.Id=@WarehouseId)
       INNER JOIN Inventory.Companies c ON c.Id = w.CompanyId AND (@CompanyId IS NULL OR c.Id=@CompanyId)
       INNER JOIN Inventory.Goods g
            ON  g.Id = ti.GoodId
       INNER JOIN Inventory.Units u
            ON  u.Id = ti.QuantityUnitId
            AND u.IsCurrency = 0 AND u.Id = @QuantityUnitId
WHERE (@GoodId IS NULL OR ti.GoodId=@GoodId)
      AND t.RegistrationDate BETWEEN @StatrtFiscalYear AND @To
GROUP BY
       g.Id,
       g.Code,
       g.Name,
       u.Name,
       c.Id,
	   w.Id
--************************** InventoryStock End Total 
--************************** InventoryStock End Priced 
INSERT INTO @Cardex
(
	[Type],
	GoodId,
	GoodCode,
	GoodName,
	QuantityUnit,
	MainCurrencyUnit,
	QuantityAmount,
	TotalPrice,
	CompanyId,  
	WarehouseId,
	Status
)
SELECT N'InventoryStockEnd' AS [Type],
   g.Id                     AS GoodId,
       g.Code,
       g.Name,
       u.Name                   AS QuantityUnit,
       u2.Name                  AS MainCurrencyUnit,
       SUM(ISNULL(tip.QuantityAmount*(CASE WHEN t.[Action]=1 THEN 1 WHEN t.[Action]=2 THEN -1 END),0))   AS QuantityAmount,
       SUM(ISNULL(tip.FeeInMainCurrency,0)*ISNULL(tip.QuantityAmount,0)*(CASE WHEN t.[Action]=1 THEN 1 WHEN t.[Action]=2 THEN -1 END)) AS TotalPrice,
       c.Id AS CompanyId,
       w.Id AS WarehouseId,
       --@CompanyId,
       --@WarehouseId,
       --N'InventoryStockPriced'
       N'Priced'
FROM   Inventory.Transactions t
       INNER JOIN Inventory.TransactionItems ti
            ON  ti.TransactionId = t.Id
       INNER JOIN Inventory.TransactionItemPrices tip
            ON  tip.TransactionItemId = ti.Id
       INNER JOIN Inventory.Warehouse w ON w.Id = t.WarehouseId AND (@WarehouseId IS NULL OR w.Id=@WarehouseId)
       INNER JOIN Inventory.Companies c ON c.Id = w.CompanyId AND (@CompanyId IS NULL OR c.Id=@CompanyId)
       INNER JOIN Inventory.Goods g
            ON  g.Id = ti.GoodId
       INNER JOIN Inventory.Units u
            ON  u.Id = ti.QuantityUnitId
            AND u.IsCurrency = 0 AND u.Id = @QuantityUnitId
       INNER JOIN Inventory.Units u2
            ON  u2.Id = tip.MainCurrencyUnitId
            AND u2.IsCurrency = 1
WHERE (@GoodId IS NULL OR ti.GoodId=@GoodId)
      AND t.RegistrationDate BETWEEN @StatrtFiscalYear AND @To
GROUP BY
       g.Id,
       g.Code,
       g.Name,
       u.Name,
       u2.Name,
       c.Id,
	   w.Id
--************************** InventoryStock End Priced 
--************************** InventoryStock End Not Priced 
INSERT INTO @Cardex
(
	[Type],
	GoodId,
	GoodCode,
	GoodName,
	QuantityUnit,
	MainCurrencyUnit,
	QuantityAmount,
	TotalPrice,
	CompanyId,  
	WarehouseId,
	Status
)
SELECT
	c.[Type],
	c.GoodId,
	c.GoodCode,
	c.GoodName,
	c.QuantityUnit,
	NULL,
	SUM(c.QuantityAmount * CASE 
	                           --WHEN c.[Status]=N'InventoryStockTotal' THEN 1
	                           --WHEN c.[Status]=N'InventoryStockPriced' THEN -1
	                           WHEN c.[Status]=N'Total' THEN 1
	                           WHEN c.[Status]=N'Priced' THEN -1
	                           ELSE 0 
	                       END) ,
	NULL,
	c.CompanyId,  
	c.WarehouseId,
	--N'InventoryStock Not Priced'
	N'Not Priced'
FROM @Cardex c
WHERE c.[Type]= N'InventoryStockEnd' 
GROUP BY	
    c.[Type],
	c.GoodId,
	c.GoodCode,
	c.GoodName,
	c.QuantityUnit,
	c.CompanyId,  
	c.WarehouseId
--************************** InventoryStock End Not Priced 

--************************** InventoryStock Start Suspended
INSERT INTO @Cardex
(
	[Type],
	GoodId,
	GoodCode,
	GoodName,
	QuantityUnit,
	MainCurrencyUnit,
	QuantityAmount,
	TotalPrice,
	CompanyId,  
	WarehouseId,
	Status
)
SELECT
		N'InventoryStockStart' AS [Type],
	   g.Id                     AS GoodId,
       g.Code					AS GoodCode,
       g.Name					AS GoodName,
       u.Name                   AS QuantityUnit,
       @MainCurrencyUnit		AS MainCurrencyUnit,
       SUM(isnull(frd.Consumption, 0)) AS QuantityAmount,
       0                        AS TotalPrice,
       w.CompanyId AS CompanyId,
       w.Id AS WarehouseId,
       --@CompanyId,
       --@WarehouseId,
       --N'InventoryStockPriced'
       N'SuspendedIssue'
FROM Fuel.FuelReport fr 
	INNER JOIN Fuel.FuelReportDetail frd ON frd.FuelReportId = fr.Id
	INNER JOIN BasicInfo.CompanyVesselView cvv ON fr.VesselInCompanyId = cvv.Id 
	INNER JOIN Inventory.Warehouse w ON w.Code = cvv.Code AND w.CompanyId = cvv.CompanyId AND (@WarehouseId IS NULL OR w.Id = @WarehouseId)
	INNER JOIN BasicInfo.CompanyGoodView cgv ON cgv.Id = frd.GoodId AND (@CompanyId IS NULL OR cgv.CompanyId = @CompanyId) 
	INNER JOIN Inventory.Goods g ON g.Code = cgv.Code AND (@GoodId IS NULL OR g.Id = @GoodId) 
	INNER JOIN BasicInfo.CompanyGoodUnitView cguv ON frd.MeasuringUnitId = cguv.Id
	INNER JOIN Inventory.Units u ON u.IsCurrency = 0 AND u.Abbreviation = cguv.Abbreviation
	--(SELECT DISTINCT 
	--	fr.VesselInCompanyId,
	--		   FIRST_VALUE(fr.EventDate) OVER(
	--			   PARTITION BY fr.VesselInCompanyId
	--			   ORDER BY fr.EventDate DESC
	--			   ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
	--		   )  AS LastFRId
	--	 FROM Fuel.FuelReport fr
	--	WHERE fr.EventDate < @From AND fr.FuelReportType IN (2, 5, 6)
	--) LastFuelReport lfr 
WHERE fr.EventDate > (SELECT TOP 1  
			   FIRST_VALUE(fr2.EventDate) OVER(
				   PARTITION BY fr2.VesselInCompanyId
				   ORDER BY fr2.EventDate DESC
				   ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
			   )  AS LastFRId
		 FROM Fuel.FuelReport fr2
		WHERE fr2.EventDate < @From AND fr2.FuelReportType IN (2, 5, 6) AND fr2.VesselInCompanyId = fr.VesselInCompanyId)
 AND fr.EventDate < @From AND fr.[State] = 1
GROUP BY 
	frd.GoodId, fr.VesselInCompanyId,
	g.Id ,
	g.Code ,
	g.Name ,
	u.Name ,
	w.CompanyId ,
	w.Id

--************************** InventoryStock Start Suspended

--************************** Update InventoryStock Start Total
UPDATE cdx 
	SET
		QuantityAmount = cdx.QuantityAmount + (c.QuantityAmount * -1)
	FROM
	@Cardex cdx
    INNER JOIN
    @Cardex c ON cdx.[Type] = c.[Type] AND cdx.GoodId = c.GoodId AND cdx.CompanyId = c.CompanyId AND cdx.WarehouseId = c.WarehouseId  
WHERE
    cdx.[Type] = N'InventoryStockStart' AND cdx.[Status] = N'Total' AND c.[Status] = N'SuspendedIssue' 

--************************** Update InventoryStock Start Total

--************************** InventoryStock End Suspended
INSERT INTO @Cardex
(
	[Type],
	GoodId,
	GoodCode,
	GoodName,
	QuantityUnit,
	MainCurrencyUnit,
	QuantityAmount,
	TotalPrice,
	CompanyId,  
	WarehouseId,
	Status
)
SELECT
		N'InventoryStockEnd' AS [Type],
	   g.Id                     AS GoodId,
       g.Code					AS GoodCode,
       g.Name					AS GoodName,
       u.Name                   AS QuantityUnit,
       @MainCurrencyUnit		AS MainCurrencyUnit,
       SUM(isnull(frd.Consumption, 0)) AS QuantityAmount,
       0                        AS TotalPrice,
       w.CompanyId AS CompanyId,
       w.Id AS WarehouseId,
       --@CompanyId,
       --@WarehouseId,
       --N'InventoryStockPriced'
       N'SuspendedIssue'

FROM Fuel.FuelReport fr 
	INNER JOIN Fuel.FuelReportDetail frd ON frd.FuelReportId = fr.Id
	INNER JOIN BasicInfo.CompanyVesselView cvv ON fr.VesselInCompanyId = cvv.Id 
	INNER JOIN Inventory.Warehouse w ON w.Code = cvv.Code AND w.CompanyId = cvv.CompanyId AND (@WarehouseId IS NULL OR w.Id = @WarehouseId)
	INNER JOIN BasicInfo.CompanyGoodView cgv ON cgv.Id = frd.GoodId AND (@CompanyId IS NULL OR cgv.CompanyId = @CompanyId) 
	INNER JOIN Inventory.Goods g ON g.Code = cgv.Code AND (@GoodId IS NULL OR g.Id = @GoodId) 
	INNER JOIN BasicInfo.CompanyGoodUnitView cguv ON frd.MeasuringUnitId = cguv.Id
	INNER JOIN Inventory.Units u ON u.IsCurrency = 0 AND u.Abbreviation = cguv.Abbreviation 
WHERE fr.EventDate > (SELECT TOP 1  
			   FIRST_VALUE(fr2.EventDate) OVER(
				   PARTITION BY fr2.VesselInCompanyId
				   ORDER BY fr2.EventDate DESC
				   ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
			   )  AS LastFRId
		 FROM Fuel.FuelReport fr2
		WHERE fr2.EventDate < @From AND fr2.FuelReportType IN (2, 5, 6) AND fr2.VesselInCompanyId = fr.VesselInCompanyId)
	AND fr.EventDate > @From
	AND fr.EventDate < @To AND fr.[State] = 1
GROUP BY 
	frd.GoodId, fr.VesselInCompanyId,
	g.Id ,
	g.Code,
	g.Name,
	u.Name ,
	w.CompanyId,
	w.Id
--************************** InventoryStock End Suspended

--************************** Update InventoryStock End Total
UPDATE cdx 
	SET
		QuantityAmount = cdx.QuantityAmount + (c.QuantityAmount * -1)
	FROM
	@Cardex cdx
    INNER JOIN
    @Cardex c ON cdx.[Type] = c.[Type] AND cdx.GoodId = c.GoodId AND cdx.CompanyId = c.CompanyId AND cdx.WarehouseId = c.WarehouseId  
WHERE
    cdx.[Type] = N'InventoryStockEnd' AND cdx.[Status] = N'Total' AND c.[Status] = N'SuspendedIssue' 

--************************** Update InventoryStock End Total

SELECT ReportQuery.*, 
CASE ReportQuery.[Type] 
WHEN 'InventoryStockStart' THEN 1 
WHEN 'Receipt' THEN 2 
WHEN 'InventoryStockEnd' THEN 3
WHEN 'Issue' THEN 4 END AS TypeId ,

CASE ReportQuery.[Type] 
WHEN 'InventoryStockStart' THEN 1 
WHEN 'Receipt' THEN 2 
WHEN 'InventoryStockEnd' THEN 3
WHEN 'Issue' THEN 4 END AS TypeDisplayOrder, 
invc.Name AS CompanyName, invw.Name AS WarehouseName FROM 
(SELECT ISNULL(c.[Type], gt.[Type])        AS [Type],
       ISNULL(c.GoodId, gt.GoodId)        AS GoodId,
       ISNULL(c.GoodCode, gt.GoodCode)    AS GoodCode,
       ISNULL(c.GoodName, gt.GoodName)    AS GoodName,
       ISNULL(c.QuantityUnit, gt.QuantityUnit) AS QuantityUnit,
       ISNULL(c.MainCurrencyUnit, gt.MainCurrencyUnit) AS MainCurrencyUnit,
       ISNULL(c.QuantityAmount, gt.QuantityAmount) AS QuantityAmount,
       ISNULL(c.TotalPrice, gt.TotalPrice) AS TotalPrice,
       ISNULL(c.CompanyId, gt.CompanyId)  AS CompanyId,
       ISNULL(c.WarehouseId, gt.WarehouseId) AS WarehouseId,
       ISNULL(c.Status, gt.Status)        AS [Status] 
FROM @Cardex c RIGHT JOIN (
SELECT 
	t.[Type]           [Type], 
	g.Id			   GoodId ,
	g.Code			   GoodCode ,
	g.Name			   GoodName ,
	u.Name			   QuantityUnit ,
	t.MainCurrencyUnit MainCurrencyUnit,
	t.QuantityAmount   QuantityAmount ,
	t.TotalPrice	   TotalPrice ,
	t.CompanyId  	   CompanyId   ,
	t.WarehouseId	   WarehouseId ,
	t.Status		   Status 
FROM   @Template t
       CROSS JOIN Inventory.Goods g
       INNER JOIN Inventory.Units u ON u.Id = g.MainUnitId
) gt ON c.[Type] = gt.[Type] AND c.GoodId = gt.GoodId AND c.[Status] = gt.[Status] AND c.CompanyId = gt.CompanyId AND c.WarehouseId = gt.WarehouseId
) ReportQuery INNER JOIN Inventory.Companies invc ON ReportQuery.CompanyId = invc.Id 
INNER JOIN Inventory.Warehouse invw ON ReportQuery.WarehouseId = invw.Id
END
-- [Fuel].[PeriodicalFuelStatistics] @CompanyId =3,@WarehouseId =18,@QuantityUnitId =1 ,@From =N'2014/10/01',@To =N'2014/12/29'
GO

GRANT EXECUTE ON [Fuel].[PeriodicalFuelStatistics] TO Public

----------------------------------------------------------------------------------------

