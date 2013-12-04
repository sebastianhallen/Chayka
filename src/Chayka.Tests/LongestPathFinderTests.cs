namespace Chayka.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class LongestPathFinderTests
    {
        private DefaultGraphBuilder<int> graphBuilder;

        /*
                0
               / \
              1   4
             /   / \
            2---5   6
           /       / \
          3-------7   8
         
        */


        [SetUp]
        public void Before()
        {
            this.graphBuilder = new DefaultGraphBuilder<int>();
            this.graphBuilder
                .AddVertex(0).AddVertex(1).AddVertex(2)
                .AddVertex(3).AddVertex(4).AddVertex(5)
                .AddVertex(6).AddVertex(7).AddVertex(8)
                .AddEdge(0, 1).AddEdge(1, 0)
                .AddEdge(1, 2).AddEdge(2, 1)
                .AddEdge(2, 3).AddEdge(2, 5).AddEdge(3, 2)
                .AddEdge(2, 5)

                .AddEdge(0, 4).AddEdge(4, 0)
                .AddEdge(4, 5).AddEdge(5, 4).AddEdge(4, 6).AddEdge(6, 4)
                .AddEdge(5, 2)
                .AddEdge(6, 7).AddEdge(6, 8).AddEdge(7, 6).AddEdge(8, 6)
                .AddEdge(7, 3);
        }

        [Test]
        public void Should_find_longest_path()
        {
            var pathFinder = this.graphBuilder.CreatePathFinder(PathType.Longest);

            var path = pathFinder.PathBetween(7, 2);
            var p = PathToString(path);

            Assert.That(p, Is.EqualTo("7 -> 6 -> 4 -> 0 -> 1 -> 2"));
        }

        [Test]
        public void Should_use_shortest_path_when_not_able_to_find_a_long_path_without_node_revisit()
        {
            var pathFinder = this.graphBuilder.CreatePathFinder(PathType.Longest);

            var path = pathFinder.PathBetween(8, 7);
            var p = PathToString(path);

            Assert.That(p, Is.EqualTo("8 -> 6 -> 7"));
        }

        private static string PathToString(IEnumerable<IEdge<int>> path)
        {
            var pathArray = path.ToArray();

            if (!pathArray.Any()) return "()";

            return
                pathArray.First().Source + " -> " +
                string.Join(" -> ", pathArray.Select(edge => edge.Target));
        }
    }
}