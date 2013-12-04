namespace Chayka
{
    public class DefaultVertex<T>
        : IVertex<T>
    {
        public T Content { get; private set; }

        public DefaultVertex(T content)
        {
            this.Content = content;
        }
    }
}