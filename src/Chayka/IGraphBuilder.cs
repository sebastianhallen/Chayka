namespace Chayka
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using QuickGraph.Algorithms.Observers;

    public interface IGraphBuilder<T>
    {
        IGraphBuilder<T> AddVertex(IVertex<T> vertex);
        IGraphBuilder<T> AddEdge(IEdge<T> edge);
        IGraph<T> BuildGraph();
    }
}
