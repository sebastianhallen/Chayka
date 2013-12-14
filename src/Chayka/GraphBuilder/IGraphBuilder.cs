namespace Chayka.GraphBuilder
{
    public interface IGraphBuilder<T>
    {
        IGraphBuilder<T> AddVertex(IVertex<T> vertex);
        IGraphBuilder<T> AddEdge(IEdge<IVertex<T>> edge);

        IGraph<T> Build();
    }
}
