namespace Chayka.Visualization.Wpf
{
    using System;
    using QuickGraph.Serialization;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Xml;

    internal interface IVisualizationUpdateCommand
    {
        Command Command { get; }
        object Content { get; }
    }

    internal abstract class VisualizationUpdateCommand
        : IVisualizationUpdateCommand
    {
        public abstract Command Command { get; }
        public object Content { get; private set; }
        protected VisualizationUpdateCommand(object content)
        {
            this.Content = content;
        }
    }

    internal class SetGraphCommand
        : VisualizationUpdateCommand
    {
        public override Command Command { get { return Command.SetGraph; } }

        public SetGraphCommand(object content) : base(content) { }
    }

    internal class SetActiveVertexCommand
        : VisualizationUpdateCommand
    {
        public override Command Command { get { return Command.SetActiveVertex; } }

        public SetActiveVertexCommand(object content) : base(content) { }
    }

    internal enum Command
    {
        SetGraph,
        SetActiveVertex
    }

    internal class GraphVisualizationServer
    {
        private readonly Socket listener;
        public const string EndOfCommandMarker = "|EndOfCommand|";
        public const string EndOfContentMarker = "|EndOfContent|";
        
        public const int Port = 11001;

        public GraphVisualizationServer()
        {
            var localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);

            this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.listener.Bind(localEndPoint);
            this.listener.Listen(10);
        }

        public IVisualizationUpdateCommand GetCommand()
        {
            var handler = this.listener.Accept();
            string data = null;

            while (true)
            {
                var bytes = new byte[1024];
                var bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                var endOfContentPosition = data.IndexOf(EndOfContentMarker, System.StringComparison.InvariantCulture);
                if (endOfContentPosition > -1)
                {
                    break;
                }
            }

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();


            var commandMarkerPosition = data.IndexOf(EndOfCommandMarker, StringComparison.InvariantCulture);
            var rawCommand = data.Remove(commandMarkerPosition);
            data = data.Remove(0, commandMarkerPosition + EndOfCommandMarker.Length);
            
            var contentMarkerPosition = data.IndexOf(EndOfContentMarker, StringComparison.InvariantCulture);
            var content = data.Substring(0, contentMarkerPosition);

            Command command;
            return Enum.TryParse(rawCommand, out command) 
                ? this.CreateCommand(command, content) 
                : null;
        }

        private IVisualizationUpdateCommand CreateCommand(Command command, string content)
        {
            if (command.Equals(Command.SetGraph))
            {
                return new SetGraphCommand(DeserializeGraph(content));
            }

            if (command.Equals(Command.SetActiveVertex))
            {
                return new SetActiveVertexCommand(new VisualizationVertex(content));
            }

            throw new Exception("Unable to handle command: " + command);
        }

        private static VisualizationGraph DeserializeGraph(string rawGraph)
        {
            using (var stringReader = new StringReader(rawGraph))
            using (var xmlreader = XmlReader.Create(stringReader))
            {
                return xmlreader.DeserializeFromXml(
                    "graph",
                    "vertex",
                    "edge",
                    "",
                    r => new VisualizationGraph(),
                    r => new VisualizationVertex(r.GetAttribute("id")),
                    r => new VisualizationEdge(new VisualizationVertex(r.GetAttribute("source")), new VisualizationVertex(r.GetAttribute("target")))
                    );
            }
        }
    }
}