namespace Chayka.PathFinder.ShortestPath
{
    using System.Collections.Generic;
    using System.Linq;
    using Chayka.GraphBuilder;
    using QuickGraph.Algorithms.ShortestPath;

    public class ShortestPathFinder<T>
        : PathFinderBase<T>
    {
        private readonly FloydWarshallAllShortestPathAlgorithm<IVertex<T>, QuickGraphEdge<T>> algorithm;

        public ShortestPathFinder(IEnumerable<IVertex<T>> vertices, IEnumerable<IEdge<IVertex<T>>> edges)
        {
            var graph = QuickGraphGraphBuilder<T>.Build(vertices, edges);
            this.algorithm = new FloydWarshallAllShortestPathAlgorithm<IVertex<T>, QuickGraphEdge<T>>(graph, edge => 1);
            this.algorithm.Compute();
        }

        public override bool TryGetPathBetween(IVertex<T> source, IVertex<T> target, out IEnumerable<IEdge<IVertex<T>>> path)
        {
            if (Equals(source, target))
            {
                path = Enumerable.Empty<IEdge<IVertex<T>>>();
                return true;
            }

            IEnumerable<QuickGraphEdge<T>> qgPath;
            var hasPath = algorithm.TryGetPath(source, target, out qgPath);

            path = (qgPath ?? Enumerable.Empty<QuickGraphEdge<T>>())
                    .Select(edge => edge.WrappedEdge);
            return hasPath;
        }
    }
}