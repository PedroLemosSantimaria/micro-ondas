using System.Collections.Generic;
using Microondas.Core.Dtos.Requests;
using Microondas.Core.Entities;

namespace Microondas.Core.Interfaces
{
    public interface IProgramService
    {
        List<HeatingProgram> GetAllPrograms();
        void CreateCustomProgram(CustomProgramRequest request);
    }
}