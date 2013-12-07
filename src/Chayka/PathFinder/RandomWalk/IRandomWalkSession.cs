namespace Chayka.PathFinder.RandomWalk
{
    public interface IRandomWalkSession<T>
    {
        bool TryGetNextEdge(IVertex<T> from, out IEdge<IVertex<T>> edge);
    }
}