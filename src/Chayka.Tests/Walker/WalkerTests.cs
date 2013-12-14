namespace Chayka.Tests.Walker
{
    using Chayka.Lookup;
    using Chayka.PathFinder;
    using Chayka.PathFinder.RandomWalk;
    using Chayka.Walker;
    using FakeItEasy;
    using NUnit.Framework;

    public abstract class WalkerTests
    {
        [UnderTest] protected DefaultGraphWalker<int> walker;
        [Fake] protected IPathFinder<int> pathFinder;
        [Fake] protected IGraph<int> graph;
        [Fake] protected IVertexFinder<int> vertexFinder;
        [Fake] protected IEdgeFinder<int> edgeFinder;
        [Fake] protected IRandomizer randomizer;

        [SetUp]
        public void Before()
        {
            Fake.InitializeFixture(this);
            A.CallTo(() => this.graph.CreatePathFinder(A<PathType>._)).Returns(this.pathFinder);
        }
    }
}
