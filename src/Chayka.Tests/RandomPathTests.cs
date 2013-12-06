namespace Chayka.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class RandomPathTests
        : PathTest
    {
        protected override PathType PathType
        {
            get { return PathType.Random; }
        }

        [TestCase(1337, "m -> i -> f -> i -> n -> j -> k -> h -> d")]
        [TestCase(9000, "m -> n -> i -> e -> j -> e -> i -> e -> a -> e -> a -> f -> e -> d")]
        public void Should_produce_repatable_path_when_using_a_known_random_input(int seed, string expectedPath)
        {
            ExampleGraphs.OverrideNext(new DefaultRandomWalkSessionFactory(new DefaultRandomizer(seed), 1000));
            var pathFinder = ExampleGraphs.BiDirectional4X4Mesh.CreatePathFinder(PathType.Random);

            var path = pathFinder.PathBetween('m', 'd').ToArray();

            Assert.That(PathToString(path), Is.EqualTo(expectedPath));
        }

        [Test]
        public void Should_stop_when_random_walk_reaches_target()
        {
            ExampleGraphs.OverrideNext(new DefaultRandomWalkSessionFactory(new DefaultRandomizer(1337), 1000));
            var pathFinder = ExampleGraphs.BiDirectional4X4Mesh.CreatePathFinder(PathType.Random);

            var path = pathFinder.PathBetween('m', 'd').ToArray();

            Console.WriteLine(PathToString(path));
            Assert.That(path.First().Source, Is.EqualTo('m'));
            Assert.That(path.Last().Target, Is.EqualTo('d'));
        }

        [Test]
        public void PathBetween_should_throw_exception_when_unable_to_find_a_path_with_random_walk()
        {
            ExampleGraphs.OverrideNext(new DefaultRandomWalkSessionFactory(new DefaultRandomizer(1337), 7));
            var pathFinder = ExampleGraphs.BiDirectional4X4Mesh.CreatePathFinder(PathType.Random);

            Assert.Throws<NoPathFoundException>(() => pathFinder.PathBetween('m', 'd'));
        }
    }
}