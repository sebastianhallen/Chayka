namespace Chayka.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Chayka.GraphBuilder;
    using Chayka.Lookup;
    using Chayka.PathFinder;
    using Chayka.PathFinder.RandomWalk;
    using Chayka.Tests.ExampleApp;
    using Chayka.Walker;
    using NUnit.Framework;
    using Chayka.Visualization.Wpf;

    [TestFixture]
    public class UsageTest
    {
        private ISearchAndFavoriteActions app;
        private IGraph<State> model;
        private IGraphWalker<State> walker;
        

        private enum State
        {
            Search,
            SingleItem,
            SearchResult,
            Favorites,
            FavoriteItem
        }

        [SetUp]
        public void Before()
        {
            var randomizer = new DefaultRandomizer(1337);
            this.app = new SearchAndFavoriteActions();
            this.model = new DefaultGraphBuilder<State>()
                .AddVertex(State.Search, () => Console.WriteLine("In Search"))
                .AddVertex(State.SingleItem, () => Console.WriteLine("In SingleItem"))
                .AddVertex(State.SearchResult, () => Console.WriteLine("In SearchResult"))
                .AddVertex(State.Favorites, () => Console.WriteLine("In Favorites"))
                .AddVertex(State.FavoriteItem, () => Console.WriteLine("In FavoriteItem"))

                .AddEdge(State.SingleItem, State.Search, () =>
                    {
                        Console.WriteLine("opening search");
                        this.app.OpenSearch();
                    })
                .AddEdge(State.SingleItem, State.FavoriteItem, () =>
                    {
                        Console.WriteLine("adding to favorites");
                        this.app.AddItemToFavorites();
                    }, () => !this.app.IsCurrentItemFavorited)
                .AddEdge(State.SingleItem, State.Favorites, () =>
                    {
                        Console.WriteLine("opening favorites");
                        this.app.OpenFavorites();
                    })
                .AddEdge(State.SearchResult, State.SingleItem, () =>
                    {
                        Console.WriteLine("opening item");
                        var wantedItem = (from item in this.app.CurrentSearchResults
                                          orderby randomizer.NextInt(int.MaxValue)
                                          select item).First();
                        this.app.OpenItem(wantedItem);
                    })
                .AddEdge(State.SearchResult, State.Favorites, () =>
                    {
                        Console.WriteLine("opening favorites");
                        this.app.OpenFavorites();
                    })
                .AddEdge(State.SearchResult, State.Search, () =>
                    {
                        Console.WriteLine("opening search");
                        this.app.OpenSearch();
                    })
                .AddEdge(State.Search, State.SearchResult, () =>
                    {
                        Console.WriteLine("performing search");
                        var query = (from term in this.app.Queries
                                     orderby randomizer.NextInt(int.MaxValue)
                                     select term).First();
                        this.app.PerformSearch(query);
                    })
                .AddEdge(State.Search, State.Favorites, () =>
                    {
                        Console.WriteLine("opening favorites");
                        this.app.OpenFavorites();
                    })
                .AddEdge(State.Favorites, State.Search, () =>
                    {
                        Console.WriteLine("opening search");
                        this.app.OpenSearch();
                    })
                .AddEdge(State.Favorites, State.FavoriteItem, () =>
                    {
                        Console.WriteLine("opening favorite item");
                        //requires favorited items: (State.SingleItem) -> (State.FavoriteItem)
                        var item = (from favorite in this.app.Favorites
                                    orderby randomizer.NextInt(int.MaxValue)
                                    select favorite).First();
                        this.app.OpenFavoriteItem(item);
                    }, () => this.app.HasFavorites)
                .AddEdge(State.FavoriteItem, State.Favorites, () =>
                    {
                        Console.WriteLine("unfavoriting item");
                        this.app.RemoteItemFromFavorites();
                    })
                .AddEdge(State.FavoriteItem, State.Favorites, () =>
                    {
                        Console.WriteLine("opening favorites");
                        this.app.OpenFavorites();
                    })
                .AddEdge(State.FavoriteItem, State.Search, () =>
                    {
                        Console.WriteLine("opening search");
                        this.app.OpenSearch();
                    })
                
                .Build();

            var vertexFinder = new DefaultVertexFinder<State>();
            var edgeFinder = new DefaultEdgeFinder<State>();
            var edgeChecker = new EdgeCheckingTraverseableEdgeChecker<State>();
            
            this.walker = new DefaultGraphWalker<State>(this.model, vertexFinder, edgeFinder, randomizer, edgeChecker);   
        }

        [Test]
        public void Should_be_able_to_walk_between_search_and_favorite_item()
        {
            //note that shortest path does not work here with the current offline path builder
            this.walker.WalkBetween(State.Search, State.FavoriteItem, PathType.Longest);
        }

        [Test]
        public void Should_be_able_to_do_a_random_walk_with_a_fixed_number_of_steps()
        {
            this.walker.RandomWalk(State.Search, 1000);
        }

        [Test, Explicit]
        public void VisualizationRotation()
        {
            GraphVisualization.SetGraph(this.model);
            System.Threading.Thread.Sleep(5000);
            GraphVisualization.SetGraph(ExampleGraphs.BiDirectional4X4Mesh);
            System.Threading.Thread.Sleep(5000);
            GraphVisualization.SetGraph(ExampleGraphs.BiDirectional4X4);
            System.Threading.Thread.Sleep(5000);
            GraphVisualization.SetGraph(ExampleGraphs.BiDirectionalPyramid);
            System.Threading.Thread.Sleep(5000);
            GraphVisualization.SetGraph(ExampleGraphs.UniDirectedLinear);
            System.Threading.Thread.Sleep(5000);
            GraphVisualization.SetGraph(ExampleGraphs.UniDirectedSquare);
            System.Threading.Thread.Sleep(5000);
            GraphVisualization.SetGraph(ExampleGraphs.WeightedBiDirectional);
        }
    }
}
