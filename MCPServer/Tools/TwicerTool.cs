using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MCPServer.Tools;

[McpServerToolType]
public static class TwicerTool
{
    [McpServerTool, Description(ToolDescription.Twicer)]
    public static int TwiceIt(int currentNumber)
    {
        return currentNumber * 2;
    }
}