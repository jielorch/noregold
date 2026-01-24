IF OBJECT_ID(N'dbo.InsertInventoryData', N'P') IS NOT NULL
	DROP PROCEDURE dbo.InsertInventoryData
GO
	CREATE PROCEDURE dbo.InsertInventoryData 
		@InventoryTable	dbo.UD_ExcelInventory_TABLE READONLY,
		@Result					BIT OUTPUT
	AS
		BEGIN
			SET NOCOUNT ON;
			
			BEGIN TRY
				
				INSERT INTO dbo.ran_inventory (RFIDNo
												,SupplierCode
												,ProductCategoryID
												,UnitPrice
												,SellingPrice
												,Quantity
												,TrayNumber
												,WeightGrams
												,Branch
												,GoldClass
												,Carat
												,IsAvailable
												,TotalCapitalPerGram
												,TotalSellingPricePerGram)
				  SELECT a.RFIDNo
						,b.SupplierCode
						,c.ID
						,a.UnitPrice
						,a.SellingPrice
						,a.Quantity
						,a.TrayNumber
						,a.WeightGrams
						,a.BranchCode
						,a.GoldClass
						,a.Karat
						,1
						,a.TotalCapitalPerGram
						,a.TotalSellingPricePerGram
				  FROM @InventoryTable a
				  INNER JOIN [dbo].[ran_suppliers] b ON a.SupplierName = b.SupplierName
				  INNER JOIN [dbo].[ran_product_category] c ON a.ProductName = c.[Name]
				  WHERE NOT EXISTS(
					SELECT 1
					FROM dbo.ran_inventory d
					WHERE d.RFIDNo = a.RFIDNo
				  );

				  IF @@ROWCOUNT > 0
						BEGIN 
							SET @Result = 1; -- INSERT OR UPDATE SUCCESSFUL
						END;


			END TRY

			BEGIN CATCH 
				DECLARE @ErrorMessage NVARCHAR(4000);
				SET @ErrorMessage = ERROR_MESSAGE();
				THROW @ErrorMessage,
						'16',
						1;
				SET @Result = 0;
			END CATCH
		END
GO
