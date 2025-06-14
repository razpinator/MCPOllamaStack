using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MCPServer.Tools;

[McpServerToolType]
public static class EchoTool
{
    [McpServerTool, Description(ToolDescription.Echo)]
    public static string Echo(string message) => $"Hello from C#: {message}";

    [McpServerTool, Description(ToolDescription.ReverseEcho)]
    public static string ReverseEcho(string message)
    {
        return new string(message.Reverse().ToArray());
    }
}