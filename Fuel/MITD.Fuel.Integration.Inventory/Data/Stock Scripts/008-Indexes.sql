--USE MiniStock
--GO 
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Inventory].[ErrorMessages]') 
					AND name = N'idxErrorMessage')
		CREATE INDEX idxErrorMessage ON [Inventory].ErrorMessages(ErrorMessage)
GO 

IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Inventory].[Users]') 
					AND name = N'idxUsers')					
		CREATE INDEX idxUsers ON [Inventory].Users(Code,[UserName])
GO 
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Inventory].[OperationReference]') 
					AND name = N'idxOperationReference')					
		CREATE INDEX idxOperationReference ON [Inventory].OperationReference(ReferenceType,ReferenceNumber)
GO 
raiserror(N'پایان ایجاد ایندکس ها.',0,1) with nowait
GO