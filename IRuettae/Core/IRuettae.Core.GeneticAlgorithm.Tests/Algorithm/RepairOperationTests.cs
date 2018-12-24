using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRuettae.Core.GeneticAlgorithm.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Tests
{
    [TestClass()]
    public class RepairOperationTests
    {
        private OptimizationInput GetInput()
        {
            return new OptimizationInput()
            {
                Santas = new Santa[]
                {
                    new Santa
                    {
                        Id = 100,
                    },
                    new Santa
                    {
                        Id = 200,
                    },
                },
                Visits = new Visit[]
                {
                    new Visit
                    {
                        Id = 0,
                        IsBreak = false,
                        SantaId = -1,
                    },
                    new Visit
                    {
                        Id = 1,
                        IsBreak = false,
                        SantaId = -1,
                    },
                    new Visit
                    {
                        Id = 2,
                        IsBreak = false,
                        SantaId = -1,
                    },
                    new Visit
                    {
                        Id = 3,
                        IsBreak = false,
                        SantaId = -1,
                    },
                    new Visit // will not be visited
                    {
                        Id = 4,
                        IsBreak = true,
                        SantaId = 200,
                    }
                },
                Days = new(int, int)[]
                {
                    (1, 2),
                    (3, 4),
                },
            };
        }

        private Dictionary<int, int> GetMapping()
        {
            return new Dictionary<int, int>
            {
                { 0, 0},
                { 1, 1},
                { 2, 2},
                { 3, 3},
                { 4, 4},
                { 5, 4},
            };
        }

        [TestMethod()]
        public void TestDecode()
        {
            var actual = new Genotype { 4, 0, -2, 1, 2, -123, -2, -2, -2, 3, 5 };
            var expected = new Genotype { 0, -2, 1, 4, 2, -123, -2, -2, 5, -2, 3 };
            var repairOperation = new RepairOperation(GetInput(), GetMapping());

            repairOperation.Repair(actual);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TestDecode_Unchanged()
        {
            var actual = new Genotype { 0, 1, -2, 4, -123, -2, 2, -2, 5, -2, 3 };
            var expected = new Genotype { 0, 1, -2, 4, -123, -2, 2, -2, 5, -2, 3 };
            var repairOperation = new RepairOperation(GetInput(), GetMapping());

            repairOperation.Repair(actual);

            Assert.AreEqual(expected, actual);
        }
    }
}