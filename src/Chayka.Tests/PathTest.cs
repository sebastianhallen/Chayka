namespace Chayka.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    public abstract class PathTest
    {
        protected abstract PathType PathType { get; }
        protected static string PathToString<T>(IEnumerable<IEdge<T>> path)
        {
            var pathArray = path.ToArray();

            if (!pathArray.Any()) return "()";

            return
                pathArray.First().Source + " -> " +
                string.Join(" -> ", pathArray.Select(edge => edge.Target));
        }

        [Test]
        public void Path_to_self_should_be_valid()
        {
            var graph = ExampleGraphs.UniDirectedLinear.CreatePathFinder(this.PathType);

            IEnumerable<IEdge<char>> _;
            var isValidPath = graph.TryGetPathBetween('a', 'a', out _);

            Assert.That(isValidPath);
        }

        [Test]
        public void Should_return_empty_path_getting_path_to_sef()
        {
            var graph = ExampleGraphs.UniDirectedLinear.CreatePathFinder(PathType.Shortest);

            IEnumerable<IEdge<char>> path;
            graph.TryGetPathBetween('a', 'a', out path);

            Assert.That(path.Any(), Is.False);
        }
    }
}