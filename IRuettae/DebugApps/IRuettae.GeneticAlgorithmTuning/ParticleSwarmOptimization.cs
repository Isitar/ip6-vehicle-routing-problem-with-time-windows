using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.GeneticAlgorithmTuning
{
    internal class ParticleSwarmOptimization
    {
        // settings
        // 17280 evaluations should take about 3 hours with degree of parallelism=8
        // 17280=3600*3/(5/8)
        // there are 7 design variables to optimze
        // the values are taken from // Todo cite
#if true
        // problem dimension=10
        // fitness evaluations=20000
        const int NumberOfGenerations = 326; // total 17278 evaluations
        const int PopulationSize = 53;
        const double initW = -0.3488; // inertia weight
        const double initC1 = -0.2746; // cognitive parameter
        const double initC2 = 4.8976; // social parameter
#elif false
        // problem dimension=5
        // fitness evaluations=10000
        const int NumberOfGenerations = 77; // total 17171 evaluations
        const int PopulationSize = 223;
        const double initW = -0.3699; // inertia weight
        const double initC1 = -0.1207; // cognitive parameter
        const double initC2 = 3.3657; // social parameter
#else
        // problem dimension=5
        // fitness evaluations=10000
        const int NumberOfGenerations = 85; // total 17255 evaluations
        const int PopulationSize = 203;
        const double initW = 0.5069; // inertia weight
        const double initC1 = 2.5524; // cognitive parameter
        const double initC2 = 1.0056; // social parameter
#endif

        private readonly int numberOfVars;
        private readonly Func<double[], double> objective;
        private readonly Func<Random, double[]> createVariables;
        private readonly Action<double[]> boundVariable;
        private readonly Func<Random, double[]> createVelocity;
        private readonly Action<double[]> boundVelocity;
        private readonly string[] names;
        private readonly Random random = new Random();

        public ParticleSwarmOptimization(
            Func<double[], double> objectiveFunction,
            Func<Random, double[]> createVariables,
            Action<double[]> boundVariable,
            Func<Random, double[]> createVelocity,
            Action<double[]> boundVelocity,
            string[] designVariableNames)
        {
            if (createVariables(random).Length != designVariableNames.Length || createVelocity(random).Length != designVariableNames.Length)
            {
                throw new ArgumentException("constructor parameters are not consistent");
            }

            objective = objectiveFunction;
            this.createVariables = createVariables;
            this.boundVariable = boundVariable;
            this.createVelocity = createVelocity;
            this.boundVelocity = boundVelocity;
            numberOfVars = designVariableNames.Length;
            names = designVariableNames;
        }

        public void Run()
        {
            // settings
            const double w = initW;  // inertia weight
            const double c1 = initC1; // cognitive parameter
            const double c2 = initC2; // social parameter

            // design variable vector's
            var x = new double[PopulationSize][];
            var velocity = new double[PopulationSize][]; // design velocity vector's
            var bestFitness = new double[PopulationSize]; // best "fitness"

            // init
            for (int p = 0; p < PopulationSize; p++)
            {
                x[p] = createVariables(random);
                velocity[p] = createVelocity(random);
                bestFitness[p] = double.MaxValue;
            }


            var invididualBestPosition = new double[PopulationSize, numberOfVars]; // individual best position
            var globalBestFitness = double.MaxValue; // global best fitness
            var globalBestPosition = new double[numberOfVars]; // global best position

            // start of evolution
            for (var g = 1; g <= NumberOfGenerations; g++)
            {
                var calculatedNewFitness = x.AsParallel().Select(s => objective(s)).ToArray();

                // loop over generations
                for (int p = 0; p < PopulationSize; p++)
                {
                    // search indivial best and global best
                    var newFitness = calculatedNewFitness[p];
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
                var output = new StringBuilder();
                output.AppendLine($"generation {g} (time: {DateTime.Now:yy-MM-dd-HH-mm-ss})");
                output.AppendLine($"stdev={StDev(calculatedNewFitness)}");
                for (int m = 0; m < numberOfVars; m++)
                {
                    output.AppendLine($"global best {names[m]}={globalBestPosition[m]}");
                }
                output.AppendLine($"w={w} f={globalBestFitness}");
                output.AppendLine();

                Console.WriteLine(output);
                using (var sw = new StreamWriter("pso-log.txt", true))
                {
                    sw.WriteLine(output);
                }

                // Update velocity & position
                for (int p = 0; p < PopulationSize; p++)
                {
                    for (int m = 0; m < numberOfVars; m++)
                    {
                        velocity[p][m] = w * velocity[p][m] + c1 * random.NextDouble() * (invididualBestPosition[p, m] - x[p][m])
                                              + c2 * random.NextDouble() * (globalBestPosition[m] - x[p][m]);
                    }
                    boundVelocity(velocity[p]);
                    for (int m = 0; m < numberOfVars; m++)
                    {
                        x[p][m] = x[p][m] + velocity[p][m];
                    }
                    boundVariable(x[p]);
                }
            }
        }

        public static double StDev(IReadOnlyList<double> values)
        {
            double ret = 0.0;
            var count = values.Count();
            if (count > 1)
            {
                var avg = values.Average();
                var sum = values.Sum(d => (d - avg) * (d - avg));
                ret = Math.Sqrt(sum / count);
            }
            return ret;
        }
    }
}