namespace Microondas.Core.Dtos.Requests
{
    public class StartHeatingRequest
    {
        public int? TimeInSeconds { get; set; }
        public int? Power { get; set; }
    }
}