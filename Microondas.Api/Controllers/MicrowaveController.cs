using Microsoft.AspNetCore.Mvc;
using Microondas.Core.Dtos.Requests;
using Microondas.Core.Dtos.Responses;
using Microondas.Core.Helpers;
using Microondas.Core.Interfaces;

namespace Microondas.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MicrowaveController : ControllerBase
    {
        private readonly IMicrowaveService microwaveService;

        public MicrowaveController(IMicrowaveService microwaveService)
        {
            this.microwaveService = microwaveService;
        }

        [HttpPost("start")]
        public IActionResult Start([FromBody] StartHeatingRequest request)
        {
            var session = microwaveService.StartManual(request?.TimeInSeconds, request?.Power);
            return Ok(ToResponse(session));
        }

        [HttpPost("quick-start")]
        public IActionResult QuickStart()
        {
            var session = microwaveService.StartQuick();
            return Ok(ToResponse(session));
        }

        [HttpPost("start-program/{programName}")]
        public IActionResult StartProgram(string programName)
        {
            var session = microwaveService.StartProgram(programName);
            return Ok(ToResponse(session));
        }

        [HttpPost("pause-cancel")]
        public IActionResult PauseOrCancel()
        {
            var session = microwaveService.PauseOrCancel();
            return Ok(ToResponse(session));
        }

        [HttpPost("resume")]
        public IActionResult Resume()
        {
            var session = microwaveService.Resume();
            return Ok(ToResponse(session));
        }

        [HttpPost("tick")]
        public IActionResult Tick()
        {
            var session = microwaveService.Tick();
            return Ok(ToResponse(session));
        }

        [HttpGet("current")]
        public IActionResult Current()
        {
            var session = microwaveService.GetCurrentSession();
            return Ok(ToResponse(session));
        }

        private MicrowaveSessionResponse ToResponse(Core.Entities.MicrowaveSession session)
        {
            return new MicrowaveSessionResponse
            {
                TotalTimeInSeconds = session.TotalTimeInSeconds,
                RemainingTimeInSeconds = session.RemainingTimeInSeconds,
                Power = session.Power,
                ProcessText = session.ProcessText,
                IsRunning = session.IsRunning,
                IsPaused = session.IsPaused,
                IsFinished = session.IsFinished,
                ProgramName = session.ProgramName,
                FormattedTime = TimeFormatter.Format(session.RemainingTimeInSeconds)
            };
        }
    }
}