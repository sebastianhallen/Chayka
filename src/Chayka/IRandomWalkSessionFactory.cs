namespace Chayka
{
    using QuickGraph;

    public interface IRandomWalkSessionFactory
    {
        IRandomWalkSession<T> Start<T>(IBidirectionalGraph<T, QuickGraph.IEdge<T>> graph);
    }
}