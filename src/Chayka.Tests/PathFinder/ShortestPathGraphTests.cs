namespace Chayka.Tests.PathFinder
{
    using System.Collections.Generic;
    using Chayka.PathFinder;
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class ShortestPathGraphTests
        : PathTest
    {
        protected override PathType PathType
        {
            get { return PathType.Shortest; }
        }

        [Test]
        public void Should_be_able_to_find_shortest_path_in_graph()
        {
            var graph = ExampleGraphs.BiDirectionalPyramid.CreatePathFinder(PathType.Shortest);

            var path = graph.PathBetween(5, 6).ToArray();

            Assert.That(PathToString(path), Is.EqualTo("5 -> 4 -> 6"));
        }

        [Test]
        public void Should_return_empty_path_when_unable_to_find_a_path()
        {
            var graph = ExampleGraphs.UniDirectedLinear.CreatePathFinder(PathType.Shortest);

            IEnumerable<IEdge<IVertex<char>>> path;
            graph.TryGetPathBetween('c', 'b', out path);

            Assert.That(path.Any(), Is.False);
        }
    }
}
