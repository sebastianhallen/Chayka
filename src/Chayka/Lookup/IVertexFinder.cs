namespace Chayka.Lookup
{
    public interface IVertexFinder<T>
    {
        IVertex<T> Find(IGraph<T> graph, T vertex);
    }
}