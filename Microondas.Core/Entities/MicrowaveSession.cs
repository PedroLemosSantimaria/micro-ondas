using Microondas.Core.Enums;

namespace Microondas.Core.Entities
{
    public class MicrowaveSession
    {
        public int TotalTimeInSeconds { get; private set; }
        public int RemainingTimeInSeconds { get; private set; }
        public int Power { get; private set; }
        public string HeatingChar { get; private set; }
        public string ProcessText { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsFinished { get; private set; }
        public bool IsCancelled { get; private set; }
        public string ProgramName { get; private set; }
        public bool IsPredefinedProgram { get; private set; }
        public bool AllowTimeAddition { get; private set; }
        public MicrowaveStatus Status { get; private set; }

        public MicrowaveSession(
            int totalTimeInSeconds,
            int power,
            string heatingChar,
            bool allowTimeAddition,
            string programName = null,
            bool isPredefinedProgram = false)
        {
            TotalTimeInSeconds = totalTimeInSeconds;
            RemainingTimeInSeconds = totalTimeInSeconds;
            Power = power;
            HeatingChar = heatingChar;
            ProcessText = string.Empty;
            ProgramName = programName;
            IsPredefinedProgram = isPredefinedProgram;
            AllowTimeAddition = allowTimeAddition;

            IsRunning = true;
            IsPaused = false;
            IsFinished = false;
            IsCancelled = false;
            Status = MicrowaveStatus.Running;
        }

        public void AddThirtySeconds()
        {
            if (!AllowTimeAddition)
                return;

            RemainingTimeInSeconds += 30;
            TotalTimeInSeconds += 30;
        }

        public void Pause()
        {
            if (!IsRunning)
                return;

            IsRunning = false;
            IsPaused = true;
            Status = MicrowaveStatus.Paused;
        }

        public void Resume()
        {
            if (!IsPaused)
                return;

            IsRunning = true;
            IsPaused = false;
            Status = MicrowaveStatus.Running;
        }

        public void Cancel()
        {
            IsRunning = false;
            IsPaused = false;
            IsFinished = false;
            IsCancelled = true;
            ProcessText = string.Empty;
            Status = MicrowaveStatus.Cancelled;
        }

        public void MarkAsIdle()
        {
            IsRunning = false;
            IsPaused = false;
            IsFinished = false;
            IsCancelled = false;
            ProcessText = string.Empty;
            Status = MicrowaveStatus.Idle;
        }

        public void Tick()
        {
            if (!IsRunning || RemainingTimeInSeconds <= 0)
                return;

            RemainingTimeInSeconds--;

            for (var i = 0; i < Power; i++)
            {
                ProcessText += HeatingChar;
            }

            ProcessText += " ";

            if (RemainingTimeInSeconds == 0)
            {
                IsRunning = false;
                IsPaused = false;
                IsFinished = true;
                Status = MicrowaveStatus.Finished;
                ProcessText += "Aquecimento concluído";
            }
        }
    }
}