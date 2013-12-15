namespace Chayka.Tests
{
    using System;
    using System.Linq;
    using Chayka.GraphBuilder;
    using Chayka.PathFinder.RandomWalk;

    public class ExampleGraphs
    {
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

        public static void OverrideNext(IRandomWalkSessionFactory sessionFactory)
        {
            _randomWalkSessionFactoryField = sessionFactory;
            _sessionFactoryTainted = true;
        }


/*
        (a)-- 1 --(e)
         |         |
         2         1
         |         |
        (b)       (d)
          \       /
           2     1
            \   /
             (c)
  
*/
        public static IGraph<char> WeightedBiDirectional
        {
            get { return WeightedBiDirectionalBuilder.Build(); }
        }

        private static IGraphBuilder<char> WeightedBiDirectionalBuilder
        {
            get
            {
                return new DefaultGraphBuilder<char>(RandomWalkSessionFactory)
                    .Vertices('a', 'b', 'c', 'd', 'e')
                    .Bi('a', 'b', 2).Bi('b', 'c', 2).Bi('c', 'd', 1).Bi('d', 'e', 1).Bi('e', 'a', 1);
            }
        }

/*
                (0)
               /   \
             (1)   (4)
             /     / \
           (2)--(5)  (6)
           /         / \
         (3)<------(7) (8)
*/
        public static IGraph<int> BiDirectionalPyramid
        {
            get { return BiDirectionalPyramidBuilder.Build(); }
        }
        private static IGraphBuilder<int> BiDirectionalPyramidBuilder
        {
            get
            {
                return new DefaultGraphBuilder<int>(RandomWalkSessionFactory)
                                    .Vertices(Enumerable.Range(0, 9).ToArray())
                                    .Bi(0, 1).Bi(0, 4)
                                    .Bi(1, 2).Bi(2, 5).Bi(4, 5).Bi(4, 6)
                                    .Bi(2, 3).Uni(7, 3).Bi(6, 7).Bi(6, 8);
            }
        }

/*
            (a)---(b)---(c)---(d)
             |     |     |     |
            (e)---(f)---(g)---(h)
             |     |     |     |
            (i)---(j)---(k)---(l)
             |     |     |     |
            (m)---(n)---(o)---(p)
          
*/
        public static IGraph<char> BiDirectional4X4
        {
            get { return BiDirectional4X4Builder.Build(); }
        }

        private static IGraphBuilder<char> BiDirectional4X4Builder
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
                    (a)---(b)---(c)---(d)
                     |  X  |  X  |  X  |
                    (e)---(f)---(g)---(h)
                     |  X  |  X  |  X  |
                    (i)---(j)---(k)---(l)
                     |  X  |  X  |  X  |
                    (m)---(n)---(o)---(p)
        */
        public static IGraph<char> BiDirectional4X4Mesh
        {
            get { return BiDirectional4X4MeshBuilder.Build(); }
        }

        public static IGraphBuilder<char> BiDirectional4X4MeshBuilder
        {
            get
            {
                return BiDirectional4X4Builder
                    .Bi('a', 'f').Bi('b', 'g').Bi('c', 'h')
                    .Bi('e', 'j').Bi('f', 'k').Bi('g', 'l')
                    .Bi('i', 'n').Bi('j', 'o').Bi('k', 'p')

                    .Bi('d', 'e').Bi('c', 'f').Bi('d', 'g')
                    .Bi('f', 'i').Bi('g', 'h').Bi('h', 'k')
                    .Bi('j', 'm').Bi('k', 'n').Bi('l', 'o');
            }
        }

/*
            (a)-->(b)
             ∧    |
             |    ∨
            (c)<--(d)
*/
        public static IGraph<char> UniDirectedSquare
        {
            get { return UniDirectedSquareBuilder.Build(); }
        }

        private static IGraphBuilder<char> UniDirectedSquareBuilder
        {
            get
            {
                return new DefaultGraphBuilder<char>(RandomWalkSessionFactory)
                    .Vertices('a', 'b', 'c','d')
                    .Uni('a', 'b').Uni('b', 'd').Uni('d', 'c').Uni('c', 'a');

            }
        }

/*
            (a)-->(b)-->(c)
*/
        public static IGraph<char> UniDirectedLinear 
        {
            get { return UniDirectedLinearBuilder.Build(); }
        }

        private static IGraphBuilder<char> UniDirectedLinearBuilder
        {
            get
            {
                return new DefaultGraphBuilder<char>(RandomWalkSessionFactory)
                                .Vertices('a', 'b', 'c')
                                .Uni('a', 'b')
                                .Uni('b', 'c');
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

        public static IGraphBuilder<T> Uni<T>(this IGraphBuilder<T> builder, T source, T target, double weight = 1)
        {
            return builder.AddEdge(source, target, (() => { }), weight: weight);
        }

        public static IGraphBuilder<T> Bi<T>(this IGraphBuilder<T> builder, T a, T b, double weight = 1)
        {
            return builder.Uni(a, b, weight).Uni(b, a, weight);
        }
    }
}