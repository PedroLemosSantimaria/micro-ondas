using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microondas.Core.Dtos.Requests;
using Microondas.Core.Dtos.Responses;
using Microondas.Core.Entities;
using Microondas.Core.Helpers;
using Microondas.Core.Interfaces;

namespace Microondas.Api.Controllers
{
    [Authorize]
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
            return Ok(ApiResponse<MicrowaveSessionResponse>.Ok(ToResponse(session), "Aquecimento iniciado."));
        }

        [HttpPost("quick-start")]
        public IActionResult QuickStart()
        {
            var session = microwaveService.StartQuick();
            return Ok(ApiResponse<MicrowaveSessionResponse>.Ok(ToResponse(session), "Início rápido acionado."));
        }

        [HttpPost("start-program/{programName}")]
        public IActionResult StartProgram(string programName)
        {
            var session = microwaveService.StartProgram(programName);
            return Ok(ApiResponse<MicrowaveSessionResponse>.Ok(ToResponse(session), "Programa iniciado."));
        }

        [HttpPost("pause-cancel")]
        public IActionResult PauseOrCancel()
        {
            var session = microwaveService.PauseOrCancel();
            return Ok(ApiResponse<MicrowaveSessionResponse>.Ok(ToResponse(session), "Ação de pausa/cancelamento executada."));
        }

        [HttpPost("resume")]
        public IActionResult Resume()
        {
            var session = microwaveService.Resume();
            return Ok(ApiResponse<MicrowaveSessionResponse>.Ok(ToResponse(session), "Aquecimento retomado."));
        }

        [HttpPost("tick")]
        public IActionResult Tick()
        {
            var session = microwaveService.Tick();
            return Ok(ApiResponse<MicrowaveSessionResponse>.Ok(ToResponse(session)));
        }

        [HttpGet("current")]
        public IActionResult Current()
        {
            var session = microwaveService.GetCurrentSession();
            return Ok(ApiResponse<MicrowaveSessionResponse>.Ok(ToResponse(session)));
        }

        [HttpPost("clear")]
        public IActionResult Clear()
        {
            var session = microwaveService.Clear();
            return Ok(ApiResponse<MicrowaveSessionResponse>.Ok(ToResponse(session), "Sessão limpa."));
        }

        private MicrowaveSessionResponse ToResponse(MicrowaveSession session)
        {
            var message = string.Empty;

            if (session.IsFinished)
                message = "Aquecimento concluído.";
            else if (session.IsPaused)
                message = "Aquecimento pausado.";
            else if (session.IsCancelled)
                message = "Aquecimento cancelado.";
            else if (session.IsRunning)
                message = "Aquecimento em andamento.";

            return new MicrowaveSessionResponse
            {
                TotalTimeInSeconds = session.TotalTimeInSeconds,
                RemainingTimeInSeconds = session.RemainingTimeInSeconds,
                Power = session.Power,
                HeatingChar = session.HeatingChar,
                ProcessText = session.ProcessText,
                IsRunning = session.IsRunning,
                IsPaused = session.IsPaused,
                IsFinished = session.IsFinished,
                IsCancelled = session.IsCancelled,
                ProgramName = session.ProgramName,
                IsPredefinedProgram = session.IsPredefinedProgram,
                CanAddTime = session.AllowTimeAddition,
                FormattedTime = TimeFormatter.Format(session.RemainingTimeInSeconds),
                Status = session.Status.ToString(),
                Message = message
            };
        }
    }
}