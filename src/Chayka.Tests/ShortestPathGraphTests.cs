namespace Chayka.Tests
{
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class ShortestPathGraphTests
        : PathTest
    {
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

            var pathCtoB = graph.PathBetween('c', 'b');

            Assert.That(pathCtoB.Any(), Is.False);
        }
    }
}
