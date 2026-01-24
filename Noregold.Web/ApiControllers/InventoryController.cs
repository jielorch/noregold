using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Noregold.Service.Interrface;
using Noregold.Web.DTO;
using Noregold.Web.Helper;
using Noregold.Web.Models;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Noregold.Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController(IInventoryService inventoryService) : ControllerBase
    {
        private readonly IInventoryService _inventoryService = inventoryService;

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var sql = "select * from [dbo].[ran_inventory]";
            var result = await _inventoryService.GetInventoryDetailsAsync<InventoryModel>(sql, null);
            return Ok(result);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] ExcelFile excelFile)
        {
            try
            {
                if (!IsValidExcelFile(excelFile.File)) return BadRequest("No file uploaded");

                DataTable dt = new();

                dt.Columns.Add("RFIDNo", typeof(string));
                dt.Columns.Add("SupplierName", typeof(string));
                dt.Columns.Add("ProductName", typeof(string));

                dt.Columns.Add("UnitPrice", typeof(decimal));
                dt.Columns.Add("SellingPrice", typeof(decimal));
                dt.Columns.Add("TotalCapitalPerGram", typeof(decimal));
                dt.Columns.Add("TotalSellingPrice", typeof(decimal));

                dt.Columns.Add("Quantity", typeof(int));

                dt.Columns.Add("WeightGrams", typeof(decimal));

                dt.Columns.Add("BranchCode", typeof(string)); 
                dt.Columns.Add("GoldClass", typeof(string));
                dt.Columns.Add("Karat", typeof(int));
                dt.Columns.Add("TrayNumber", typeof(int));


                using var stream = excelFile.File.OpenReadStream();
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);

                var rows = worksheet?.RangeUsed()?.RowsUsed();

                if (rows != null)
                {
                    foreach (var row in rows.Skip(1))
                    {
                        var rfidNo = row.Cell(1).GetValue<string>();
                        var supplierName = row.Cell(2).GetValue<string>();
                        var productName = row.Cell(3).GetValue<string>();

                        var unitPrice = ExtractNumber<decimal>(row.Cell(5).GetValue<string>());

                        var sellingPrice = ExtractNumber<decimal>(row.Cell(6).GetValue<string>());
                        var totalCapitalPerGram = ExtractNumber<decimal>(row.Cell(7).GetValue<string>());
                        var totalSellingPrice = ExtractNumber<decimal>(row.Cell(8).GetValue<string>());

                        var Quantity = row.Cell(9).GetValue<int>();

                        var weightGrams = ExtractNumber<decimal>(row.Cell(10).GetValue<string>());
                        var branchCode = row.Cell(13).GetValue<string>();
                         
                        var goldClass = row.Cell(14).GetValue<string>();
                        var karat = ExtractNumber<int>(row.Cell(15).GetValue<string>());
                        var trayNumber = row.Cell(16).GetValue<int>();

                        var dataRow = dt.NewRow();

                        dataRow["RFIDNo"] = rfidNo;
                        dataRow["SupplierName"] = supplierName;
                        dataRow["ProductName"] = productName;
                        dataRow["UnitPrice"] = unitPrice;
                        dataRow["SellingPrice"] = sellingPrice;
                        dataRow["TotalCapitalPerGram"] = totalCapitalPerGram;
                        dataRow["TotalSellingPrice"] = totalSellingPrice;
                        dataRow["Quantity"] = Quantity;
                        dataRow["WeightGrams"] = weightGrams;
                        dataRow["BranchCode"] = branchCode; 
                        dataRow["GoldClass"] = goldClass;
                        dataRow["Karat"] = karat;
                        dataRow["TrayNumber"] = trayNumber;

                        dt.Rows.Add(dataRow);
                    }
                }

                var parameters = new DynamicParameters()
                                               .AddTableValuedParameter("InventoryTable", dt, "dbo.UD_ExcelInventory_TABLE")
                                               .AddOutputParameter("Result", DbType.Boolean);

                var isSuccessful = await ExecuteBulkUpload(parameters, "dbo.InsertInventoryData");

                return Ok(isSuccessful);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Please try again later...");
            }
        }

        public virtual async Task<bool> ExecuteBulkUpload(DynamicParameters parameters, string storedProcedure)
        {
            await _inventoryService.BulkUploadAsync(storedProcedure, parameters);
            return parameters.Get<bool>("Result");
        }

        private static bool IsValidExcelFile(IFormFile file)
        {
            // 1. Basic checks
            if (file == null || file.Length == 0) return false;

            // 2. Extension check (.xlsx and .xlsm only for ClosedXML)
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".xlsx" && extension != ".xlsm") return false;

            // 3. MIME type check
            var mimeType = file.ContentType;
            if (mimeType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" &&
                mimeType != "application/vnd.ms-excel.sheet.macroEnabled.12") return false;

            // 4. Magic Byte Validation (The secure part)
            // Read the first 4 bytes to ensure it's a valid ZIP/OpenXML structure
            using var stream = file.OpenReadStream();
            byte[] buffer = new byte[4];
            stream.ReadExactly(buffer, 0, 4);

            // ZIP magic bytes: 50 4B 03 04
            byte[] zipHeader = new byte[] { 0x50, 0x4B, 0x03, 0x04 };

            return buffer.SequenceEqual(zipHeader);
        }

        private static T ExtractNumber<T>(string input) where T : struct, IConvertible
        {
            if(string.IsNullOrWhiteSpace(input)) return default;

            string cleanValue = Regex.Replace(input, @"[^-0-9.]", "");

            try
            {
                return (T)Convert.ChangeType(cleanValue, typeof(T), CultureInfo.InvariantCulture);
            }
            catch
            {
                return default;
            }
        }

    }
}
