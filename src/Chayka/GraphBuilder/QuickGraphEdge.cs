namespace Chayka.GraphBuilder
{
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