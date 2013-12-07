namespace Chayka.PathFinder
{
    using System;

    public class NoPathFoundException
        : Exception
    {
        public NoPathFoundException(string message)
            : base(message)
        {
        }
    }
}