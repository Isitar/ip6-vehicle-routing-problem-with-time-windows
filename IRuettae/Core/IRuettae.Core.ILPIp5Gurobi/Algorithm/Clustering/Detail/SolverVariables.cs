using Gurobi;

namespace IRuettae.Core.ILPIp5Gurobi.Algorithm.Clustering.Detail
{
    internal class SolverVariables
    {
        /// <summary>
        /// [Santa] is santa used
        /// </summary>
        public GRBVar[] SantaUsed { get; set; }

        /// <summary>
        /// [Santa,Visit] if santa visits this Visit
        /// </summary>
        public GRBVar[,] SantaVisit { get; set; }

        /// <summary>
        /// [Santa,Visit] if santa visits this Visit
        /// </summary>
        public GRBVar[,] SantaVisitBonus { get; set; }


        /// <summary>
        /// floating [Santa] = VisitTime
        /// </summary>
        public GRBVar[] SantaVisitTime { get; set; }

        /// <summary>
        /// floating [Santa] = RouteTime
        /// </summary>
        public GRBVar[] SantaRouteCost { get; set; }

        /// <summary>
        /// [santa][source,destination] if santa uses this way (calculated by spanningTree)
        /// </summary>
        public GRBVar[][,] SantaUsesWay { get; set; }

        /// <summary>
        /// [santa][source,destination] how much flow way has
        /// </summary>
        public GRBVar[][,] SantaWayFlow { get; set; }

        /// <summary>
        /// [santa][source,destination] how much flow way has
        /// </summary>
        public GRBVar[][,] SantaWayHasFlow { get; set; }
    }
}