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
        private string layoutAlgorithmTypeField;
        public string LayoutAlgorithmType
        {
            get { return this.layoutAlgorithmTypeField; }
            set
            {
                this.layoutAlgorithmTypeField = value;
                this.OnPropertyChanged("LayoutAlgorithmType");
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
            //this.LayoutAlgorithmType = this.LayoutAlgorithms.Skip(3).First();
            //this.LayoutAlgorithmType = "EfficientSugiyama";
            this.LayoutAlgorithmType = "KK";

            //this.worker = new BackgroundWorker();
            //this.worker.DoWork += (sender, args) =>
            //    {
            //        System.Threading.Thread.Sleep(5000);
            //        var skip = (int)args.Argument;

            //        this.LayoutAlgorithmType = this.LayoutAlgorithms.Skip(skip).First();

            //        if (++skip >= this.LayoutAlgorithms.Count()) skip = 1;
            //        args.Result = skip;


            //    };
            //this.worker.RunWorkerCompleted += (sender, args) => this.worker.RunWorkerAsync(args.Result);
            //this.worker.RunWorkerAsync(0);
        }

        private BackgroundWorker worker;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
