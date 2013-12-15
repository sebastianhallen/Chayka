namespace Chayka.Visualization.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using System.ComponentModel;
    using GraphSharp.Algorithms.Layout.Simple.FDP;
    using GraphSharp.Algorithms.Layout.Simple.Hierarchical;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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

            var sugiyamaLayoutParameters = this.graphLayout.LayoutParameters as EfficientSugiyamaLayoutParameters;
            if (sugiyamaLayoutParameters != null)
            {
                sugiyamaLayoutParameters.EdgeRouting = SugiyamaEdgeRoutings.Orthogonal;
                sugiyamaLayoutParameters.MinimizeEdgeLength = true;
                sugiyamaLayoutParameters.VertexDistance = 10;
            }

            var kkLayoutParameters = this.graphLayout.LayoutParameters as KKLayoutParameters;
            if (kkLayoutParameters != null)
            {
                kkLayoutParameters.AdjustForGravity = false;
                
            }
        }
    }
}
