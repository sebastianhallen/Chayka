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
                Send(Command.SetGraph, graphContent);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to set graph: " + e);
            }
        }

        public static void SetActiveVertex<T>(T vertex)
        {
            try
            {
                Send(Command.SetActiveVertex, vertex.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to set active vertex: " + e);
            }
        }

        private static void Send(Command command, string content)
        {

            var remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), GraphVisualizationServer.Port);
            var sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(remoteEndPoint);

            var msg = Encoding.ASCII.GetBytes(
                command + GraphVisualizationServer.EndOfCommandMarker +
                content + GraphVisualizationServer.EndOfContentMarker);

            sender.Send(msg);
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
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