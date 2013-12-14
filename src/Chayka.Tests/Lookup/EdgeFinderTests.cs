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
        [Test]
        public void Should_find_all_matching_edges_when_doing_lookup_by_source_vertex()
        {
            var edgeFinder = new DefaultEdgeFinder<int>();
            var graph = A.Fake<IGraph<int>>();
            IVertex<int> matchingVertex = new DefaultVertex<int>(123);
            IVertex<int> nonMatchingVertex = new DefaultVertex<int>(321);
            IEdge<IVertex<int>> e0 = new DefaultEdge<IVertex<int>>(matchingVertex, null, _ => { });
            IEdge<IVertex<int>> e1 = new DefaultEdge<IVertex<int>>(matchingVertex, null, _ => { });
            IEdge<IVertex<int>> e2 = new DefaultEdge<IVertex<int>>(nonMatchingVertex, null, _ => { });
            
            A.CallTo(() => graph.Edges).Returns(new[] {e0, e1, e2});

            var edges = edgeFinder.FindEgesFrom(graph, matchingVertex);

            Assert.That(edges, Is.EquivalentTo(new [] { e0, e1 }));
        }
    }
}
