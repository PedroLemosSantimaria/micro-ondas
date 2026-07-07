using System.Text.Json.Serialization;

namespace Microondas.Core.Entities
{
    public class HeatingProgram
    {
        public string Name { get; set; }
        public string Food { get; set; }
        public int TimeInSeconds { get; set; }
        public int Power { get; set; }
        public string HeatingChar { get; set; }
        public string Instructions { get; set; }
        public bool IsDefault { get; set; }

        public HeatingProgram()
        {
        }

        [JsonConstructor]
        public HeatingProgram(
            string name,
            string food,
            int timeInSeconds,
            int power,
            string heatingChar,
            string instructions,
            bool isDefault)
        {
            Name = name;
            Food = food;
            TimeInSeconds = timeInSeconds;
            Power = power;
            HeatingChar = heatingChar;
            Instructions = instructions;
            IsDefault = isDefault;
        }
    }
}