namespace Microondas.Core.Dtos.Requests
{
    public class CustomProgramRequest
    {
        public string Name { get; set; }
        public string Food { get; set; }
        public int TimeInSeconds { get; set; }
        public int Power { get; set; }
        public string HeatingChar { get; set; }
        public string Instructions { get; set; }
    }
}