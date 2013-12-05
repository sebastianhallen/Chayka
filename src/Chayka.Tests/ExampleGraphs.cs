namespace Chayka.Tests
{
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
        public static IGraphBuilder<int> BiDirectionalPyramid
        {
            get
            {
                return new DefaultGraphBuilder<int>()
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
            a--->b
            ∧    |
            |    ∨
            c<---d
        */
        public static IGraphBuilder<char> UniDirectedSquare
        {
            get
            {
                return new DefaultGraphBuilder<char>()
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
                return new DefaultGraphBuilder<char>()
                                .AddVertex('a')
                                .AddVertex('b')
                                .AddVertex('c')
                                .AddEdge('a', 'b')
                                .AddEdge('b', 'c');
            }
        }
    }
}