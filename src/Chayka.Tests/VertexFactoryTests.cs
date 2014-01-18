namespace Chayka.Tests
{
    using System;
    using System.Linq;
    using Chayka.GraphBuilder;
    using NUnit.Framework;

    [TestFixture]
    public class VertexFactoryTests
    {
        [Test]
        public void Should_be_able_to_use_custom_vertex_types_when_building_graph()
        {
            var builder = new DefaultGraphBuilder<int>();
            VertexFactory.VertexBuilder = new TestVertexBuilder();

            builder.AddVertex(100);

            var vertex = builder.Vertices.Single();
            Assert.That(vertex, Is.TypeOf<TestVertex<int>>());
        }

        [Test]
        public void Should_revert_to_default_factory_when_setting_vertex_factory_to_null()
        {
            var builder = new DefaultGraphBuilder<int>();
            VertexFactory.VertexBuilder = new TestVertexBuilder();
            VertexFactory.VertexBuilder = null;

            builder.AddVertex(100);

            var vertex = builder.Vertices.Single();
            Assert.That(vertex, Is.TypeOf<DefaultVertex<int>>());
        }

        [Test]
        public void Should_revert_to_default_factory_when_resetting_vertex_factory()
        {
            var builder = new DefaultGraphBuilder<int>();
            VertexFactory.VertexBuilder = new TestVertexBuilder();
            VertexFactory.Reset();

            builder.AddVertex(100);

            var vertex = builder.Vertices.Single();
            Assert.That(vertex, Is.TypeOf<DefaultVertex<int>>());
        }

        [TearDown]
        public void AfterEach()
        {
           VertexFactory.VertexBuilder =null;
        }

        private class TestVertexBuilder
            : IVertexBuilder
        {
            public IVertex<T> Build<T>(T content, Action onEntry)
            {
                return new TestVertex<T>();
            }
        }


        private class TestVertex<T>
            : IVertex<T>
        {
            public T Content { get; private set; }
            public void OnEntry()
            {
                
            }
        }
    }
}