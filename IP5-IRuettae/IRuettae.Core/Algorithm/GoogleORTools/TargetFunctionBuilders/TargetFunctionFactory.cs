﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;
using IRuettae.Core.Algorithm.GoogleORTools.Detail;

namespace IRuettae.Core.Algorithm.GoogleORTools.TargetFunctionBuilders
{
    internal class TargetFunctionFactory
    {
        private readonly SolverData solverData;

        public TargetFunctionFactory(SolverData solverData)
        {
            this.solverData = solverData;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="weight"></param>
        /// <returns>LinearExpr which should be minimised</returns>
        public LinearExpr CreateTargetFunction(TargetType target, double? weight = 1.0)
        {
            switch (target)
            {
                case TargetType.MinTime:
                    return CreateTargetFunctionMinTime(weight);
                case TargetType.MinSantas:
                    return CreateTargetFunctionMinSantas(weight);
                case TargetType.MinSantaShifts:
                    return CreateTargetFunctionMinSantaShifts(weight);
                case TargetType.TryVisitEarly:
                    return CreateTargetFunctionTryVisitEarly(weight);
                default:
                    throw new NotSupportedException($"The type {target} is not supported.");
            }
        }

        private LinearExpr CreateTargetFunctionTryVisitEarly(double? weight)
        {
            var maxWeight = 0.0;
            var sum = new LinearExpr[solverData.NumberOfDays];
            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                sum[day] = new LinearExpr();
                for (int timeslice = 1; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                {
                    for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                    {
                        sum[day] += solverData.Variables.SantaEnRoute[day][santa, timeslice] * timeslice;

                    }
                }
                double slices = solverData.SlicesPerDay[day];

                // gauss sum formula
                maxWeight += (slices * slices + slices) / 2;
            }

            if (weight.HasValue)
            {
                weight /= maxWeight;
            }
            return LinearExprArrayHelper.Sum(sum) * (weight ?? 1.0);
        }

        private LinearExpr CreateTargetFunctionMinTime(double? weight)
        {
            var sum = new LinearExpr[solverData.NumberOfDays];
            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                sum[day] = new LinearExpr();
                foreach (var v in solverData.Variables.SantaEnRoute[day])
                {
                    sum[day] += v;
                }
            }
            return LinearExprArrayHelper.Sum(sum);
        }

        private LinearExpr CreateTargetFunctionMinSantaShifts(double? weight)
        {
            return solverData.Variables.NumberOfSantasNeeded.Sum();
        }

        private LinearExpr CreateTargetFunctionMinSantas(double? weight)
        {
            return solverData.Variables.NumberOfSantasNeededOverall;
        }
    }
}
