namespace Chayka
{
    using System.Collections.Generic;
    using System.Linq;
    using QuickGraph.Algorithms.ShortestPath;

    public class ShortestPathGraph<T>
        : QuickGraphGraph<T>
    {
        private readonly FloydWarshallAllShortestPathAlgorithm<T, QuickGraph.IEdge<T>> algorithm;

        public ShortestPathGraph(IEnumerable<IVertex<T>> vertices, IEnumerable<IEdge<T>> edges) 
            : base(vertices, edges)
        {
            this.algorithm = new FloydWarshallAllShortestPathAlgorithm<T, QuickGraph.IEdge<T>>(this.Graph, edge => 1);
            this.algorithm.Compute();
        }

        public override bool TryGetPathBetween(T source, T target, out IEnumerable<IEdge<T>> path)
        {
            IEnumerable<QuickGraph.IEdge<T>> qgPath;
            var hasPath = algorithm.TryGetPath(source, target, out qgPath);

            path = (qgPath ?? Enumerable.Empty<QuickGraph.IEdge<T>>())
                    .Cast<QuickGraphEdge>()
                    .Select(edge => edge.WrappedEdge);
            return hasPath;
        }
    }
}