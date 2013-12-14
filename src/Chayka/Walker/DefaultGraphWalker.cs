namespace Chayka.Walker
{
    using System.Linq;
    using Chayka.Lookup;
    using Chayka.PathFinder;
    using Chayka.PathFinder.RandomWalk;

    public class DefaultGraphWalker<T>
        : IGraphWalker<T>
    {
        private readonly IGraph<T> graph;
        private readonly IVertexFinder<T> vertexFinder;
        private readonly IEdgeFinder<T> edgeFinder;
        private readonly IRandomizer randomizer;
        private readonly ITraverseableEdgeChecker<T> edgeChecker;

        public DefaultGraphWalker(IGraph<T> graph, IVertexFinder<T> vertexFinder, IEdgeFinder<T> edgeFinder, IRandomizer randomizer, ITraverseableEdgeChecker<T> edgeChecker)
        {
            this.graph = graph;
            this.vertexFinder = vertexFinder;
            this.edgeFinder = edgeFinder;
            this.randomizer = randomizer;
            this.edgeChecker = edgeChecker;
        }

        public void WalkBetween(T souce, T target, PathType pathType)
        {
            var pathFinder = this.graph.CreatePathFinder(pathType);

            var path = pathFinder.PathBetween(souce, target);

            foreach (var edge in path)
            {
                edge.OnTraverse();
            }
        }

        public void RandomWalk(T startVertex, int steps)
        {
            var currentVertex = this.vertexFinder.Find(this.graph, startVertex);

            if (Equals(null, currentVertex))
            {
                throw GraphLookupException.VertexNotFound(startVertex);
            }

            for (var i = 0; i < steps; ++i)
            {
                var currentEdge = (from edge in this.edgeFinder.FindEgesFrom(this.graph, currentVertex, this.edgeChecker)
                                   orderby this.randomizer.NextInt(int.MaxValue)
                                   select edge).FirstOrDefault();

                if (currentEdge == null)
                {
                    throw GraphTraversalException.StuckOn(currentVertex);
                }

                currentEdge.OnTraverse();

                currentVertex = currentEdge.Target;
            }
        }
    }
}