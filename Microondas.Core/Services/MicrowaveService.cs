using System;
using System.Linq;
using Microondas.Core.Entities;
using Microondas.Core.Exceptions;
using Microondas.Core.Interfaces;

namespace Microondas.Core.Services
{
    public class MicrowaveService : IMicrowaveService
    {
        private readonly IHeatingProgramRepository heatingProgramRepository;
        private MicrowaveSession currentSession;

        public MicrowaveService(IHeatingProgramRepository heatingProgramRepository)
        {
            this.heatingProgramRepository = heatingProgramRepository;
        }

        public MicrowaveSession StartManual(int? timeInSeconds, int? power)
        {
            if (currentSession != null && currentSession.IsRunning)
            {
                if (currentSession.IsPredefinedProgram)
                    throw new BusinessException("Não é permitido adicionar tempo em programa pré-definido.");

                currentSession.AddThirtySeconds();
                return currentSession;
            }

            if (currentSession != null && currentSession.IsPaused)
            {
                currentSession.Resume();
                return currentSession;
            }

            if (!timeInSeconds.HasValue && !power.HasValue)
            {
                return StartQuick();
            }

            var finalPower = power ?? 10;

            ValidateManualHeating(timeInSeconds, finalPower);

            currentSession = new MicrowaveSession(
                timeInSeconds.Value,
                finalPower,
                ".",
                false
            );

            currentSession.Start();
            return currentSession;
        }

        public MicrowaveSession StartQuick()
        {
            if (currentSession != null && currentSession.IsRunning)
            {
                if (currentSession.IsPredefinedProgram)
                    throw new BusinessException("Não é permitido adicionar tempo em programa pré-definido.");

                currentSession.AddThirtySeconds();
                return currentSession;
            }

            if (currentSession != null && currentSession.IsPaused)
            {
                currentSession.Resume();
                return currentSession;
            }

            currentSession = new MicrowaveSession(30, 10, ".", false);
            currentSession.Start();
            return currentSession;
        }

        public MicrowaveSession StartProgram(string programName)
        {
            if (string.IsNullOrWhiteSpace(programName))
                throw new BusinessException("Programa não informado.");

            if (currentSession != null && currentSession.IsPaused)
            {
                currentSession.Resume();
                return currentSession;
            }

            if (currentSession != null && currentSession.IsRunning)
                throw new BusinessException("Já existe um aquecimento em andamento.");

            var program = heatingProgramRepository
                .GetAllPrograms()
                .FirstOrDefault(x => x.Name.Equals(programName, StringComparison.OrdinalIgnoreCase));

            if (program == null)
                throw new BusinessException("Programa não encontrado.");

            currentSession = new MicrowaveSession(
                program.TimeInSeconds,
                program.Power,
                program.HeatingChar,
                true,
                program.Name
            );

            currentSession.Start();
            return currentSession;
        }

        public MicrowaveSession PauseOrCancel()
        {
            if (currentSession == null)
            {
                return CreateEmptySession();
            }

            if (currentSession.IsRunning)
            {
                currentSession.Pause();
                return currentSession;
            }

            if (currentSession.IsPaused)
            {
                currentSession.Cancel();
                currentSession = null;
                return CreateEmptySession();
            }

            currentSession = null;
            return CreateEmptySession();
        }

        public MicrowaveSession Resume()
        {
            if (currentSession == null)
                throw new BusinessException("Nenhum aquecimento para continuar.");

            if (!currentSession.IsPaused)
                throw new BusinessException("O aquecimento não está pausado.");

            currentSession.Resume();
            return currentSession;
        }

        public MicrowaveSession Tick()
        {
            if (currentSession == null)
                throw new BusinessException("Nenhum aquecimento em andamento.");

            currentSession.Tick();
            return currentSession;
        }

        public MicrowaveSession GetCurrentSession()
        {
            if (currentSession == null)
                return CreateEmptySession();

            return currentSession;
        }

        private void ValidateManualHeating(int? timeInSeconds, int power)
        {
            if (!timeInSeconds.HasValue)
                throw new BusinessException("Informe um tempo válido.");

            if (timeInSeconds.Value < 1 || timeInSeconds.Value > 120)
                throw new BusinessException("Informe um tempo válido entre 1 segundo e 2 minutos.");

            if (power < 1 || power > 10)
                throw new BusinessException("A potência deve estar entre 1 e 10.");
        }

        private MicrowaveSession CreateEmptySession()
        {
            return new MicrowaveSession(0, 0, ".", false);
        }
    }
}