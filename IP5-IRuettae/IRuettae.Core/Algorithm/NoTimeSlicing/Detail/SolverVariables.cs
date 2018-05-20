using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.NoTimeSlicing.Detail
{
    internal class SolverVariables
    {
        /// <summary>
        /// [Santa,Visit] if santa visits this visit
        /// </summary>
        public GLS.Variable[,] SantaVisit { get; set; }


        /// <summary>
        /// floating [Santa] = VisitTime
        /// </summary>
        public GLS.Variable[] SantaVisitTime { get; set; }

        /// <summary>
        /// floating [Santa] = RouteTime
        /// </summary>
        public GLS.Variable[] SantaRouteCost { get; set; }

        /// <summary>
        /// [santa][source,destination] if santa visits both, source & dest
        /// Used for spanningTree Constraint
        /// </summary>
        public GLS.Variable[][,] SantaGraphEdge { get; set; }

        /// <summary>
        /// [santa][source,destination] if santa uses this way (calculated by spanningTree)
        /// </summary>
        public GLS.Variable[][,] SantaUsesWay { get; set; }

        /// <summary>
        /// [santa][source,destination] how much flow way has
        /// </summary>
        public GLS.Variable[][,] SantaWayFlow { get; set; }

        /// <summary>
        /// [santa][source,destination] how much flow way has
        /// </summary>
        public GLS.Variable[][,] SantaWayHasFlow { get; set; }
        /// <summary>
        /// [santa,visit] how much flow way has
        /// </summary>
        public GLS.Variable[,] SantaVisitFlow { get; set; }

    }
}