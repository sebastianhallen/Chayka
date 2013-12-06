namespace Chayka.Tests
{
    using System.Linq;

    public class ExampleGraphs
    {
        /*
                0
               / \
              1   4
             /   / \
            2---5   6
           /       / \
          3-------7   8
        */

        private static bool _sessionFactoryTainted;
        private static IRandomWalkSessionFactory _randomWalkSessionFactoryField; 
        private static IRandomWalkSessionFactory RandomWalkSessionFactory
        {
            get
            {
                var factory = _randomWalkSessionFactoryField ?? (_randomWalkSessionFactoryField = new DefaultRandomWalkSessionFactory(new DefaultRandomizer(1337), 1000));

                if (_sessionFactoryTainted)
                {
                    _randomWalkSessionFactoryField = null;
                    _sessionFactoryTainted = false;
                }
                return factory;
            }
        }

        public static ExampleGraphs OverrideNext(IRandomWalkSessionFactory sessionFactory)
        {
            _randomWalkSessionFactoryField = sessionFactory;
            _sessionFactoryTainted = true;

            return new ExampleGraphs();
        }

        public static IGraphBuilder<int> BiDirectionalPyramid
        {
            get
            {
                return new DefaultGraphBuilder<int>(RandomWalkSessionFactory)
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
        }

        /*
            a---b---c---d
            |   |   |   |
            e---f---g---h
            |   |   |   |
            i---j---k---l
            |   |   |   |
            m---n---o---p
          
        */
        public static IGraphBuilder<char> BiDirectional4X4
        {
            get
            {
                return new DefaultGraphBuilder<char>(RandomWalkSessionFactory)
                    .Vertices("abcdefghijklmnop".Select(c => c).ToArray())
                    .Bi('a', 'b').Bi('b', 'c').Bi('c', 'd')
                    .Bi('e', 'f').Bi('f', 'g').Bi('g', 'h')
                    .Bi('i', 'j').Bi('j', 'k').Bi('k', 'l')
                    .Bi('m', 'n').Bi('n', 'o').Bi('o', 'p')

                    .Bi('a', 'e').Bi('e', 'i').Bi('i', 'm')
                    .Bi('b', 'f').Bi('f', 'j').Bi('j', 'n')
                    .Bi('c', 'g').Bi('g', 'k').Bi('k', 'o')
                    .Bi('d', 'h').Bi('h', 'l').Bi('l', 'p');
            }
        }

        /*
            a---b---c---d
            | X | X | X |
            e---f---g---h
            | X | X | X |
            i---j---k---l
            | X | X | X |
            m---n---o---p
          
        */
        public static IGraphBuilder<char> BiDirectional4X4Mesh
        {
            get
            {
                return BiDirectional4X4
                    .Bi('a', 'f').Bi('b', 'g').Bi('c', 'h')
                    .Bi('e', 'j').Bi('f', 'k').Bi('g', 'l')
                    .Bi('i', 'n').Bi('j', 'o').Bi('k', 'p')

                    .Bi('d', 'e').Bi('c', 'f').Bi('d', 'g')
                    .Bi('f', 'i').Bi('g', 'h').Bi('h', 'k')
                    .Bi('j', 'm').Bi('k', 'n').Bi('l', 'o');
            }
        }

        /*
            a--->b
            ∧    |
            |    ∨
            c<---d
        */
        public static IGraphBuilder<char> UniDirectedSquare
        {
            get
            {
                return new DefaultGraphBuilder<char>(RandomWalkSessionFactory)
                    .AddVertex('a').AddVertex('b').AddVertex('c').AddVertex('d')
                    .AddEdge('a', 'b').AddEdge('b', 'd').AddEdge('d', 'c').AddEdge('c', 'a');

            }
        }

        /*
            a-->b-->c
        */
        public static IGraphBuilder<char> UniDirectedLinear
        {
            get
            {
                return new DefaultGraphBuilder<char>(RandomWalkSessionFactory)
                                .AddVertex('a')
                                .AddVertex('b')
                                .AddVertex('c')
                                .AddEdge('a', 'b')
                                .AddEdge('b', 'c');
            }
        }
    }

    internal static class ExampleGraphBuilderExtensions
    {
        public static IGraphBuilder<T> Vertices<T>(this IGraphBuilder<T> builder, params T[] vertices)
        {
            foreach (var vertex in vertices)
            {
                builder.AddVertex(vertex);
            }

            return builder;
        }

        public static IGraphBuilder<T> Uni<T>(this IGraphBuilder<T> builder, T source, T target)
        {
            return builder.AddEdge(source, target);
        }

        public static IGraphBuilder<T> Bi<T>(this IGraphBuilder<T> builder, T a, T b)
        {
            return builder.Uni(a, b).Uni(b, a);
        }
    }
}