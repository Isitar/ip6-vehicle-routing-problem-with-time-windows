using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Algorithm;
using SolverInputData = IRuettae.Core.Algorithm.NoTimeSlicing.SolverInputData;

namespace IRuettae.ConsoleApp
{
    public class TestNewSolution
    {
        public static void Test()
        {
           
        }

        private void TestFakeData()
        {
            const int numberOfDays = 1;
            const int numberOfSantas = 2;
            const int numberOfVisits = 5;
            var santas = new bool[numberOfDays, numberOfSantas]
            {
                { true, true },
                // { true, true }
            };

            var visitsDuration = new int[numberOfVisits] { 0, 3, 3, 3, 3 };

            var t = VisitState.Default;
            var visits = new VisitState[numberOfDays, numberOfVisits]
            {
                { t, t, t, t, t},
                //   { t, t, t, t, t},
            };


            var distances = new int[numberOfVisits, numberOfVisits]
            {
                {0, 2, 2, 2, 2 },
                {2, 0, 1, 3, 4 },
                {2, 1, 0, 2, 3 },
                {2, 3, 2, 0, 1 },
                {2, 4, 3, 1, 0 },
            };
            var dayDuration = new int[numberOfDays]
            {
                10,
                //10
            };
            var solverInputData = new SolverInputData(santas, visitsDuration, visits, distances, dayDuration);
            Starter.Optimise(solverInputData, TargetBuilderType.Default, true);

        }
    }
}
