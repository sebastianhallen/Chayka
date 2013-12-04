namespace Chayka
{
    using System.Collections.Generic;
    using System.Linq;
    using QuickGraph;
    using QuickGraph.Algorithms.ShortestPath;

    public class QuickGraphGraph<T>
        : IGraph<T>
    {
        private readonly IVertexAndEdgeListGraph<T, QuickGraphEdge> graph;

        private class QuickGraphEdge 
            : QuickGraph.IEdge<T>
        {
            public T Source { get; private set; }
            public T Target { get; private set; }
            public IEdge<T> WrappedEdge { get; private set; }
            
            public QuickGraphEdge(IEdge<T> edge)
            {
                this.WrappedEdge = edge;
                this.Source = edge.Source;
                this.Target = edge.Target;
            }
        }

        public QuickGraphGraph(IEnumerable<IVertex<T>> vertices, IEnumerable<IEdge<T>> edges)
        {
            var bidirectionalGraph = new BidirectionalGraph<T, QuickGraphEdge>();
            
            bidirectionalGraph.AddVertexRange(vertices.Select(vertex => vertex.Content));
            bidirectionalGraph.AddEdgeRange(edges.Select(edge => new QuickGraphEdge(edge)));

            this.graph = new ArrayBidirectionalGraph<T, QuickGraphEdge>(bidirectionalGraph);
        }

        public IEnumerable<IEdge<T>> PathBetween(T source, T target)
        {
            IEnumerable<IEdge<T>> path;
            this.TryGetPathBetweet(source, target, out path);

            return path;
        }

        public bool TryGetPathBetweet(T source, T target, out IEnumerable<IEdge<T>> path)
        {
            var algorithm = new FloydWarshallAllShortestPathAlgorithm<T, QuickGraphEdge>(this.graph, edge => 1);
            
            algorithm.Compute();


            IEnumerable<QuickGraphEdge> qgPath;
            var hasPath = algorithm.TryGetPath(source, target, out qgPath);

            path = (qgPath ?? Enumerable.Empty<QuickGraphEdge>()).Select(edge => edge.WrappedEdge);
            return hasPath;
        }
    }
}