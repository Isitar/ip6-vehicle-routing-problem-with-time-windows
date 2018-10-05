using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.ILP.Algorithm.Clustering.Detail
{
    internal class SolverVariables
    {
        /// <summary>
        /// [Santa,Visit] if santa visits this Visit
        /// </summary>
        public GLS.Variable[,] SantaVisit { get; set; }

        /// <summary>
        /// [Santa,Visit] if santa visits this Visit
        /// </summary>
        public GLS.Variable[,] SantaVisitBonus { get; set; }


        /// <summary>
        /// floating [Santa] = VisitTime
        /// </summary>
        public GLS.Variable[] SantaVisitTime { get; set; }

        /// <summary>
        /// floating [Santa] = RouteTime
        /// </summary>
        public GLS.Variable[] SantaRouteCost { get; set; }
        
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
    }
}