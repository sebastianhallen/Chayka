namespace Chayka.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class RandomPathTests
        : PathTest
    {
        [Test]
        public void Should_try_shortest_path_between_nodes_when_random_walk_fails_to_reach_target()
        {
            var pathFinder = ExampleGraphs.BiDirectional4X4Mesh.CreatePathFinder(PathType.Random);

            var path = pathFinder.PathBetween('m', 'd');

            Console.WriteLine(PathToString(path));
            Assert.That(path.First().Source, Is.EqualTo('m'));
            Assert.That(path.Last().Target, Is.EqualTo('d'));
        }
    }
}