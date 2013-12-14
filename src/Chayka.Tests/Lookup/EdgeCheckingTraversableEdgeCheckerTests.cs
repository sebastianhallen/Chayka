namespace Chayka.Tests.Lookup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Chayka.Lookup;
    using FakeItEasy;
    using NUnit.Framework;

    [TestFixture]
    public class EdgeCheckingTraversableEdgeCheckerTests
    {
        [Test]
        public void Should_query_edge_for_traversability()
        {
            var edge = A.Fake<IEdge<IVertex<int>>>();
            var edgeChecker = new EdgeCheckingTraverseableEdgeChecker<int>();

            edgeChecker.IsTraverseable(edge);

            A.CallTo(() => edge.IsWalkable()).MustHaveHappened();
        }
    }
}
