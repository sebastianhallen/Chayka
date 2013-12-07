namespace Chayka
{
    using System.Collections.Generic;

    public class DefaultEdge<T>
        : IEdge<T>
    {
        public T Source { get; private set; }
        public T Target { get; private set; }
        public double Weight { get; private set; }

        public DefaultEdge(T source, T target, double weight = 1)
        {
            this.Source = source;
            this.Target = target;
            this.Weight = weight;
        }

        protected bool Equals(DefaultEdge<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Source, other.Source)
                && EqualityComparer<T>.Default.Equals(Target, other.Target)
                && Weight.Equals(other.Weight);
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
                int hashCode = EqualityComparer<T>.Default.GetHashCode(Source);
                hashCode = (hashCode*397) ^ EqualityComparer<T>.Default.GetHashCode(Target);
                hashCode = (hashCode*397) ^ Weight.GetHashCode();
                return hashCode;
            }
        }
    }
}