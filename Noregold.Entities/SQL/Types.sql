IF TYPE_ID(N'dbo.UD_ExcelInventory_TABLE') IS NOT NULL
	DROP TYPE dbo.UD_ExcelInventory_TABLE;
GO

BEGIN
	CREATE TYPE dbo.UD_ExcelInventory_TABLE AS TABLE (
		 RFIDNo VARCHAR(255)
		,SupplierName VARCHAR(255)
		,ProductName VARCHAR(255)
		,UnitPrice DECIMAL(12, 5)
		,SellingPrice DECIMAL(12, 5)
		,TotalCapitalPerGram DECIMAL(12, 5)
		,TotalSellingPricePerGram DECIMAL(12, 5)
		,Quantity INT
		,WeightGrams DECIMAL(12, 5)
		,BranchCode VARCHAR(255)
		,GoldClass VARCHAR(255)
		,Karat VARCHAR(255)
		,TrayNumber INT
		);
END;
GO