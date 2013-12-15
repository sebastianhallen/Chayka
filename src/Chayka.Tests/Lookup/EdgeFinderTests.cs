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
    public class EdgeFinderTests
    {
        private DefaultEdgeFinder<int> edgeFinder;
        private IVertex<int> matchingVertex;
        private IVertex<int> nonMatchingVertex;
        private IEdge<IVertex<int>> e0;
        private IEdge<IVertex<int>> e1;
        private IEdge<IVertex<int>> e2;
        private IGraph<int> graph;
        private ITraverseableEdgeChecker<int> edgeChecker;

        [SetUp]
        public void Before()
        {
            this.edgeFinder = new DefaultEdgeFinder<int>();
            this.graph = A.Fake<IGraph<int>>();
            this.matchingVertex = new DefaultVertex<int>(123, (() => { }));
            this.nonMatchingVertex = new DefaultVertex<int>(321, (() => { }));
            this.e0 = new DefaultEdge<IVertex<int>>(matchingVertex, null, () => { });
            this.e1 = new DefaultEdge<IVertex<int>>(matchingVertex, null, () => { });
            this.e2 = new DefaultEdge<IVertex<int>>(nonMatchingVertex, null, () => { });
            this.edgeChecker = A.Fake<ITraverseableEdgeChecker<int>>();

            A.CallTo(() => this.graph.Edges).Returns(new[] { this.e0, this.e1, this.e2 });
        }

        [Test]
        public void Should_find_all_matching_edges_when_doing_lookup_by_source_vertex()
        {
            var edges = this.edgeFinder.FindEgesFrom(this.graph, this.matchingVertex, this.edgeChecker);

            Assert.That(edges, Is.EquivalentTo(new [] { this.e0, this.e1 }));
        }

        [Test]
        public void Should_check_if_edge_is_traverseable_before_including_edge_in_matching_edges()
        {
            this.edgeFinder.FindEgesFrom(this.graph, this.matchingVertex, this.edgeChecker).ToArray();

            A.CallTo(() => this.edgeChecker.IsTraverseable(this.e0)).MustHaveHappened();
            A.CallTo(() => this.edgeChecker.IsTraverseable(this.e1)).MustHaveHappened();
            A.CallTo(() => this.edgeChecker.IsTraverseable(this.e2)).MustNotHaveHappened();
        }
    }
}
