namespace Chayka
{
    public interface IVertex<out T>
    {
        T Content { get; }
        void OnEntry();
    }
}