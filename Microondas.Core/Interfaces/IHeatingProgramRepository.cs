using System.Collections.Generic;
using Microondas.Core.Entities;

namespace Microondas.Core.Interfaces
{
    public interface IHeatingProgramRepository
    {
        List<HeatingProgram> GetDefaultPrograms();
        List<HeatingProgram> GetCustomPrograms();
        List<HeatingProgram> GetAllPrograms();
        void SaveCustomProgram(HeatingProgram program);
    }
}