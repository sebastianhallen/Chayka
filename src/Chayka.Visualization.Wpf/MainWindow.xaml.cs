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
            this.worker.DoWork += (sender, args) => args.Result = this.graphVisualizationServer.GetGraph();
            this.worker.RunWorkerCompleted += (sender, args) =>
                {
                    var graph = (VisualizationGraph)args.Result;
                    this.viewModel.Graph = graph;
                    this.worker.RunWorkerAsync();
                };
                
            this.DataContext = this.viewModel;

            InitializeComponent();

            this.worker.RunWorkerAsync();

            var sugiyamaLayoutParameters = this.GraphLayout.LayoutParameters as EfficientSugiyamaLayoutParameters;
            if (sugiyamaLayoutParameters != null)
            {
                sugiyamaLayoutParameters.EdgeRouting = SugiyamaEdgeRoutings.Orthogonal;
                sugiyamaLayoutParameters.MinimizeEdgeLength = false;
                sugiyamaLayoutParameters.OptimizeWidth = true;
                sugiyamaLayoutParameters.VertexDistance = 100;
                sugiyamaLayoutParameters.LayerDistance = 100;
                //sugiyamaLayoutParameters.PositionMode = 0;
            }

            var kkLayoutParameters = this.GraphLayout.LayoutParameters as KKLayoutParameters;
            if (kkLayoutParameters != null)
            {
                kkLayoutParameters.AdjustForGravity = false;
                
                
            }
        }
    }
}
