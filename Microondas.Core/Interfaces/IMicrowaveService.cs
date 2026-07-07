using Microondas.Core.Entities;

namespace Microondas.Core.Interfaces
{
    public interface IMicrowaveService
    {
        MicrowaveSession StartManual(int? timeInSeconds, int? power);
        MicrowaveSession StartQuick();
        MicrowaveSession StartProgram(string programName);
        MicrowaveSession PauseOrCancel();
        MicrowaveSession Resume();
        MicrowaveSession Tick();
        MicrowaveSession GetCurrentSession();
        MicrowaveSession Clear();
    }
}