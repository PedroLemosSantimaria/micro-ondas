using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microondas.Core.Entities;
using Microondas.Core.Interfaces;

namespace Microondas.Infrastructure.Repositories
{
    public class HeatingProgramRepository : IHeatingProgramRepository
    {
        private readonly string customProgramsFilePath;

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
        }

        public List<HeatingProgram> GetDefaultPrograms()
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
                    true
                ),

                new HeatingProgram(
                    "Leite",
                    "Leite",
                    300,
                    5,
                    "~",
                    "Cuidado com aquecimento de líquidos. O choque térmico aliado ao movimento do recipiente pode causar fervura imediata com risco de queimaduras.",
                    true
                ),

                new HeatingProgram(
                    "Carnes de boi",
                    "Carne em pedaço ou fatias",
                    840,
                    4,
                    "#",
                    "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para um descongelamento mais uniforme.",
                    true
                ),

                new HeatingProgram(
                    "Frango",
                    "Frango (qualquer corte)",
                    480,
                    7,
                    "@",
                    "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para um descongelamento mais uniforme.",
                    true
                ),

                new HeatingProgram(
                    "Feijão",
                    "Feijão congelado",
                    480,
                    9,
                    "%",
                    "Deixe o recipiente destampado e, se for de plástico, tenha cuidado ao retirar, pois ele pode perder resistência em altas temperaturas.",
                    true
                )
            };
        }

        public List<HeatingProgram> GetCustomPrograms()
        {
            if (!File.Exists(customProgramsFilePath))
            {
                return new List<HeatingProgram>();
            }

            var json = File.ReadAllText(customProgramsFilePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<HeatingProgram>();
            }

            var programs = JsonSerializer.Deserialize<List<HeatingProgram>>(json);

            return programs ?? new List<HeatingProgram>();
        }

        public List<HeatingProgram> GetAllPrograms()
        {
            var allPrograms = new List<HeatingProgram>();

            allPrograms.AddRange(GetDefaultPrograms());
            allPrograms.AddRange(GetCustomPrograms());

            return allPrograms;
        }

        public void SaveCustomProgram(HeatingProgram program)
        {
            var customPrograms = GetCustomPrograms();
            customPrograms.Add(program);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(customPrograms, options);
            File.WriteAllText(customProgramsFilePath, json);
        }
    }
}