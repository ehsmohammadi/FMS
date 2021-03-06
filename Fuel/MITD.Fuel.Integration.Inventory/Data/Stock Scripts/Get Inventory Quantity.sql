DECLARE @UnitId BIGINT , @UnitName NVARCHAR(50);

SELECT TOP 1 @UnitId=Id , @UnitName = Name FROM Inventory.Units u WHERE u.IsCurrency = 0 AND UPPER(u.Abbreviation) = UPPER(@UnitAbbreviation);

/****** Script for SelectTopNRows command from SSMS  ******/
SELECT Good.[Code] AS GoodCode, Good.[Name] AS GoodName, SUM([Inventory].GetMojodi(Good.Id, Warehouse.Id, 1, NULL, NULL, NULL, NULL, @UnitId, NULL)) AS TotalQuantity
	,@UnitName  AS UnitName, COUNT(*) AS VesselsCount
FROM [Inventory].[Goods] Good CROSS JOIN
   Inventory.Warehouse Warehouse 
WHERE  (Warehouse.Id IN (@WarehouseId)) AND (Warehouse.CompanyId IN (@CompanyId)) AND Warehouse.IsActive = 1
GROUP BY Good.Code, Good.Name