# MCP Ollama Stack

A Model Context Protocol (MCP) chat application that integrates Ollama with a custom MCP server to provide AI-powered conversations with tool integration capabilities.

## Project Structure

```
MCPOllamaStack/
├── MCPServer/          # MCP Server application
│   └── Program.cs      # Server implementation
└── MCPClient/          # MCP Client application  
    └── Program.cs      # Client implementation with chat interface
```

## Overview

This project consists of two main components:

- **MCP Server**: A web application that exposes tools via the Model Context Protocol using stdio transport
- **MCP Client**: A console chat application that connects to both Ollama and the MCP server to provide an interactive AI experience with tool capabilities

## Prerequisites

### Required Software

1. **.NET SDK** (version 8.0 or later)
2. **Ollama** - Local AI model server
   - Download from [ollama.ai](https://ollama.ai)
   - Must be running on `http://localhost:11434`

### Required Model

The project is configured to use the `llama3.2:latest` model. Ensure you have this model installed:

```bash
ollama pull llama3.2:latest
```

## Setup Instructions

### 1. Start Ollama

Make sure Ollama is running with the required model:

```bash
# Start Ollama (if not already running)
ollama serve

# Verify the model is available
ollama list
```

### 2. Run the Project

Navigate to the client directory and run the application:

```bash
cd MCPClient
dotnet run
```

The client will automatically:
- Start the MCP server from the parent directory
- Connect to Ollama at `http://localhost:11434`
- Initialize the chat interface
- Display available tools from the MCP server

## Usage

### Chat Interface

Once the application starts, you'll see:
1. A list of available tools from the MCP server
2. A prompt asking for your query

### Available Commands

- **Regular chat**: Type any message to interact with the AI
- **Clear history**: Type `/clear` to reset the conversation history
- **Exit**: Press `Ctrl+C` to stop the application

### Example Session

```
Tool: example-tool - Example tool description

Query:
Hello, what can you help me with?

Bot:
I'm an AI assistant that can help you with various tasks. I have access to tools from the MCP server that can extend my capabilities...
```

## Features

- **Streaming responses**: Real-time AI response streaming for better user experience
- **Tool integration**: Automatic discovery and integration of MCP server tools
- **Conversation history**: Maintains chat context throughout the session
- **Error handling**: Robust error handling for missing directories and connection issues

## Architecture

The application uses:
- **OllamaSharp**: For connecting to the Ollama API
- **Microsoft.Extensions.AI**: For AI chat client abstraction
- **ModelContextProtocol.Client**: For MCP server communication
- **Stdio transport**: For communication between client and server

## Troubleshooting

### Common Issues

1. **"MCPServer directory not found"**
   - Ensure the project structure matches the expected layout
   - Verify you're running from the correct directory

2. **Ollama connection failed**
   - Confirm Ollama is running: `ollama list`
   - Check if the service is accessible at `http://localhost:11434`

3. **Model not found**
   - Pull the required model: `ollama pull llama3.2:latest`

4. **Tools not loading**
   - Check MCP server logs for any startup errors
   - Ensure the server has proper tool implementations

### Logs and Debugging

The MCP server includes console logging with trace-level output directed to stderr for debugging purposes.

## Development

### Adding New Tools

To add new tools to the MCP server:
1. Create tool classes in the MCPServer project
2. Ensure they're discoverable via `WithToolsFromAssembly()`
3. Restart the client to reload tools

### Modifying the Model

To use a different Ollama model, update the model name in the client's `Program.cs`:

```csharp
new OllamaApiClient(new Uri("http://localhost:11434/"), "your-model-name")
```

## License

This project is provided as-is for educational and development purposes.