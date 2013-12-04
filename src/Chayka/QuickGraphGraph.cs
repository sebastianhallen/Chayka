namespace Chayka
{
    using System.Collections.Generic;
    using System.Linq;
    using QuickGraph;

    public abstract class QuickGraphGraph<T>
        : IPathFinder<T>
    {
        protected readonly IVertexAndEdgeListGraph<T, QuickGraphEdge> Graph;

        protected QuickGraphGraph(IEnumerable<IVertex<T>> vertices, IEnumerable<IEdge<T>> edges)
        {
            var bidirectionalGraph = new BidirectionalGraph<T, QuickGraphEdge>();
            
            bidirectionalGraph.AddVertexRange(vertices.Select(vertex => vertex.Content));
            bidirectionalGraph.AddEdgeRange(edges.Select(edge => new QuickGraphEdge(edge)));

            this.Graph = new ArrayBidirectionalGraph<T, QuickGraphEdge>(bidirectionalGraph);
        }

        public abstract bool TryGetPathBetween(T source, T target, out IEnumerable<IEdge<T>> path);

        public IEnumerable<IEdge<T>> PathBetween(T source, T target)
        {
            IEnumerable<IEdge<T>> path;
            this.TryGetPathBetween(source, target, out path);

            return path;
        }

        protected class QuickGraphEdge
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
    }
}