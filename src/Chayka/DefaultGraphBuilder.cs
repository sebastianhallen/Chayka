namespace Chayka
{
    using System.Collections.Generic;

    public static class DefaultGraphBuilderExtensions
    {
        public static IGraphBuilder<T> AddVertex<T>(this IGraphBuilder<T> builder, T vertex)
        {
            return builder.AddVertex(new DefaultVertex<T>(vertex));
        }

        public static IGraphBuilder<T> AddEdge<T>(this IGraphBuilder<T> builder, T source, T target)
        {
            return builder.AddEdge(new DefaultEdge<T>(source, target));
        }
    }

    public class DefaultGraphBuilder<T>
        : IGraphBuilder<T>
    {
        private readonly List<IVertex<T>> vertices;
        private readonly List<IEdge<T>> edges;

        public DefaultGraphBuilder()
        {
            this.vertices = new List<IVertex<T>>();
            this.edges = new List<IEdge<T>>();
        }

        public IGraphBuilder<T> AddVertex(IVertex<T> vertex)
        {
            this.vertices.Add(vertex);
            return this;
        }

        public IGraphBuilder<T> AddEdge(IEdge<T> edge)
        {
            this.edges.Add(edge);
            return this;
        }

        public IPathFinder<T> CreatePathFinder(PathType pathType)
        {
            if (pathType.Equals(PathType.Shortest)) return new ShortestPathGraph<T>(this.vertices, this.edges);
            if (pathType.Equals(PathType.Longest)) return new LongestPathGraph<T>(this.vertices, this.edges);
            return null;
        }
    }
}