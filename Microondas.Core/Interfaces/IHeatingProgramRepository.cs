using System.Collections.Generic;
using Microondas.Core.Entities;

namespace Microondas.Core.Interfaces
{
    public interface IHeatingProgramRepository
    {
        List<HeatingProgram> GetAllPrograms();
        HeatingProgram GetProgramByName(string name);
        void SaveCustomProgram(HeatingProgram program);
    }
}