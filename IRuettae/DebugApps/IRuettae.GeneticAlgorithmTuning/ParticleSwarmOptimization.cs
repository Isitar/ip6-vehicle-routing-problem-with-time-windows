using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.GeneticAlgorithmTuning
{
    internal class ParticleSwarmOptimization
    {
        private readonly int numberOfVars;
        const int NumberOfGenerations = 150;
        const int PopulationSize = 100;
        const double xmin = -5;
        const double xmax = 5;

        private double w = 1; // inertia weight
        private double c1 = 2; // cognitive parameter
        private double c2 = 2; // social parameter

        private readonly Func<double[], double> objective;

        public ParticleSwarmOptimization(Func<double[], double> objectiveFunction, int numberOfDesignVariables)
        {
            objective = objectiveFunction;
            numberOfVars = numberOfDesignVariables;
        }

        public void Run()
        {
            var random = new Random();
            // design variable vector's
            var x = new double[PopulationSize][];
            var bestFitness = new double[PopulationSize]; // best "fitness"

            // init
            for (int p = 0; p < PopulationSize; p++)
            {
                for (int m = 0; m < numberOfVars; m++)
                {
                    x[p] = new double[numberOfVars];
                    x[p][m] = xmin + (xmax - xmin) * random.NextDouble();
                }
                bestFitness[p] = double.MaxValue;
            }

            var invididualBestPosition = new double[PopulationSize, numberOfVars]; // individual best position
            var velocity = new double[PopulationSize, numberOfVars]; // design velocity vector's
            var globalBestFitness = double.MaxValue; // global best fitness
            var globalBestPosition = new double[numberOfVars]; // global best position

            // hier startet die "Evolution"
            for (var g = 1; g <= NumberOfGenerations; g++)
            {
                // Loop über Generationen

                for (int p = 0; p < PopulationSize; p++)
                {
                    // Suche individual best und global best
                    var newFitness = objective(x[p]);
                    if (newFitness < bestFitness[p])
                    {
                        // Individual best
                        bestFitness[p] = newFitness;
                        for (int m = 0; m < numberOfVars; m++)
                        {
                            invididualBestPosition[p, m] = x[p][m];
                        }
                        if (newFitness < globalBestFitness)
                        {
                            // global best
                            globalBestFitness = newFitness;
                            for (int m = 0; m < numberOfVars; m++)
                            {
                                globalBestPosition[m] = x[p][m];
                            }
                        }
                    }
                }

                // Output progress
                Console.WriteLine($"generation {g}");
                for (int m = 0; m < numberOfVars; m++)
                {
                    Console.WriteLine($"global best[{m}]={globalBestPosition[m]}");
                }
                Console.WriteLine($"w={w} f={globalBestFitness}");

                // Update velocity & position
                for (int p = 0; p < PopulationSize; p++)
                {
                    for (int m = 0; m < numberOfVars; m++)
                    {
                        velocity[p, m] = w * velocity[p, m] + c1 * random.NextDouble() * (invididualBestPosition[p, m] - x[p][m])
                                              + c2 * random.NextDouble() * (globalBestPosition[m] - x[p][m]);
                        x[p][m] = x[p][m] + velocity[p, m];
                    }
                }
                // Inertia weight changes
                w = w * 0.99;
            }
        }
    }
}