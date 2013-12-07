namespace Chayka.Tests.PathFinder
{
    using System.Collections.Generic;
    using Chayka.PathFinder;
    using NUnit.Framework;

    [TestFixture]
    public class LongestPathFinderTests
        : PathTest
    {
        protected override PathType PathType
        {
            get { return PathType.Longest; }
        }

        [Test]
        public void Should_find_longest_path()
        {
            var pathFinder = ExampleGraphs.BiDirectionalPyramid.CreatePathFinder(PathType.Longest);

            var path = pathFinder.PathBetween(7, 2);
            
            Assert.That(PathToString(path), Is.EqualTo("7 -> 6 -> 4 -> 0 -> 1 -> 2"));
        }

        [Test]
        public void Should_not_visit_the_same_node_twice()
        {
            var pathFinder = ExampleGraphs.BiDirectionalPyramid.CreatePathFinder(PathType.Longest);

            var path = pathFinder.PathBetween(8, 7);
            
            Assert.That(PathToString(path), Is.EqualTo("8 -> 6 -> 7"));
        }

        [Test]
        public void Should_use_shortest_path_when_not_able_to_find_a_long_path_without_node_revisit()
        {
            var pathFinder = ExampleGraphs.BiDirectionalPyramid.CreatePathFinder(PathType.Longest);

            var path = pathFinder.PathBetween(8, 6);
            
            Assert.That(PathToString(path), Is.EqualTo("8 -> 6"));
        }

        [Test] //or should we try to find the longest round trip path?
        public void Should_give_empty_path_when_finding_longest_path_to_self()
        {
            var pathFinder = ExampleGraphs.UniDirectedSquare.CreatePathFinder(PathType.Longest);

            IEnumerable<IEdge<IVertex<char>>> path;
            pathFinder.TryGetPathBetween('a', 'a', out path);

            Assert.That(PathToString(path), Is.EqualTo("()"));
        }
    }
}