namespace Chayka
{
    public interface IRandomWalkSession<T>
    {
        bool TryGetNextEdge(T from, out QuickGraph.IEdge<T> edge);
    }
}