namespace Chayka.Visualization.Wpf
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Xml;
    using QuickGraph.Algorithms;
    using QuickGraph.Serialization;

    public class GraphVisualization
    {
        public static void SetGraph<T>(IGraph<T> graph)
        {
            try
            {
                var graphContent = Serialize(graph);
                var ipHostInfo = Dns.Resolve(Dns.GetHostName());
                var ipAddress = ipHostInfo.AddressList[0];
                var remoteEndPoint = new IPEndPoint(ipAddress, GraphVisualizationServer.Port);

                var sender = new Socket(AddressFamily.InterNetwork,
                                        SocketType.Stream, ProtocolType.Tcp);

                sender.Connect(remoteEndPoint);

                var msg = Encoding.ASCII.GetBytes(graphContent + GraphVisualizationServer.EndOfMessageMarker);

                sender.Send(msg);

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to set graph: " + e);
            }
        }

        private static string Serialize<T>(IGraph<T> graph)
        {
            var visualizationGraph = graph.ToVisualizationGraph();
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
            {
                visualizationGraph.SerializeToXml(
                    writer,
                    v => v.Label,
                    visualizationGraph.GetEdgeIdentity(),
                    "graph",
                    "vertex",
                    "edge",
                    "");
            }

            return sb.ToString();
        }
    }
}