namespace Chayka.GraphBuilder
{
    using Chayka.PathFinder;

    public interface IGraphBuilder<T>
    {
        IGraphBuilder<T> AddVertex(IVertex<T> vertex);
        IGraphBuilder<T> AddEdge(IEdge<IVertex<T>> edge);

        IPathFinder<T> CreatePathFinder(PathType pathType);
    }
}
