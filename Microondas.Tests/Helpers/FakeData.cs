using System.Collections.Generic;
using Microondas.Core.Entities;

namespace Microondas.Tests.Helpers
{
    public static class FakeData
    {
        public static List<HeatingProgram> GetPrograms()
        {
            return new List<HeatingProgram>
            {
                new HeatingProgram(
                    "Pipoca",
                    "Pipoca de micro-ondas",
                    180,
                    7,
                    "*",
                    "Teste",
                    true),

                new HeatingProgram(
                    "Leite",
                    "Leite",
                    300,
                    5,
                    "~",
                    "Teste",
                    true),

                new HeatingProgram(
                    "Frango custom",
                    "Frango",
                    90,
                    6,
                    "@",
                    "Teste",
                    false)
            };
        }
    }
}