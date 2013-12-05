namespace Chayka.Tests
{
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class ShortestPathGraphTests
    {
        [Test]
        public void Should_be_able_to_find_shortest_path_in_graph()
        {
            var graph = ExampleGraphs.BiDirectionalPyramid.CreatePathFinder(PathType.Shortest);

            var pathAtoC = graph.PathBetween(5, 6).ToArray();

            Assert.That(pathAtoC.First().Source, Is.EqualTo(5));
            Assert.That(pathAtoC.First().Target, Is.EqualTo(4));
            Assert.That(pathAtoC.Last().Source, Is.EqualTo(4));
            Assert.That(pathAtoC.Last().Target, Is.EqualTo(6));
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
