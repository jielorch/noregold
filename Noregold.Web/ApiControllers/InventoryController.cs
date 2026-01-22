using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noregold.Service.Interrface;
using Noregold.Web.DTO;
using Noregold.Web.Models;

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
            return Ok();
        }

    }
}
