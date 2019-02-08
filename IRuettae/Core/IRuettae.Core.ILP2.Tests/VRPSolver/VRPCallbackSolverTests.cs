using System.Collections.Generic;
using System.Linq;
using IRuettae.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRuettae.Core.ILP2.VRPSolver.Tests
{
    [TestClass]
    public class VRPCallbackSolverTests
    {
        [TestMethod]
        public void SimpleVRPTests()
        {
            {
                //    0 - 0
                //  /       \
                // 0         0
                //  \       /
                //    0 - 0
                var tspInstance = new OptimizationInput
                {
                    Days = new (int from, int to)[] { (0, 100) },
                    RouteCosts = new[,]
                    {
                        {0, 1, 2, 3, 2},
                        {1, 0, 1, 2, 3},
                        {2, 1, 0, 1, 2},
                        {3, 2, 1, 0, 1},
                        {2, 3, 2, 1, 0}
                    },
                    Santas = new[] { new Santa { Id = 0 } },
                    Visits = new[]
                    {
                        new Visit{Id = 1, Desired = new (int from, int to)[0], Unavailable = new (int from, int to)[0],Duration = 1, WayCostFromHome = 1, WayCostToHome = 1},
                        new Visit{Id = 2, Desired = new (int from, int to)[0], Unavailable = new (int from, int to)[0],Duration = 1, WayCostFromHome = 2, WayCostToHome = 2},
                        new Visit{Id = 3, Desired = new (int from, int to)[0], Unavailable = new (int from, int to)[0],Duration = 1, WayCostFromHome = 3, WayCostToHome = 3},
                        new Visit{Id = 4, Desired = new (int from, int to)[0], Unavailable = new (int from, int to)[0],Duration = 1, WayCostFromHome = 2, WayCostToHome = 2},
                        new Visit{Id = 5, Desired = new (int from, int to)[0], Unavailable = new (int from, int to)[0],Duration = 1, WayCostFromHome = 1, WayCostToHome = 1}
                    }
                };

                var tspResult = new VRPCallbackSolver(tspInstance).SolveVRP(1000);
                Assert.IsNotNull(tspResult);
                Assert.AreEqual(1, tspResult.Count);
                Assert.AreEqual(6, tspResult[0].Length);
                // symmetric solution 5 4 3 2 1 0 would be valid too but symmetry breaking constraint should prevent it from happening
                Assert.IsTrue(tspResult[0].SequenceEqual(new List<int> { 0, 1, 2, 3, 4, 5 }));
            }

            {

                //     4 - 3
                //     \_2_/
                //       |
                //       |
                //       1
                //       |
                //       |
                //       0 V0
                //       |
                //       |
                //       5
                //       |
                //       |
                //      _6_
                //     /   \
                //     8 - 7
                var simpleVRPInstance = new OptimizationInput
                {
                    Days = new (int from, int to)[] { (0, 100) },
                    RouteCosts = new[,]
                    {
                        {0, 2, 3, 3, 4, 6, 7, 7}, // v1
                        {2, 0, 1, 1, 6, 8, 9, 9}, // v2
                        {3, 1, 0, 1, 7, 9, 10, 10}, // v3
                        {3, 1, 1, 0, 7, 9, 10, 10}, // v4
                        {4, 6, 7, 7, 0, 2, 3, 3}, // v5
                        {6, 8, 9, 9, 2, 0, 1, 1}, // v6
                        {7, 9, 10, 10, 3, 1, 0, 1}, // v7
                        {7, 9, 10, 10, 3, 1, 1, 0}, // v8
                    },
                    Santas = new[] { new Santa { Id = 0 }, new Santa { Id = 1 } },
                    Visits = new[]
                    {
                        new Visit{Id = 0, Desired = new (int from, int to)[0], Unavailable = new (int from, int to)[0],Duration = 1, WayCostFromHome = 2, WayCostToHome = 2},
                        new Visit{Id = 1, Desired = new (int from, int to)[0], Unavailable = new (int from, int to)[0],Duration = 1, WayCostFromHome = 4, WayCostToHome = 4},
                        new Visit{Id = 2, Desired = new (int from, int to)[0], Unavailable = new (int from, int to)[0],Duration = 1, WayCostFromHome = 5, WayCostToHome = 5},
                        new Visit{Id = 3, Desired = new (int from, int to)[0], Unavailable = new (int from, int to)[0],Duration = 1, WayCostFromHome = 5, WayCostToHome = 5},
                        new Visit{Id = 4, Desired = new (int from, int to)[0], Unavailable = new (int from, int to)[0],Duration = 1, WayCostFromHome = 2, WayCostToHome = 2},
                        new Visit{Id = 5, Desired = new (int from, int to)[0], Unavailable = new (int from, int to)[0],Duration = 1, WayCostFromHome = 4, WayCostToHome = 4},
                        new Visit{Id = 6, Desired = new (int from, int to)[0], Unavailable = new (int from, int to)[0],Duration = 1, WayCostFromHome = 5, WayCostToHome = 5},
                        new Visit{Id = 7, Desired = new (int from, int to)[0], Unavailable = new (int from, int to)[0],Duration = 1, WayCostFromHome = 5, WayCostToHome = 5},
                    }
                };

                var simpleVRPInstanceResult = new VRPCallbackSolver(simpleVRPInstance).SolveVRP(5000);
                Assert.IsNotNull(simpleVRPInstanceResult);
                Assert.AreEqual(2, simpleVRPInstanceResult.Count);
                Assert.AreEqual(5, simpleVRPInstanceResult[0].Length);
                Assert.AreEqual(5, simpleVRPInstanceResult[1].Length);
                Assert.AreEqual(11,GetRouteLength(simpleVRPInstance.RouteCosts,
                    simpleVRPInstance.Visits.Select(v => v.WayCostFromHome).ToArray(),
                    simpleVRPInstance.Visits.Select(v => v.WayCostToHome).ToArray(),
                    simpleVRPInstanceResult[0].Skip(1).Select(v => v-1).ToArray()
                    )
                );

                Assert.AreEqual(11, GetRouteLength(simpleVRPInstance.RouteCosts,
                        simpleVRPInstance.Visits.Select(v => v.WayCostFromHome).ToArray(),
                        simpleVRPInstance.Visits.Select(v => v.WayCostToHome).ToArray(),
                        simpleVRPInstanceResult[1].Skip(1).Select(v => v - 1).ToArray()
                    )
                );
            }
        }


        private static int GetRouteLength(int[,] distance, int[] distFromHome, int[] distToHome, int[] sequence)
        {
            var length = distFromHome[sequence[0]];
            for (int i = 1; i < sequence.Length; i++)
            {
                length += distance[sequence[i - 1], sequence[i]];
            }

            return length + distToHome[sequence[sequence.Length-1]];
        }
    }
}