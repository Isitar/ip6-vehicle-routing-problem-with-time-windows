using System;
using System.Linq;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Models
{
    public class GenAlgStarterData
    {
        public int MaxNumberOfSantas { get; private set; }
        public long MaxNumberOfGenerations { get; } = long.MaxValue;
        public int PopulationSize { get; private set; }

        public double ElitismPercentage { get; } = 0.357;
        public double DirectMutationPercentage { get; } = 0.378;
        public double RandomPercentage { get; } = 0.0;

        public double OrderBasedCrossoverProbability { get; } = 0.884;
        public double MutationProbability { get; } = 0.0;
        public double PositionMutationProbability { get; } = 0.886;

        /// <summary>
        /// Create default regarding the input.
        /// The MaxNumberOfSantas will be set so that additional santas are permitted.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static GenAlgStarterData GetDefault(OptimizationInput input)
        {
            var starterData = new GenAlgStarterData
            {
                MaxNumberOfSantas = input.Santas.Length,
            };

            #region PopulationSize
            {
                const int sizeDefault = 262144;
                const int sizeBigger = 16;

                // number of visits
                var x = input.Visits.Length;
                // PopulationSize
                double y;

                // x -> y
                // 10 -> 262144
                // 20 -> 262144
                // 31 -> 262144
                // 35 -> 262144
                // 50 -> 131072
                // 100 -> 16384
                // 200 -> 16
                // 1000 -> 16

                if (x <= 35) // [-inf,35]
                {
                    y = sizeDefault;
                }
                else if (x > 35 && x <= 50) // (35,50]
                {
                    // linear interpolation
                    // generated with https://mycurvefit.com/
                    // linear fit method: linear regression
                    // y = -8738.133*x + 567978.7
                    y = Math.Round(-8738.133 * x + 567978.7);
                }
                else if (x > 50 && x < 200) // (50,200)
                {
                    // non-linear approximation
                    // generated with https://mycurvefit.com/
                    // non-linear fit method: exponential basic
                    // y = -250.92 + 1036717 * e ^ (-0.0413231 * x)
                    y = Math.Round(-250.92 + 1036717 * Math.Pow(Math.E, (-0.0413231 * x)));
                }
                else // [200,inf]
                {
                    y = sizeBigger;
                }

                starterData.PopulationSize = (int)y;
            }
            #endregion PopulationSize

            return starterData;
        }

        /// <summary>
        /// Create default regarding the input.
        /// The MaxNumberOfSantas will be set so that additional santas are allowed.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static GenAlgStarterData GetDefaultAdditionalSantas(OptimizationInput input)
        {
            // take default and change MaxNumberOfSantas
            var ret = GetDefault(input);
            ret.MaxNumberOfSantas = input.Visits.Length;
            return ret;
        }

        /// <summary>
        /// use GetDefault
        /// </summary>
        private GenAlgStarterData()
        {
        }

        /// <summary>
        /// Should only be used by unit tests.
        /// </summary>
        /// <param name="maxNumberOfSantas"></param>
        /// <param name="maxNumberOfGenerations"></param>
        /// <param name="populationSize"></param>
        /// <param name="elitismPercentage"></param>
        /// <param name="directMutationPercentage"></param>
        /// <param name="randomPercentage"></param>
        /// <param name="orderBasedCrossoverProbability"></param>
        /// <param name="mutationProbability"></param>
        /// <param name="positionMutationProbability"></param>
        public GenAlgStarterData(int maxNumberOfSantas, long maxNumberOfGenerations, int populationSize, double elitismPercentage, double directMutationPercentage, double randomPercentage, double orderBasedCrossoverProbability, double mutationProbability, double positionMutationProbability)
        {
            MaxNumberOfSantas = maxNumberOfSantas;
            MaxNumberOfGenerations = maxNumberOfGenerations;
            PopulationSize = populationSize;
            ElitismPercentage = elitismPercentage;
            DirectMutationPercentage = directMutationPercentage;
            RandomPercentage = randomPercentage;
            OrderBasedCrossoverProbability = orderBasedCrossoverProbability;
            MutationProbability = mutationProbability;
            PositionMutationProbability = positionMutationProbability;
        }

        /// <summary>
        /// Returns if this is a valid configuration.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (MaxNumberOfSantas < 1)
            {
                return false;
            }
            if (MaxNumberOfGenerations < 0)
            {
                return false;
            }
            // one individual for elitism plus one "normal" individual
            if (PopulationSize < 2)
            {
                return false;
            }

            // check percentages
            if (ElitismPercentage <= 0 || ElitismPercentage >= 1)
            {
                return false;
            }
            if (DirectMutationPercentage < 0 || DirectMutationPercentage > 1)
            {
                return false;
            }
            if (RandomPercentage < 0 || RandomPercentage > 1)
            {
                return false;
            }
            if (RandomPercentage < 0 || RandomPercentage > 1)
            {
                return false;
            }
            if (MutationProbability < 0 || MutationProbability > 1)
            {
                return false;
            }
            if (OrderBasedCrossoverProbability < 0 || OrderBasedCrossoverProbability > 1)
            {
                return false;
            }
            if (PositionMutationProbability < 0 || PositionMutationProbability > 1)
            {
                return false;
            }

            // make sure PopulationSize is stable
            var size = (int)Math.Max(1, ElitismPercentage * PopulationSize) + (int)(DirectMutationPercentage * PopulationSize) + (int)(RandomPercentage * PopulationSize);
            if (size < 0 || size > PopulationSize)
            {
                return false;
            }

            return true;
        }

        public override string ToString() => string.Join(Environment.NewLine, GetType().GetProperties().Select(p => $"{p.Name}: {(p.GetIndexParameters().Length > 0 ? "Indexed Property cannot be used" : p.GetValue(this, null))}"));
    }
}
