using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noregold.Service.Interrface;
using Noregold.Web.DTO;
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
            dt.Columns.Add("BranchName", typeof(string));
            dt.Columns.Add("GoldClass", typeof(string));
            dt.Columns.Add("TrayNumber", typeof(int));


            using var stream = excelFile.File.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1);

            var rows = worksheet?.RangeUsed()?.RowsUsed();

            if(rows != null)
            {
                foreach (var row in rows.Skip(1))
                {
                    var rfidNo = row.Cell(1).GetValue<string>();
                    var supplierName = row.Cell(2).GetValue<string>();
                    var productName = row.Cell(3).GetValue<string>();

                    var unitPrice = ExtractDecimal(row.Cell(5).GetValue<string>());

                    var sellingPrice = ExtractDecimal(row.Cell(6).GetValue<string>());
                    var totalCapitalPerGram = ExtractDecimal(row.Cell(7).GetValue<string>());
                    var totalSellingPrice = ExtractDecimal(row.Cell(8).GetValue<string>());

                    var Quantity = row.Cell(9).GetValue<int>();

                    var weightGrams = ExtractDecimal(row.Cell(10).GetValue<string>());
                    var branchCode = row.Cell(13).GetValue<string>();

                    var branchName = row.Cell(14).GetValue<string>();
                    var goldClass = row.Cell(15).GetValue<string>();
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
                    dataRow["BranchName"] = branchName;
                    dataRow["GoldClass"] = goldClass;
                    dataRow["TrayNumber"] = trayNumber;

                    dt.Rows.Add(dataRow);
                }
            }
                


            return Ok();
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

        private static decimal ExtractDecimal(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return 0m;

            // Regex pattern: 
            // [^-0-9.] means "anything that is NOT a negative sign, a digit, or a dot"
            string cleanValue = Regex.Replace(input, @"[^-0-9.]", "");

            // Use InvariantCulture to ensure the dot is always treated as a decimal separator
            if (decimal.TryParse(cleanValue, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }

            return 0m;
        }

    }
}
