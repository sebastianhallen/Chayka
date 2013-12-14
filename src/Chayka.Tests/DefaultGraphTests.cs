namespace Chayka.Tests
{
    using System;
    using System.Linq;
    using Chayka.PathFinder;
    using NUnit.Framework;

    [TestFixture]
    public class DefaultGraphTests
    {
        [Test]
        public void Should_explode_when_unable_to_create_path_finder()
        {
            var vertices = Enumerable.Empty<IVertex<int>>();
            var edges = Enumerable.Empty<IEdge<IVertex<int>>>();
            var graph = new DefaultGraph<int>(null, vertices, edges);

            var exception = Assert.Throws<Exception>(() => graph.CreatePathFinder((PathType) (-1)));

            Assert.That(exception.Message, Is.EqualTo("Unhandled path type: -1"));
        }
    }
}