namespace Chayka.PathFinder.RandomWalk
{
    using System;

    public class DefaultRandomizer
        : IRandomizer
    {
        private readonly Random random;

        public DefaultRandomizer()
            : this(Environment.TickCount)
        {
        }

        public DefaultRandomizer(int seed)
        {
            this.random = new Random(seed);
        }

        public int NextInt(int max)
        {
            return this.random.Next(max);
        }
    }
}