using OllamaSharp;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;

IChatClient chatClient = new ChatClientBuilder(
    new OllamaApiClient(new Uri("http://localhost:11434/"),"llama3.2:latest"))
    .UseFunctionInvocation()
    .Build();

string projectPath = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName 
                     ?? throw new InvalidOperationException("Could not determine project directory.");

string serverPath = Path.Combine(projectPath, "MCPServer");

// Verify the server path exists
if (!Directory.Exists(serverPath))
{
    throw new DirectoryNotFoundException($"MCPServer directory not found at: {serverPath}");
}

//Create a new MCP client
IMcpClient mcpClient = await McpClientFactory.CreateAsync(
    new StdioClientTransport(new()
    {
        Command = "dotnet",
        Arguments = [ "run", "--project", serverPath ],
        WorkingDirectory = serverPath, // Set working directory to the server path
        Name = "Minimal MCP Server"
    }));

// List all available tools from the MCP Server
IList<McpClientTool> tools = await mcpClient.ListToolsAsync();
foreach (McpClientTool tool in tools)
{
    Console.WriteLine($"Tool: {tool.Name} - {tool.Description}");
}
Console.WriteLine();

// Conversational loop that can utilize the tools via prompts
List<ChatMessage> chatHistory = new();
while (true)
{
    // Get User input
    AcceptPrompt:
    Console.WriteLine("\nQuery:");
    var userPrompt = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(userPrompt))
    {
        goto AcceptPrompt;
    }
    
    chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

    if (userPrompt == "/clear")
    {
        chatHistory.Clear();
        Console.WriteLine("\n Chat history has been cleared.");
        goto AcceptPrompt;
    }

    // Stream the AI response and add to chat history
    Console.WriteLine("\nBot:");
    var response = "";
    await foreach(ChatResponseUpdate item in 
        chatClient.GetStreamingResponseAsync(chatHistory, new() { Tools = [..tools] }))
        {
            Console.Write(item.Text);
            response += item.Text;
        }
    chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
    Console.WriteLine("\n\n");
}