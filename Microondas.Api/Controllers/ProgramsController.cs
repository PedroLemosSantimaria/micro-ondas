using Microsoft.AspNetCore.Mvc;
using Microondas.Core.Dtos.Requests;
using Microondas.Core.Interfaces;

namespace Microondas.Api.Controllers
{
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
            return Ok(programs);
        }

        [HttpPost("custom")]
        public IActionResult CreateCustom([FromBody] CustomProgramRequest request)
        {
            programService.CreateCustomProgram(request);

            return Ok(new
            {
                message = "Programa customizado salvo com sucesso."
            });
        }
    }
}