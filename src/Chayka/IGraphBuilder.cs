namespace Chayka
{
    public interface IGraphBuilder<T>
    {
        IGraphBuilder<T> AddVertex(IVertex<T> vertex);
        IGraphBuilder<T> AddEdge(IEdge<T> edge);

        IPathFinder<T> CreatePathFinder(PathType pathType);
    }
}
