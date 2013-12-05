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
        public void Should_be_able_to_get_a_path_between_nodes()
        {
            var pathFinder = ExampleGraphs.BiDirectional4X4Mesh.CreatePathFinder(PathType.Random);

            var path = pathFinder.PathBetween('m', 'd');

            Console.WriteLine(PathToString(path));
            Assert.That(path.First().Source, Is.EqualTo('m'));
            Assert.That(path.Last().Target, Is.EqualTo('d'));
        }
    }
}