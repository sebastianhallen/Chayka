namespace Chayka.GraphBuilder
{
    using System;
    using System.Linq;

    public static class GraphBuilderExtensions
    {
        public static IGraphBuilder<T> AddVertex<T>(this IGraphBuilder<T> builder, T content, Action onEntry = null)
        {
            var vertex = VertexFactory.Create(content, onEntry ?? (() => { }));
            return builder.AddVertex(vertex);
        }

        public static IGraphBuilder<T> AddEdge<T>(this IGraphBuilder<T> builder, T source, T target, Action onTraverse, Func<bool> isWalkable = null, double weight = 1)
        {
            var sourceVertex = builder.Vertices.SingleOrDefault(v => Equals(v.Content, source));
            var targetVertex = builder.Vertices.SingleOrDefault(v => Equals(v.Content, target));
            if (sourceVertex == null || targetVertex == null)
            {
                throw GraphConstructionException.AddingEdgeBeforeVertex;
            }

            return builder.AddEdge(
                new DefaultEdge<IVertex<T>>(
                    sourceVertex,
                    targetVertex, 
                    onTraverse, 
                    isWalkable ?? (() => true), 
                    weight
                )
            );
        }
    }
}