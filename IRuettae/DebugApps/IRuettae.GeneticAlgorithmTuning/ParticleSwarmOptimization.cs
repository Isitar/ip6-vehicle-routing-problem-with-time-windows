using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.GeneticAlgorithmTuning
{
    internal class ParticleSwarmOptimization
    {
        private readonly int n;
        const int ngen = 150;
        const int npop = 100;
        const double xmin = -5;
        const double xmax = 5;

        private double w = 1; // insertia weight
        private double c1 = 2; // cognitive parameter
        private double c2 = 2; // social parameter

        private readonly Func<double[], double> objective;

        public ParticleSwarmOptimization(Func<double[], double> objectiveFunction, int numberOfDesignVariables)
        {
            objective = objectiveFunction;
            n = numberOfDesignVariables;
        }

        public void Run()
        {
            var random = new Random();
            var x = new double[npop][]; // design variable vector's
            var v = new double[npop, n]; // design velocity vector's
            var f = new double[npop]; // best "fitness"
            var ibest = new double[npop, n]; // individual best position
            var gbest = new double[n]; // global best position
            var fbest = double.MaxValue; // global best fitness
            double fnew; // global best fitness

            // init
            for (int p = 0; p < npop; p++)
            {
                for (int m = 0; m < n; m++)
                {
                    x[p] = new double[n];
                    x[p][m] = xmin + (xmax - xmin) * random.NextDouble();
                }
                f[p] = double.MaxValue;
            }

            // hier startet die "Evolution"
            for (int g = 1; g < ngen; g++)
            {
                // Loop über Generationen

                for (int p = 0; p < npop; p++)
                {
                    // Suche individual best und global best
                    fnew = objective(x[p]);
                    if (fnew < f[p])
                    {
                        // Individual best
                        f[p] = fnew;
                        for (int m = 0; m < n; m++)
                        {
                            ibest[p, m] = x[p][m];
                        }
                        fbest = fnew;
                        // Global best
                        for (int m = 0; m < n; m++)
                        {
                            gbest[m] = x[p][m];
                        }
                    }
                }

                // Output progress
                Console.WriteLine($"generation {g}");
                for (int m = 0; m < n; m++)
                {
                    Console.WriteLine($"global best[{m}]={gbest[m]}");
                }
                Console.WriteLine($"w={w} f={fbest}");

                // Update velocity & position
                for (int p = 0; p < npop; p++)
                {
                    for (int m = 0; m < n; m++)
                    {
                        v[p, m] = w * v[p, m] + c1 * random.NextDouble() * (ibest[p, m] - x[p][m])
                                              + c2 * random.NextDouble() * (gbest[m] - x[p][m]);
                        x[p][m] = x[p][m] + v[p, m];
                    }
                }
                // Intertia weight changes
                w = w * 0.99;
            }
        }
    }
}