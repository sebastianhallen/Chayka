namespace Chayka
{
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
}