namespace Chayka.Visualization.Wpf
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using GraphSharp.Algorithms.Layout;

    internal class MainWindowViewModel
        : INotifyPropertyChanged
    {
        private string layoutAlgorithmField;
        public string LayoutAlgorithm
        {
            get { return this.layoutAlgorithmField; }
            set
            {
                this.layoutAlgorithmField = value;
                this.OnPropertyChanged("LayoutAlgorithm");
            }
        }

        public IEnumerable<string> LayoutAlgorithms
        {
            get
            {
                return new []
                    {
                        "BoundedFR",
                        "Circular",
                        "CompoundFDP",
                        "EfficientSugiyama",
                        "FR",
                        "ISOM",
                        "KK",
                        "LinLog",
                        "Tree",
                    };
            }
        }

        private VisualizationGraph graphField;
        public VisualizationGraph Graph
        {
            get { return this.graphField; }
            set
            {
                this.graphField = value;
                this.OnPropertyChanged("Graph");
            }
        }

        public MainWindowViewModel()
        {
            //this.LayoutAlgorithm = this.LayoutAlgorithms.Skip(3).First();
            //this.LayoutAlgorithm = "EfficientSugiyama";
            this.LayoutAlgorithm = "KK";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
