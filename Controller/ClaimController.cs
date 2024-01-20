﻿using ClaimManagement.Data.Entities;
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
        [HttpGet]
        public IActionResult Get()
        {
            var test = new Claim();
            
            return Ok(test
            );
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
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
