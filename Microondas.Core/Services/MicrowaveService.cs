using System;
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
            currentSession = CreateIdleSession();
        }

        public MicrowaveSession StartManual(int? timeInSeconds, int? power)
        {
            if (!timeInSeconds.HasValue && !power.HasValue)
            {
                return StartQuick();
            }

            if (currentSession != null && currentSession.IsPaused)
            {
                currentSession.Resume();
                return currentSession;
            }

            if (currentSession != null && currentSession.IsRunning)
            {
                if (!currentSession.AllowTimeAddition)
                    throw new BusinessException("Não é permitido adicionar tempo em programas pré-definidos.");

                currentSession.AddThirtySeconds();
                return currentSession;
            }

            var finalTime = timeInSeconds ?? 30;
            var finalPower = power ?? 10;

            ValidateManualHeating(finalTime, finalPower);

            currentSession = new MicrowaveSession(
                finalTime,
                finalPower,
                ".",
                allowTimeAddition: true);

            return currentSession;
        }

        public MicrowaveSession StartQuick()
        {
            if (currentSession != null && currentSession.IsPaused)
            {
                currentSession.Resume();
                return currentSession;
            }

            if (currentSession != null && currentSession.IsRunning)
            {
                if (!currentSession.AllowTimeAddition)
                    throw new BusinessException("Não é permitido adicionar tempo em programas pré-definidos.");

                currentSession.AddThirtySeconds();
                return currentSession;
            }

            currentSession = new MicrowaveSession(
                30,
                10,
                ".",
                allowTimeAddition: true);

            return currentSession;
        }

        public MicrowaveSession StartProgram(string programName)
        {
            if (string.IsNullOrWhiteSpace(programName))
                throw new BusinessException("Informe o nome do programa.");

            var program = heatingProgramRepository.GetProgramByName(programName);

            if (program == null)
                throw new BusinessException("Programa não encontrado.");

            currentSession = new MicrowaveSession(
                program.TimeInSeconds,
                program.Power,
                program.HeatingChar,
                allowTimeAddition: false,
                programName: program.Name,
                isPredefinedProgram: program.IsDefault);

            return currentSession;
        }

        public MicrowaveSession PauseOrCancel()
        {
            if (currentSession == null)
            {
                currentSession = CreateIdleSession();
                return currentSession;
            }

            if (currentSession.IsRunning)
            {
                currentSession.Pause();
                return currentSession;
            }

            if (currentSession.IsPaused)
            {
                currentSession = CreateCancelledSession();
                return currentSession;
            }

            currentSession = CreateIdleSession();
            return currentSession;
        }

        public MicrowaveSession Resume()
        {
            if (currentSession == null || !currentSession.IsPaused)
                throw new BusinessException("Não existe aquecimento pausado para retomar.");

            currentSession.Resume();
            return currentSession;
        }

        public MicrowaveSession Tick()
        {
            if (currentSession == null)
                throw new BusinessException("Não existe aquecimento em andamento.");

            if (!currentSession.IsRunning)
                return currentSession;

            currentSession.Tick();
            return currentSession;
        }

        public MicrowaveSession GetCurrentSession()
        {
            if (currentSession == null)
            {
                currentSession = CreateIdleSession();
            }

            return currentSession;
        }

        public MicrowaveSession Clear()
        {
            currentSession = CreateIdleSession();
            return currentSession;
        }

        private void ValidateManualHeating(int timeInSeconds, int power)
        {
            if (timeInSeconds < 1 || timeInSeconds > 120)
                throw new BusinessException("Informe um tempo válido entre 1 segundo e 2 minutos.");

            if (power < 1 || power > 10)
                throw new BusinessException("A potência deve estar entre 1 e 10.");
        }

        private MicrowaveSession CreateIdleSession()
        {
            var session = new MicrowaveSession(1, 10, ".", true);
            session.MarkAsIdle();
            return session;
        }

        private MicrowaveSession CreateCancelledSession()
        {
            var session = new MicrowaveSession(1, 10, ".", true);
            session.Cancel();
            return session;
        }
    }
}