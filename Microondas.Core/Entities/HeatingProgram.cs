namespace Microondas.Core.Entities
{
    public class HeatingProgram
    {
        public string Name { get; private set; }
        public string Food { get; private set; }
        public int TimeInSeconds { get; private set; }
        public int Power { get; private set; }
        public string HeatingChar { get; private set; }
        public string Instructions { get; private set; }
        public bool IsDefault { get; private set; }

        public HeatingProgram()
        {
        }

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