namespace Chayka
{
    using QuickGraph;

    public class DefaultRandomWalkSessionFactory
        : IRandomWalkSessionFactory
    {
        public IRandomWalkSession<T> Start<T>(IBidirectionalGraph<T, QuickGraph.IEdge<T>> graph)
        {
            return new DefaultRandomWalkSession<T>(graph, 1000);
        }
    }
}