namespace Chayka
{
    using System;
    using System.Collections.Generic;

    public class DefaultVertex<T>
        : IVertex<T>
    {
        private readonly Action onEntry;

        public DefaultVertex(T content, Action onEntry)
        {
            this.onEntry = onEntry;
            this.Content = content;
        }

        public T Content { get; private set; }

        public void OnEntry()
        {
            this.onEntry();
        }

        protected bool Equals(DefaultVertex<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Content, other.Content);
        }

        public override string ToString()
        {
            return this.Content == null ? "()" : this.Content.ToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DefaultVertex<T>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Content);
        }
    }
}