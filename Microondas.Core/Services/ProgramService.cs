using System;
using System.Collections.Generic;
using System.Linq;
using Microondas.Core.Dtos.Requests;
using Microondas.Core.Entities;
using Microondas.Core.Exceptions;
using Microondas.Core.Interfaces;

namespace Microondas.Core.Services
{
    public class ProgramService : IProgramService
    {
        private readonly IHeatingProgramRepository heatingProgramRepository;

        public ProgramService(IHeatingProgramRepository heatingProgramRepository)
        {
            this.heatingProgramRepository = heatingProgramRepository;
        }

        public List<HeatingProgram> GetAllPrograms()
        {
            return heatingProgramRepository.GetAllPrograms();
        }

        public void CreateCustomProgram(CustomProgramRequest request)
        {
            ValidateCustomProgram(request);

            var program = new HeatingProgram(
                request.Name.Trim(),
                request.Food.Trim(),
                request.TimeInSeconds,
                request.Power,
                request.HeatingChar.Trim(),
                request.Instructions ?? "",
                false
            );

            heatingProgramRepository.SaveCustomProgram(program);
        }

        private void ValidateCustomProgram(CustomProgramRequest request)
        {
            if (request == null)
                throw new BusinessException("Dados do programa não informados.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new BusinessException("Informe o nome do programa.");

            if (string.IsNullOrWhiteSpace(request.Food))
                throw new BusinessException("Informe o alimento.");

            if (request.TimeInSeconds < 1)
                throw new BusinessException("Informe um tempo válido.");

            if (request.Power < 1 || request.Power > 10)
                throw new BusinessException("A potência deve estar entre 1 e 10.");

            if (string.IsNullOrWhiteSpace(request.HeatingChar))
                throw new BusinessException("Informe o caractere de aquecimento.");

            var heatingChar = request.HeatingChar.Trim();

            if (heatingChar == ".")
                throw new BusinessException("O caractere de aquecimento não pode ser o padrão '.'.");

            if (heatingChar.Length != 1)
                throw new BusinessException("O caractere de aquecimento deve ter apenas 1 caractere.");

            var alreadyExists = heatingProgramRepository
                .GetAllPrograms()
                .Any(x => x.HeatingChar.Equals(heatingChar, StringComparison.OrdinalIgnoreCase));

            if (alreadyExists)
                throw new BusinessException("Esse caractere de aquecimento já está sendo usado.");
        }
    }
}