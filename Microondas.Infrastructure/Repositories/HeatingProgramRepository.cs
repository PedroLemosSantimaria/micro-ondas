using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microondas.Core.Entities;
using Microondas.Core.Interfaces;

namespace Microondas.Infrastructure.Repositories
{
    public class HeatingProgramRepository : IHeatingProgramRepository
    {
        private readonly string customProgramsFilePath;
        private readonly List<HeatingProgram> defaultPrograms;

        public HeatingProgramRepository()
        {
            var dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            customProgramsFilePath = Path.Combine(dataFolder, "customPrograms.json");

            if (!File.Exists(customProgramsFilePath))
            {
                File.WriteAllText(customProgramsFilePath, "[]");
            }

            defaultPrograms = CreateDefaultPrograms();
        }

        public List<HeatingProgram> GetAllPrograms()
        {
            var customPrograms = GetCustomPrograms();

            return defaultPrograms
                .Concat(customPrograms)
                .ToList();
        }

        public HeatingProgram GetProgramByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return GetAllPrograms()
                .FirstOrDefault(x =>
                    x != null &&
                    !string.IsNullOrWhiteSpace(x.Name) &&
                    x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void SaveCustomProgram(HeatingProgram program)
        {
            if (program == null)
                return;

            var programs = GetCustomPrograms();

            var alreadyExists = programs.Any(x =>
                x != null &&
                !string.IsNullOrWhiteSpace(x.Name) &&
                x.Name.Equals(program.Name, StringComparison.OrdinalIgnoreCase));

            if (alreadyExists)
                return;

            programs.Add(program);

            var json = JsonSerializer.Serialize(programs, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(customProgramsFilePath, json);
        }

        private List<HeatingProgram> GetCustomPrograms()
        {
            if (!File.Exists(customProgramsFilePath))
                return new List<HeatingProgram>();

            var json = File.ReadAllText(customProgramsFilePath);

            if (string.IsNullOrWhiteSpace(json))
                return new List<HeatingProgram>();

            var programs = JsonSerializer.Deserialize<List<HeatingProgram>>(json) ?? new List<HeatingProgram>();

            programs = programs
                .Where(x =>
                    x != null &&
                    !string.IsNullOrWhiteSpace(x.Name) &&
                    !string.IsNullOrWhiteSpace(x.Food) &&
                    !string.IsNullOrWhiteSpace(x.HeatingChar) &&
                    x.TimeInSeconds > 0 &&
                    x.Power >= 1 &&
                    x.Power <= 10)
                .ToList();

            return programs;
        }

        private List<HeatingProgram> CreateDefaultPrograms()
        {
            return new List<HeatingProgram>
            {
                new HeatingProgram(
                    "Pipoca",
                    "Pipoca de micro-ondas",
                    180,
                    7,
                    "*",
                    "Observar o barulho de estouros do milho. Caso haja um intervalo de mais de 10 segundos entre um estouro e outro, interrompa o aquecimento.",
                    true),

                new HeatingProgram(
                    "Leite",
                    "Leite",
                    300,
                    5,
                    "~",
                    "Cuidado com aquecimento de líquidos. O choque térmico aliado ao movimento do recipiente pode causar fervura imediata com risco de queimaduras.",
                    true),

                new HeatingProgram(
                    "Carnes de boi",
                    "Carne em pedaço ou fatias",
                    840,
                    4,
                    "#",
                    "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para um descongelamento mais uniforme.",
                    true),

                new HeatingProgram(
                    "Frango",
                    "Frango (qualquer corte)",
                    480,
                    7,
                    "@",
                    "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para um descongelamento mais uniforme.",
                    true),

                new HeatingProgram(
                    "Feijão",
                    "Feijão congelado",
                    480,
                    9,
                    "%",
                    "Deixe o recipiente destampado e, se for de plástico, tenha cuidado ao retirar, pois ele pode perder resistência em altas temperaturas.",
                    true)
            };
        }
    }
}