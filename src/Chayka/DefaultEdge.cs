namespace Chayka
{
    public class DefaultEdge<T>
        : IEdge<T>
    {
        public T Source { get; private set; }
        public T Target { get; private set; }

        public DefaultEdge(T source, T target)
        {
            this.Source = source;
            this.Target = target;
        }
    }
}