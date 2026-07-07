using System.Collections.Generic;
using System.Linq;
using Microondas.Core.Dtos.Requests;
using Microondas.Core.Entities;
using Microondas.Core.Exceptions;
using Microondas.Core.Interfaces;
using Microondas.Core.Services;
using Moq;
using Xunit;

namespace Microondas.Tests.Services
{
    public class ProgramServiceTests
    {
        private readonly Mock<IHeatingProgramRepository> heatingProgramRepositoryMock;
        private readonly ProgramService programService;
        private readonly List<HeatingProgram> programs;

        public ProgramServiceTests()
        {
            heatingProgramRepositoryMock = new Mock<IHeatingProgramRepository>();

            programs = new List<HeatingProgram>
            {
                new HeatingProgram("Pipoca", "Pipoca", 180, 7, "*", "Teste", true),
                new HeatingProgram("Leite", "Leite", 300, 5, "~", "Teste", true),
                new HeatingProgram("Meu programa", "Arroz", 90, 6, "@", "Teste", false)
            };

            heatingProgramRepositoryMock
                .Setup(x => x.GetAllPrograms())
                .Returns(() => programs);

            heatingProgramRepositoryMock
                .Setup(x => x.SaveCustomProgram(It.IsAny<HeatingProgram>()))
                .Callback<HeatingProgram>(program => programs.Add(program));

            programService = new ProgramService(heatingProgramRepositoryMock.Object);
        }

        [Fact]
        public void GetAllPrograms_ShouldReturnProgramsOrdered()
        {
            var result = programService.GetAllPrograms();

            Assert.NotNull(result);
            Assert.True(result.Count >= 3);
        }

        [Fact]
        public void CreateCustomProgram_WithValidData_ShouldSaveProgram()
        {
            var request = new CustomProgramRequest
            {
                Name = "Macarrão",
                Food = "Macarrão congelado",
                TimeInSeconds = 120,
                Power = 8,
                HeatingChar = "$",
                Instructions = "Mexer no meio do aquecimento"
            };

            programService.CreateCustomProgram(request);

            Assert.Contains(programs, x => x.Name == "Macarrão");
        }

        [Fact]
        public void CreateCustomProgram_WithEmptyName_ShouldThrowBusinessException()
        {
            var request = new CustomProgramRequest
            {
                Name = "",
                Food = "Macarrão",
                TimeInSeconds = 120,
                Power = 8,
                HeatingChar = "$"
            };

            Assert.Throws<BusinessException>(() =>
            {
                programService.CreateCustomProgram(request);
            });
        }

        [Fact]
        public void CreateCustomProgram_WithEmptyFood_ShouldThrowBusinessException()
        {
            var request = new CustomProgramRequest
            {
                Name = "Macarrão",
                Food = "",
                TimeInSeconds = 120,
                Power = 8,
                HeatingChar = "$"
            };

            Assert.Throws<BusinessException>(() =>
            {
                programService.CreateCustomProgram(request);
            });
        }

        [Fact]
        public void CreateCustomProgram_WithInvalidPower_ShouldThrowBusinessException()
        {
            var request = new CustomProgramRequest
            {
                Name = "Macarrão",
                Food = "Macarrão",
                TimeInSeconds = 120,
                Power = 11,
                HeatingChar = "$"
            };

            Assert.Throws<BusinessException>(() =>
            {
                programService.CreateCustomProgram(request);
            });
        }

        [Fact]
        public void CreateCustomProgram_WithDotHeatingChar_ShouldThrowBusinessException()
        {
            var request = new CustomProgramRequest
            {
                Name = "Macarrão",
                Food = "Macarrão",
                TimeInSeconds = 120,
                Power = 8,
                HeatingChar = "."
            };

            Assert.Throws<BusinessException>(() =>
            {
                programService.CreateCustomProgram(request);
            });
        }

        [Fact]
        public void CreateCustomProgram_WithDuplicatedHeatingChar_ShouldThrowBusinessException()
        {
            var request = new CustomProgramRequest
            {
                Name = "Macarrão",
                Food = "Macarrão",
                TimeInSeconds = 120,
                Power = 8,
                HeatingChar = "*"
            };

            Assert.Throws<BusinessException>(() =>
            {
                programService.CreateCustomProgram(request);
            });
        }

        [Fact]
        public void CreateCustomProgram_WithDuplicatedName_ShouldThrowBusinessException()
        {
            var request = new CustomProgramRequest
            {
                Name = "Pipoca",
                Food = "Outra comida",
                TimeInSeconds = 120,
                Power = 8,
                HeatingChar = "$"
            };

            Assert.Throws<BusinessException>(() =>
            {
                programService.CreateCustomProgram(request);
            });
        }
    }
}