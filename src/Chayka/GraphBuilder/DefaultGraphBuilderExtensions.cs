namespace Chayka.GraphBuilder
{
    public static class DefaultGraphBuilderExtensions
    {
        public static IGraphBuilder<T> AddVertex<T>(this IGraphBuilder<T> builder, T vertex)
        {
            return builder.AddVertex(new DefaultVertex<T>(vertex));
        }

        public static IGraphBuilder<T> AddEdge<T>(this IGraphBuilder<T> builder, T source, T target, int weight = 1)
        {
            return builder.AddEdge(new DefaultEdge<IVertex<T>>(new DefaultVertex<T>(source), new DefaultVertex<T>(target), weight));
        }
    }
}