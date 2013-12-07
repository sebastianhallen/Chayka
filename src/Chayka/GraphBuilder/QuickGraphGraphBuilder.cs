namespace Chayka.GraphBuilder
{
    using System.Collections.Generic;
    using System.Linq;
    using QuickGraph;

    public class QuickGraphGraphBuilder<T>
    {
        public static IBidirectionalGraph<IVertex<T>, QuickGraphEdge<T>> Build(IEnumerable<IVertex<T>> vertices,
                                                                        IEnumerable<Chayka.IEdge<IVertex<T>>> edges)
        {
            var bidirectionalGraph = new BidirectionalGraph<IVertex<T>, QuickGraphEdge<T>>();
            
            bidirectionalGraph.AddVertexRange(vertices);
            bidirectionalGraph.AddEdgeRange(edges.Select(edge => new QuickGraphEdge<T>(edge)));

            return new ArrayBidirectionalGraph<IVertex<T>, QuickGraphEdge<T>>(bidirectionalGraph);
        }
    }

    public class QuickGraphEdge<T>
            : QuickGraph.IEdge<IVertex<T>>
    {
        public IVertex<T> Source
        {
            get { return this.WrappedEdge.Source; }
        }
        public IVertex<T> Target
        {
            get { return this.WrappedEdge.Target; }
        }
        public Chayka.IEdge<IVertex<T>> WrappedEdge { get; private set; }

        public double Weight
        {
            get { return this.WrappedEdge.Weight; }
        }

        public QuickGraphEdge(Chayka.IEdge<IVertex<T>> edge)
        {
            this.WrappedEdge = edge;
        }
    }
}