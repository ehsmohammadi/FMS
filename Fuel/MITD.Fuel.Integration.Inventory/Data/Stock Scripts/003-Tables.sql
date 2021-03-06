--ALTER DATABASE MiniStock SET MULTI_USER
--USE MiniStock
--GO 
-------------------------------------
if not Exists(SELECT * FROM sys.objects WHERE object_Id = OBJECT_Id(N'[Inventory].[ErrorMessages]') AND type in (N'U'))
BEGIN
Create table [Inventory].ErrorMessages
(
	ErrorMessage NVARCHAR(200) NOT NULL,
	TextMessage NVARCHAR(200) NOT NULL,
	[Action] NVARCHAR(20),
	CONSTRAINT PK_Key PRIMARY KEY(ErrorMessage,TextMessage,Action)
)
END	
GO
---------------------User----------------
if not Exists(SELECT * FROM sys.objects WHERE object_Id = OBJECT_Id(N'[Inventory].[Users]') AND type in (N'U'))
BEGIN
Create table [Inventory].Users
(
	Id INT  NOT NULL,--IdENTITY(1,1)
	Code int NOT NULL, 
	Name nvarchar(100) NOT NULL,
	[User_Name] VARCHAR(256) NOT NULL,
	[Password] nvarchar(100) NOT NULL,
	IsActive BIT DEFAULT 1,
	Email_Address VARCHAR(256) NULL,
	IPAddress VARCHAR(15) NULL,
	Login BIT DEFAULT 0,
	SessionId NVARCHAR(88) NULL,
	UserCreatorId int NULL,
	CreateDate DATETIME DEFAULT getdate(),
	CONSTRAINT PK_Users_Id PRIMARY KEY(Id),
	CONSTRAINT UQ_Users_Code UNIQUE (Code),
	CONSTRAINT UQ_Users_User_Name UNIQUE ([User_Name]),
	CONSTRAINT FK_Users_UserCreatorId FOREIGN KEY(UserCreatorId) REFERENCES [Inventory].Users(Id)
) 
END
GO	
--------------------------------------------
if not Exists(SELECT * FROM sys.objects WHERE object_Id = OBJECT_Id(N'[Inventory].[FinancialYear]') AND type in (N'U'))
BEGIN
Create table [Inventory].FinancialYear
(
	Id INT IdENTITY(1,1) NOT NULL,
	Name nvarchar(256) NOT NULL,
	[StartDate] DATETIME NOT NULL,
	[EndDate] DATETIME NOT NULL,
	UserCreatorId INT NOT NULL,
	CreateDate DATETIME DEFAULT getdate(),
	--IsActive BIT DEFAULT 0,
	CONSTRAINT PK_FinancialYear_Id PRIMARY KEY(Id),
	CONSTRAINT UQ_FinancialYear_Name UNIQUE (Name),
    CONSTRAINT UQ_FinancialYear_StartDate UNIQUE ([StartDate]),
    CONSTRAINT UQ_FinancialYear_EndDate UNIQUE ([EndDate]),
	CONSTRAINT FK_FinancialYear_UserCreatorId FOREIGN KEY(UserCreatorId) REFERENCES [Inventory].Users(Id),
	CONSTRAINT chk_IsActualDateInFinancialYear CHECK ([Inventory].IsActualDateInFinancialYear([StartDate],[EndDate])<>0),
	CONSTRAINT chk_IsValIdDayInFinancialYear CHECK ([Inventory].IsValIdDayInFinancialYear(Id,[StartDate],[EndDate])<>0)
) 
END
GO	
-------------------------------------
if not Exists(SELECT * FROM sys.objects WHERE object_Id = OBJECT_Id(N'[Inventory].[TimeBucket]') AND type in (N'U'))
BEGIN
Create table [Inventory].TimeBucket
(
	Id INT IdENTITY(1,1) NOT NULL,
	Name nvarchar(256) NOT NULL,
	[StartDate] DATETIME  DEFAULT getdate(),
	[EndDate] DATETIME  DEFAULT getdate(),
	FinancialYearId INT NOT NULL,
	UserCreatorId INT NOT NULL,
	CreateDate DATETIME DEFAULT getdate(),
	IsActive BIT DEFAULT 0,
	CONSTRAINT PK_TimeBucket_Id PRIMARY KEY(Id),
	CONSTRAINT UQ_TimeBucket_Name_FinancialYearId UNIQUE (NAME,FinancialYearId),
    CONSTRAINT UQ_TimeBucket_StartDate UNIQUE ([StartDate]),
    CONSTRAINT UQ_TimeBucket_EndDate UNIQUE ([EndDate]),
	CONSTRAINT FK_TimeBucket_UserCreatorId FOREIGN KEY(UserCreatorId) REFERENCES [Inventory].Users(Id),
	CONSTRAINT FK_TimeBucket_FinancialYearId FOREIGN KEY(FinancialYearId) REFERENCES [Inventory].FinancialYear(Id),
	CONSTRAINT chk_IsActualDateInTimeBucket CHECK ([Inventory].IsActualDateInTimeBucket([StartDate],[EndDate])<>0),
	CONSTRAINT chk_IsValIdDateInTimeBucket CHECK ([Inventory].IsValIdDateInTimeBucket(Id,FinancialYearId,[StartDate])<>1),
	CONSTRAINT chk_IsValIdDateInTimeBucket_Than_FinancialYear CHECK ([Inventory].IsValIdDateInTimeBucket_Than_FinancialYear(Id,FinancialYearId,[StartDate],EndDate)<>1),
	CONSTRAINT chk_IsNoGapInTimeBucket CHECK ([Inventory].IsNoGapInTimeBucket(Id,FinancialYearId,[StartDate])<>1),
	CONSTRAINT chk_IsDiffDateGreaterThan29DaysInTimeBucket CHECK ([Inventory].IsDiffDateGreaterThan29DaysInTimeBucket([STARTDATE],EndDate)<>0),
	CONSTRAINT chk_IsStartDateEQStartYear_EndDateEQEndYearInTimeBucket CHECK ([Inventory].IsStartDateEQStartYear_EndDateEQEndYearInTimeBucket(Id,FinancialYearId,StartDate,EndDate)<>0)
) 
END
GO	
----------------------Company---------------
if not Exists(SELECT * FROM sys.objects WHERE object_Id = OBJECT_Id(N'[Inventory].[Companies]') AND type in (N'U'))
BEGIN
Create table [Inventory].Companies
(
	Id BIGINT  NOT NULL,--IdENTITY(1,1)
	Code  NVARCHAR(256) NOT NULL, 
	Name nvarchar(256) NOT NULL,
	IsActive BIT DEFAULT 1,
	UserCreatorId int,
	CreateDate DATETIME DEFAULT getdate(),
	CONSTRAINT PK_Companies_Id PRIMARY KEY(Id),
	CONSTRAINT UQ_Companies_Code UNIQUE (Code),
	CONSTRAINT UQ_Companies_Name UNIQUE ([Name]),
	CONSTRAINT FK_Companies_UserCreatorId FOREIGN KEY(UserCreatorId) REFERENCES [Inventory].Users(Id)
) 
END
GO
/************************* انبار **************************/
if not Exists(select * from sys.tables where UPPER(NAME) = UPPER(N'[Inventory].Warehouse'))
BEGIN
Create table [Inventory].Warehouse
(
	Id BIGINT Not Null,-- Identity(1,1)
	Code  NVARCHAR(128)	NOT NULL,
	Name NVARCHAR(256),
	CompanyId BIGINT Not NULL,
	IsActive BIT DEFAULT 1,
	UserCreatorId int,
	CreateDate DATETIME DEFAULT getdate(),
	CONSTRAINT PK_Warehouse_Id PRIMARY KEY(Id),
	CONSTRAINT UQ_Warehouse_Code UNIQUE (Code,NAME,CompanyId),
	CONSTRAINT FK_Warehouse_CompanyId FOREIGN KEY(CompanyId) REFERENCES [Inventory].Companies(Id),
	CONSTRAINT FK_Warehouse_UserCreatorId FOREIGN KEY(UserCreatorId) REFERENCES [Inventory].Users(Id)
)
END
GO
/************************* واحدهای اندازه گیری  **************************/
if not Exists(select * from sys.tables where UPPER(NAME) = UPPER(N'[Inventory].Units'))
BEGIN
Create table [Inventory].Units
(
	Id BIGINT Identity(1,1) Not Null,
	Abbreviation NVARCHAR(256) NOT NULL,
	Name NVARCHAR(256)NOT NUll,
	IsCurrency BIT DEFAULT 0,
	IsBaseCurrency BIT DEFAULT 0,
	IsActive bit default 1 NOT NULL,
	UserCreatorId int,
	CreateDate DATETIME DEFAULT getdate(),
	CONSTRAINT PK_Units_Id PRIMARY KEY(Id),
	CONSTRAINT UQ_Units_Code UNIQUE (Abbreviation),
	CONSTRAINT UQ_Units_Name UNIQUE (Name),
	CONSTRAINT FK_Units_UserCreatorId FOREIGN KEY(UserCreatorId) REFERENCES [Inventory].Users(Id)
)
END	
GO
/************************* کالا **************************/
if not Exists(select * from sys.tables where UPPER(NAME) = UPPER(N'[Inventory].Goods'))
BEGIN
Create table [Inventory].Goods
(
	Id BIGINT  Not Null,--Identity(1,1)
	Code nvarchar(100) NOT NULL,
	Name  nvarchar(200) NOT NULL,
	IsActive bit default 1 NOT NULL,
	MainUnitId BIGINT NOT NULL,
	UserCreatorId int,
	CreateDate DATETIME DEFAULT getdate(),
	CONSTRAINT PK_Goods_Id PRIMARY KEY(Id),
	CONSTRAINT UQ_Goods_Code UNIQUE (Code),
	CONSTRAINT UQ_Goods_Name UNIQUE (Name),
	CONSTRAINT FK_Goods_MainUnitId FOREIGN KEY(MainUnitId) REFERENCES [Inventory].Units(Id),
	CONSTRAINT FK_Goods_UserCreatorId FOREIGN KEY(UserCreatorId) REFERENCES [Inventory].Users(Id)
)
END
GO
/************************* کالا **************************/
if not Exists(SELECT * FROM sys.objects WHERE object_Id = OBJECT_Id(N'[Inventory].[UnitConverts]') AND type in (N'U'))
BEGIN
Create table [Inventory].UnitConverts
(
	Id INT IdENTITY(1,1) NOT NULL,
	UnitId BIGINT NOT NULL,
	SubUnitId BIGINT NOT NULL,
	Coefficient DECIMAL(18,3) NOT NULL,
	--FinancialYearId INT,
	EffectiveDateStart DATETIME  DEFAULT getdate(),
	EffectiveDateEnd DATETIME  NULL,
	[RowVersion] smallint not null,
	UserCreatorId INT NOT NULL,
	CreateDate DATETIME DEFAULT getdate(),
	CONSTRAINT PK_UnitConverts_Id PRIMARY KEY(Id),
	CONSTRAINT UQ_UnitConverts_RowVersion UNIQUE (RowVersion),
    CONSTRAINT UQ_UnitConverts_PrimarySecondary_FinancialYear UNIQUE (UnitId,SubUnitId,EffectiveDateStart,Coefficient),
    CONSTRAINT FK_UnitConverts_UnitId FOREIGN KEY(UnitId) REFERENCES [Inventory].Units(Id),
    CONSTRAINT FK_UnitConverts_SubUnitId FOREIGN KEY(SubUnitId) REFERENCES [Inventory].Units(Id),
	CONSTRAINT FK_UnitConverts_UserCreatorId FOREIGN KEY(UserCreatorId) REFERENCES [Inventory].Users(Id),
	--CONSTRAINT FK_UnitConverts_FinancialYearId FOREIGN KEY(FinancialYearId) REFERENCES [Inventory].FinancialYear(Id),
	CONSTRAINT chk_NoMatch2UnitInUnitConverts CHECK ([Inventory].NoMatch2UnitInUnitConverts(UnitId,SubUnitId)<>0)
) 
END
/************************* انواع رسید و حواله **************************/
if not Exists(select * from sys.tables where UPPER(NAME) = UPPER(N'[Inventory].StoreTypes'))
BEGIN
Create table [Inventory].StoreTypes
(
	Id INT Identity(1,1) Not Null,
	Code smallint NOT NULL,
	Type TINYINT NOT NULL DEFAULT 0,--      1-Person   2-Warehouse     3-CostCenter
	InputName nvarchar(100) ,
	OutputName nvarchar(100) ,
	UserCreatorId int,
	CreateDate DATETIME DEFAULT getdate(),
	constraint PK_StoreTypes_Id PRIMARY KEY(Id),
	CONSTRAINT UQ_StoreTypes_Code UNIQUE (Code),
	--CONSTRAINT UQ_StoreTypes_InputName UNIQUE (InputName),
	--CONSTRAINT UQ_StoreTypes_OutputName UNIQUE (OutputName),
	CONSTRAINT FK_StoreTypes_UserCreatorId FOREIGN KEY(UserCreatorId) REFERENCES [Inventory].Users(Id)
)
END
GO
/************************* رسید,حواله,درخواست خرید,درخواست کالا  **************************/
if not Exists(select * from sys.tables where UPPER(NAME) = UPPER(N'[Inventory].Transactions'))
BEGIN
Create table [Inventory].Transactions
(
	Id int Identity(1,1) Not Null,
	[Action] tinyint Not Null,--رسيد -حواله - درخواست خريد - درخواست کالا
	Code decimal(20,2) NULL,--شماره رسید یا حواله یا درخواست خرید,درخواست کالا 
	[Description] nvarchar(max),
	--CrossId int ,--شماره رسيد يا حواله مقابل که خودکار ثبت شده
	PricingReferenceId INT NULL,--شماره مرجع براي قيمتگذاري - رسيد يا حواله هاي برگشتي
	WarehouseId BIGINT Not Null,-- شماره انبار
	StoreTypesId INT Not Null,--نوع رسيد و حواله(خريد؛برگشت از فروش؛ انتقال و ... )
	TimeBucketId INT not null,
	Status tinyint DEFAULT 1 ,--  1-Normal   2-Partial Priced  3-Full Priced  4-Vouchered
	RegistrationDate DATETIME DEFAULT getdate(),--تاريخ ثبت
	SenderReciver int null,--شماره طرف حساب
	HardCopyNo NVARCHAR(10),--شماره فاکتور خريد کاغذي
	ReferenceType NVARCHAR(100),--نوع مرجع
	ReferenceNo NVARCHAR(100),--شماره مرجع
	ReferenceDate DATETIME DEFAULT getdate(),--تاريخ استفاده از مرجع
	UserCreatorId int,
	CreateDate DATETIME DEFAULT getdate(),
	CONSTRAINT PK_Transaction_Id PRIMARY KEY(Id),
	CONSTRAINT UQ_Transaction_Warehouse_Code UNIQUE (WarehouseId,Code),
	--CONSTRAINT UQ_Transaction_ReferenceNo UNIQUE (ReferenceType,ReferenceNo,[Action]),
	--ONSTRAINT FK_Transaction_CrossId FOREIGN KEY(CrossId) REFERENCES Transactions(Id),
	CONSTRAINT FK_Transaction_PricingReferenceId FOREIGN KEY(PricingReferenceId) REFERENCES [Inventory].Transactions(Id),
	CONSTRAINT FK_Transaction_WarehouseId FOREIGN KEY(WarehouseId) REFERENCES [Inventory].Warehouse(Id),
	CONSTRAINT FK_Transaction_StoreTypesId FOREIGN KEY(StoreTypesId) REFERENCES [Inventory].StoreTypes(Id),
	CONSTRAINT FK_Transaction_UserCreatorId FOREIGN KEY(UserCreatorId) REFERENCES [Inventory].Users(Id),
	CONSTRAINT chk_IsValidAction check(Action>=1 and Action<=3)
)
END
GO
GRANT ALTER ON [Inventory].Transactions TO [public] AS [dbo]
GO
/************************* بدنه رسید,حواله,درخواست خرید,درخواست کالا   **************************/
if not Exists(select * from sys.tables where UPPER(NAME) = UPPER(N'[Inventory].TransactionItems'))
BEGIN
CREATE TABLE [Inventory].TransactionItems
(
	Id int Identity(1,1) Not Null,
	[RowVersion] smallint not null,
	--Action tinyint Not Null,
	TransactionId int not null,--شماره رسید یا حواله یا درخواست خرید,درخواست کالا 
	--RegistrationDate DATETIME DEFAULT getdate(),--تاريخ ثبت
	GoodId BIGINT not null,
    QuantityUnitId BIGINT NOT NULL,
    QuantityAmount DECIMAL(20,3) DEFAULT 0,
    Description nvarchar(max),
    UserCreatorId int,
	CreateDate DATETIME DEFAULT getdate(),
    CONSTRAINT PK_TransactionItems_Id PRIMARY KEY(Id),
    CONSTRAINT UQ_TransactionItems_RowVersion UNIQUE (RowVersion,TransactionId),
	CONSTRAINT FK_TransactionItems_TransactionId FOREIGN KEY(TransactionId) REFERENCES [Inventory].Transactions(Id),
	CONSTRAINT FK_TransactionItems_GoodId FOREIGN KEY(GoodId) REFERENCES [Inventory].Goods(Id),
	CONSTRAINT FK_TransactionItems_QuantityUnitId FOREIGN KEY(QuantityUnitId) REFERENCES [Inventory].Units(Id),
	CONSTRAINT FK_TransactionItems_UserCreatorId FOREIGN KEY(UserCreatorId) REFERENCES [Inventory].Users(Id)
)
END
GO
/************************* قيمت گذاري بدنه رسید,حواله,درخواست خرید,درخواست کالا   **************************/
if not Exists(select * from sys.tables where UPPER(NAME) = UPPER(N'[Inventory].TransactionItemPrices'))
BEGIN
CREATE TABLE [Inventory].TransactionItemPrices
(
	Id int Identity(1,1) Not Null,
	[RowVersion] smallint not null,
	--Action tinyint Not Null,
	TransactionId int not null,--شماره رسید یا حواله یا درخواست خرید,درخواست کالا 
	TransactionItemId int not null,--شماره آيتم رسید یا حواله یا درخواست خرید,درخواست کالا 
	Description nvarchar(max),
	QuantityUnitId BIGINT NOT NULL,
    QuantityAmount DECIMAL(20,3) DEFAULT 0,
    PriceUnitId BIGINT NOT NULL,
    Fee DECIMAL(20,3) DEFAULT 0,
    MainCurrencyUnitId BIGINT NOT NULL,
    FeeInMainCurrency DECIMAL(20,3) DEFAULT 0,
    --Coefficient DECIMAL(18,3) NOT NULL,
    RegistrationDate DATETIME DEFAULT getdate(),--تاريخ قيمت گذاري
    QuantityAmountUseFIFO DECIMAL(20,3) DEFAULT 0,
    TransactionReferenceId int NULL,
    IssueReferenceIds NVARCHAR(MAX) DEFAULT N'',
    UserCreatorId int,
	CreateDate DATETIME DEFAULT getdate(),
    CONSTRAINT PK_TransactionItemPrices_Id PRIMARY KEY(Id),
    CONSTRAINT UQ_TransactionItemPrices_RowVersion UNIQUE (RowVersion,TransactionItemId),
	--CONSTRAINT FK_TransactionItemPrices_TransactionId FOREIGN KEY(TransactionId) REFERENCES [Inventory].Transaction(Id),
	CONSTRAINT FK_TransactionItemPrices_TransactionItemsId FOREIGN KEY(TransactionItemId) REFERENCES [Inventory].TransactionItems(Id),
	CONSTRAINT FK_TransactionItemPrices_TransactionReferenceId FOREIGN KEY(TransactionReferenceId) REFERENCES [Inventory].TransactionItemPrices(Id),
	CONSTRAINT FK_TransactionItemPrices_QuantityUnitId FOREIGN KEY(QuantityUnitId) REFERENCES [Inventory].Units(Id),
	CONSTRAINT FK_TransactionItemPrices_PriceUnitId FOREIGN KEY(PriceUnitId) REFERENCES [Inventory].Units(Id),
	CONSTRAINT FK_TransactionItemPrices_MainCurrencyUnitId FOREIGN KEY(MainCurrencyUnitId) REFERENCES [Inventory].Units(Id),
	CONSTRAINT FK_TransactionItemPrices_UserCreatorId FOREIGN KEY(UserCreatorId) REFERENCES [Inventory].Users(Id),
	Constraint chk_IsValidQuantityAmountUseFIFO check(QuantityAmountUseFIFO<=QuantityAmount)
)
END
GO
/**************************************************/
if not Exists(select * from sys.tables where UPPER(NAME) = UPPER(N'[Inventory].OperationReference'))
BEGIN
CREATE TABLE [Inventory].OperationReference
(
	[Id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [OperationId]   BIGINT         NOT NULL,
    [OperationType] INT            NOT NULL,
    [ReferenceType]   VARCHAR (256) NOT NULL,
    [ReferenceNumber] VARCHAR (256)  NOT NULL,
	[RegistrationDate] DATETIME NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [PK_TransactionReference] PRIMARY KEY CLUSTERED ([Id] ASC)
)
END
GO
GO
-------------------------------------
raiserror(N'جدول های دیتابیس با موفقیت ایجاد شدند.',0,1) with nowait
GO