namespace Chayka
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using QuickGraph;

    public class NoPathFoundException
        : Exception
    {
        public NoPathFoundException(string message)
            : base(message)
        {
        }
    }

    public abstract class QuickGraphGraph<T>
        : IPathFinder<T>
    {
        protected readonly IBidirectionalGraph<T, QuickGraph.IEdge<T>> Graph;

        protected QuickGraphGraph(IEnumerable<IVertex<T>> vertices, IEnumerable<IEdge<T>> edges)
        {
            var bidirectionalGraph = new BidirectionalGraph<T, QuickGraph.IEdge<T>>();
            
            bidirectionalGraph.AddVertexRange(vertices.Select(vertex => vertex.Content));
            bidirectionalGraph.AddEdgeRange(edges.Select(edge => new QuickGraphEdge(edge)));

            this.Graph = new ArrayBidirectionalGraph<T, QuickGraph.IEdge<T>>(bidirectionalGraph);
        }

        public abstract bool TryGetPathBetween(T source, T target, out IEnumerable<IEdge<T>> path);

        public IEnumerable<IEdge<T>> PathBetween(T source, T target)
        {
            IEnumerable<IEdge<T>> path;
            if (!this.TryGetPathBetween(source, target, out path))
            {
                throw new NoPathFoundException(string.Format("Unable to find a path between {0} and {1}", source, target));
            }

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