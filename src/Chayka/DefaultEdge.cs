namespace Chayka
{
    using System.Collections.Generic;

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

        protected bool Equals(DefaultEdge<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Source, other.Source) && EqualityComparer<T>.Default.Equals(Target, other.Target);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DefaultEdge<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(Source)*397) ^ EqualityComparer<T>.Default.GetHashCode(Target);
            }
        }
    }
}