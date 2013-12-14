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
                .AddVertex(State.Search)
                .AddVertex(State.SingleItem)
                .AddVertex(State.SearchResult)
                .AddVertex(State.Favorites)
                .AddVertex(State.FavoriteItem)

                .AddEdge(State.SingleItem, State.Search, _ =>
                    {
                        Console.WriteLine("opening search");
                        this.app.OpenSearch();
                    })
                .AddEdge(State.SingleItem, State.FavoriteItem, _ =>
                    {
                        Console.WriteLine("adding to favorites");
                        this.app.AddItemToFavorites();
                    }, () => this.app.IsCurrentItemFavorited)
                .AddEdge(State.SingleItem, State.Favorites, _ =>
                    {
                        Console.WriteLine("opening favorites");
                        this.app.AddItemToFavorites();
                    })
                .AddEdge(State.SearchResult, State.SingleItem, _ =>
                    {
                        Console.WriteLine("opening item");
                        //STATE DEPENDENT
                        var wantedItem = (from item in this.app.CurrentSearchResults
                                          orderby randomizer.NextInt(int.MaxValue)
                                         select item).First();
                        this.app.OpenItem(wantedItem);
                    })
                .AddEdge(State.SearchResult, State.Favorites, _ =>
                    {
                        Console.WriteLine("opening favorites");
                        this.app.OpenFavorites();
                    })
                .AddEdge(State.SearchResult, State.Search, _ =>
                    {
                        Console.WriteLine("opening search");
                        this.app.OpenSearch();
                    })
                .AddEdge(State.Search, State.SearchResult, _ =>
                    {
                        Console.WriteLine("performing search");
                        //STATE DEPENDENT
                        var query = (from term in this.app.Queries
                                     orderby randomizer.NextInt(int.MaxValue)
                                     select term).First();
                        this.app.PerformSearch(query);
                    })
                .AddEdge(State.Search, State.Favorites, _ =>
                    {
                        Console.WriteLine("opening favorites");
                        this.app.OpenFavorites();
                    })
                .AddEdge(State.Favorites, State.Search, _ =>
                    {
                        Console.WriteLine("opening search");
                        this.app.OpenSearch();
                    })
                .AddEdge(State.Favorites, State.FavoriteItem, _ =>
                    {
                        Console.WriteLine("opening favorite item");
                        //STATE DEPENDENT
                        var item = (from favorite in this.app.Favorites
                                    orderby randomizer.NextInt(int.MaxValue)
                                    select favorite).First();
                        this.app.OpenFavoriteItem(item);
                    }, () => this.app.HasFavorites)
                .AddEdge(State.FavoriteItem, State.Favorites, _ =>
                    {
                        Console.WriteLine("unfavoriting item");
                        this.app.RemoteItemFromFavorites();
                    })
                .AddEdge(State.FavoriteItem, State.Favorites, _ =>
                    {
                        Console.WriteLine("opening favorites");
                        this.app.OpenFavorites();
                    })
                .AddEdge(State.FavoriteItem, State.Search, _ =>
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
        [Explicit]
        public void Should_be_able_to_walk_between_search_and_favorite_item()
        {
            this.walker.WalkBetween(State.Search, State.FavoriteItem, PathType.Shortest);
        }

        [Test]
        public void Should_be_able_to_do_a_random_walk_with_a_fixed_number_of_steps()
        {
            this.walker.RandomWalk(State.Search, 100);
        }
    }
}
