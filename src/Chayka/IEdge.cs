﻿namespace Chayka
{
    public interface IEdge<out T>
    {
        T Source { get; }
        T Target { get; }
    }
}