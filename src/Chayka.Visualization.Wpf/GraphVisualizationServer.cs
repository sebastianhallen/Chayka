namespace Chayka.Visualization.Wpf
{
    using QuickGraph.Serialization;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Xml;

    internal class GraphVisualizationServer
    {
        private readonly Socket listener;
        public const string EndOfMessageMarker = "<EOF>";
        public const int Port = 11001;

        public GraphVisualizationServer()
        {
            var localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);

            this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.listener.Bind(localEndPoint);
            this.listener.Listen(10);
        }

        public VisualizationGraph GetGraph()
        {
            var handler = this.listener.Accept();
            string data = null;

            while (true)
            {
                var bytes = new byte[1024];
                var bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                var endOfMesssagePosition = data.IndexOf(EndOfMessageMarker, System.StringComparison.InvariantCulture);
                if (endOfMesssagePosition > -1)
                {
                    data = data.Remove(endOfMesssagePosition);
                    break;
                }
            }

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();

            return Deserialize(data);
        }

        private static VisualizationGraph Deserialize(string rawGraph)
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