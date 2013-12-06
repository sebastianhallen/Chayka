namespace Chayka
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using QuickGraph;

    public interface IRandomWalkSessionFactory
    {
        IRandomWalkSession<T> Start<T>(IBidirectionalGraph<T, QuickGraph.IEdge<T>> graph);
    }

    public interface IRandomWalkSession<T>
    {
        bool TryGetNextEdge(T from, out QuickGraph.IEdge<T> edge);
    }

    public class DefaultRandomWalkSessionFactory
        : IRandomWalkSessionFactory
    {
        public IRandomWalkSession<T> Start<T>(IBidirectionalGraph<T, QuickGraph.IEdge<T>> graph)
        {
            return new DefaultRandomWalkSession<T>(graph, 1000);
        }
    }

    public class DefaultRandomWalkSession<T>
        : IRandomWalkSession<T>
    {
        private readonly IBidirectionalGraph<T, QuickGraph.IEdge<T>> graph;
        private readonly int maxPathLength;
        private readonly Random randomizer;
        private int currentPathLength;
        private const int MaxNumberOfOrderedEdges = 1 << 10;

        public DefaultRandomWalkSession(IBidirectionalGraph<T, QuickGraph.IEdge<T>> graph, int maxPathLength)
        {
            this.graph = graph;
            this.maxPathLength = maxPathLength;
            this.currentPathLength = 0;
            this.randomizer = new Random();
        }

        public bool TryGetNextEdge(T @from, out QuickGraph.IEdge<T> edge)
        {
            if (++this.currentPathLength >= this.maxPathLength)
            {
                edge = null;
                return false;
            }
            edge = (from e in this.graph.Edges
                             where e.Source.Equals(@from)
                             orderby this.NextRandom(MaxNumberOfOrderedEdges)
                             select e).FirstOrDefault();
            return edge != null;
        }

        private int NextRandom(int max)
        {
            return this.randomizer.Next(max);
        }
    }

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
            if (pathType.Equals(PathType.Random)) return new RandomPathGraph<T>(new DefaultRandomWalkSessionFactory(), this.vertices, this.edges);
            return null;
        }
    }
}