namespace Microondas.Core.Dtos.Responses
{
    public class MicrowaveSessionResponse
    {
        public int TotalTimeInSeconds { get; set; }
        public int RemainingTimeInSeconds { get; set; }
        public int Power { get; set; }
        public string HeatingChar { get; set; }
        public string ProcessText { get; set; }
        public bool IsRunning { get; set; }
        public bool IsPaused { get; set; }
        public bool IsFinished { get; set; }
        public bool IsCancelled { get; set; }
        public string ProgramName { get; set; }
        public bool IsPredefinedProgram { get; set; }
        public bool CanAddTime { get; set; }
        public string FormattedTime { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}