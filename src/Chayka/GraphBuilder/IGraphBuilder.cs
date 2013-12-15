namespace Chayka.GraphBuilder
{
    using System.Collections.Generic;

    public interface IGraphBuilder<T>
    {
        IEnumerable<IVertex<T>> Vertices { get; } 
 
        IGraphBuilder<T> AddVertex(IVertex<T> vertex);
        IGraphBuilder<T> AddEdge(IEdge<IVertex<T>> edge);

        IGraph<T> Build();
    }
}
