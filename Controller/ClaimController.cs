using ClaimManagement.Data.Entities;
using ClaimManagement.Data.Services;
using ClaimManagement.Model;
using Microsoft.AspNetCore.Mvc;


namespace ClaimManagement.Controller
{

    [Route("[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimService _claimService;
        public ClaimController(IClaimService claimService)
        {
                _claimService = claimService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _claimService.GetClaimByIdAsync(id));
        }

        [HttpPut("ExcelSheetMapper")]
        public async Task<IActionResult> ExcelSheetMapper(ClaimEcxalSheetMappingModel Model)
        {
            return Ok(_claimService.ExcelSheetMapper(Model));
        }
        [HttpGet("ExcelSheetMapper")]
        public IActionResult GetExcelSheetMapper()
        {
            return Ok(_claimService.GetExcelSheetMapper());
        }
        [HttpPost("ImportFromExcelFile")]
        public async Task<IActionResult> ImportFromExcelFile(IFormFile file)
        {
            return Ok(await _claimService.ImportClaimsFromExcelFile(file));
        }
   
    }
}
