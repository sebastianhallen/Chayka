namespace Chayka.Tests.PathFinder
{
    using System.Collections.Generic;
    using System.Linq;
    using Chayka.GraphBuilder;
    using Chayka.PathFinder;
    using NUnit.Framework;

    public abstract class PathTest
    {
        protected abstract PathType PathType { get; }
        protected static string PathToString<T>(IEnumerable<IEdge<IVertex<T>>> path)
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

            IEnumerable<IEdge<IVertex<char>>> _;
            var isValidPath = graph.TryGetPathBetween('a', 'a', out _);

            Assert.That(isValidPath);
        }

        [Test]
        public void Should_return_empty_path_getting_path_to_sef()
        {
            var graph = ExampleGraphs.UniDirectedLinear.CreatePathFinder(this.PathType);

            IEnumerable<IEdge<IVertex<char>>> path;
            graph.TryGetPathBetween('a', 'a', out path);

            Assert.That(path.Any(), Is.False);
        }

        [Test]
        public void Should_return_false_when_unable_to_find_a_path()
        {
            var pathFinder = ExampleGraphs.BiDirectional4X4Mesh.AddVertex('ö').CreatePathFinder(this.PathType);

            IEnumerable<IEdge<IVertex<char>>> _;
            var pathFound = pathFinder.TryGetPathBetween('m', 'ö', out _);

            Assert.That(pathFound, Is.False);
        }

        [Test]
        public void PathBetween_should_throw_exception_when_unable_to_find_a_path()
        {
            var pathFinder = ExampleGraphs.BiDirectional4X4Mesh.AddVertex('ö').CreatePathFinder(this.PathType);

            Assert.Throws<NoPathFoundException>(() => pathFinder.PathBetween('m', 'ö'));
        }
    }
}