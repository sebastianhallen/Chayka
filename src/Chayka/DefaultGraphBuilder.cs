namespace Chayka
{
    using System.Collections.Generic;

    public class DefaultGraphBuilder<T>
        : IGraphBuilder<T>
    {
        private readonly List<IVertex<T>> vertices;
        private readonly List<IEdge<T>> edges;

        public DefaultGraphBuilder()
        {
            this.vertices = new List<IVertex<T>>();
            this.edges = new List<IEdge<T>>();
        }

        public IGraphBuilder<T> AddVertex(IVertex<T> vertex)
        {
            this.vertices.Add(vertex);
            return this;
        }

        public IGraphBuilder<T> AddEdge(IEdge<T> edge)
        {
            this.edges.Add(edge);
            return this;
        }

        public IGraph<T> BuildGraph()
        {
            return new QuickGraphGraph<T>(this.vertices, this.edges);
        }
    }
}