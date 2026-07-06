using System.Text;

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
        public bool IsPredefinedProgram { get; private set; }
        public string ProgramName { get; private set; }

        public MicrowaveSession(
            int totalTimeInSeconds,
            int power,
            string heatingChar,
            bool isPredefinedProgram,
            string programName = "")
        {
            TotalTimeInSeconds = totalTimeInSeconds;
            RemainingTimeInSeconds = totalTimeInSeconds;
            Power = power;
            HeatingChar = heatingChar;
            IsPredefinedProgram = isPredefinedProgram;
            ProgramName = programName ?? "";
            ProcessText = "";
            IsRunning = false;
            IsPaused = false;
            IsFinished = false;
        }

        public void Start()
        {
            IsRunning = true;
            IsPaused = false;
            IsFinished = false;
        }

        public void Pause()
        {
            if (!IsRunning)
                return;

            IsRunning = false;
            IsPaused = true;
        }

        public void Resume()
        {
            if (!IsPaused)
                return;

            IsRunning = true;
            IsPaused = false;
        }

        public void Cancel()
        {
            IsRunning = false;
            IsPaused = false;
            IsFinished = false;
            RemainingTimeInSeconds = 0;
            ProcessText = "";
        }

        public void AddThirtySeconds()
        {
            RemainingTimeInSeconds += 30;
            TotalTimeInSeconds += 30;
        }

        public void Tick()
        {
            if (!IsRunning || IsPaused || IsFinished)
                return;

            if (RemainingTimeInSeconds <= 0)
                return;

            RemainingTimeInSeconds--;

            ProcessText += BuildHeatingStep();

            if (RemainingTimeInSeconds == 0)
            {
                IsRunning = false;
                IsFinished = true;
                ProcessText += " Aquecimento concluído";
            }
        }

        private string BuildHeatingStep()
        {
            var text = new StringBuilder();

            for (var i = 0; i < Power; i++)
            {
                text.Append(HeatingChar);
            }

            text.Append(" ");
            return text.ToString();
        }
    }
}