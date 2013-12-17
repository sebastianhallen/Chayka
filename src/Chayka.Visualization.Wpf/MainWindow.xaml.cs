namespace Chayka.Visualization.Wpf
{
    using System.Windows;
    using GraphSharp.Algorithms.Layout.Simple.FDP;
    using GraphSharp.Algorithms.Layout.Simple.Hierarchical;
    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
        : Window
    {
        private readonly MainWindowViewModel viewModel;
        private readonly BackgroundWorker worker;
        private readonly GraphVisualizationServer graphVisualizationServer;

        public MainWindow()
        {
            this.viewModel = new MainWindowViewModel();
            this.graphVisualizationServer = new GraphVisualizationServer();
            this.worker = new BackgroundWorker();
            this.worker.DoWork += (sender, args) => args.Result = this.graphVisualizationServer.GetCommand();
            this.worker.RunWorkerCompleted += (sender, args) =>
                {
                    var command = args.Result as IVisualizationUpdateCommand;
                    if (command is SetGraphCommand)
                    {
                        var graph = (VisualizationGraph)command.Content;
                        this.ConfigureLayout();
                        this.viewModel.Graph = graph;
                    }

                    if (command is SetActiveVertexCommand)
                    {
                        var vertex = (VisualizationVertex)command.Content;
                        this.GraphLayout.HighlightAlgorithm.ResetHighlight();
                        this.GraphLayout.HighlightVertex(vertex, "None");
                    }


                    this.worker.RunWorkerAsync();
                };
                
            this.DataContext = this.viewModel;

            InitializeComponent();

            this.worker.RunWorkerAsync();

            
        }

        private void ConfigureLayout()
        {
            var sugiyamaLayoutParameters = this.GraphLayout.LayoutParameters as EfficientSugiyamaLayoutParameters;
            if (sugiyamaLayoutParameters != null)
            {
                sugiyamaLayoutParameters.EdgeRouting = SugiyamaEdgeRoutings.Orthogonal;
                sugiyamaLayoutParameters.MinimizeEdgeLength = true;
                sugiyamaLayoutParameters.OptimizeWidth = true;
                sugiyamaLayoutParameters.VertexDistance = 30;
                sugiyamaLayoutParameters.LayerDistance = 30;
                //sugiyamaLayoutParameters.PositionMode = -1;
            }

            var kkLayoutParameters = this.GraphLayout.LayoutParameters as KKLayoutParameters;
            if (kkLayoutParameters != null)
            {
                kkLayoutParameters.AdjustForGravity = false;


            }
        }
    }
}
