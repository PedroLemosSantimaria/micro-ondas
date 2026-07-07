using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microondas.Core.Dtos.Requests;
using Microondas.Core.Dtos.Responses;
using Microondas.Core.Entities;
using Microondas.Core.Interfaces;
using System.Collections.Generic;

namespace Microondas.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramsController : ControllerBase
    {
        private readonly IProgramService programService;

        public ProgramsController(IProgramService programService)
        {
            this.programService = programService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var programs = programService.GetAllPrograms();
            return Ok(ApiResponse<List<HeatingProgram>>.Ok(programs));
        }

        [HttpPost("custom")]
        public IActionResult CreateCustom([FromBody] CustomProgramRequest request)
        {
            programService.CreateCustomProgram(request);

            return Ok(ApiResponse<object>.Ok(null, "Programa customizado salvo com sucesso."));
        }
    }
}