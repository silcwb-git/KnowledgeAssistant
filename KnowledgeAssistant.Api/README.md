# 🤖 KnowledgeAssistant

A production-ready **RAG (Retrieval-Augmented Generation)** API built with **.NET Core** and integrated with the **GPT Maker** agent platform. It answers questions using your own knowledge base — not just the model's memory.

## ✨ Features

- 🔍 **RAG pipeline** — retrieves relevant snippets from a knowledge base, then generates answers grounded in that context
- 🧠 **Real AI integration** — talks to a GPT Maker agent via `HttpClient` + Bearer token
- 🛠️ **Clean architecture** — Dependency Injection, interfaces (`IAiClient`, `IKnowledgeSearch`), Options pattern
- 🧪 **Demo fallback mode** — runs without credentials, returning retrieved snippets so you can test the flow instantly
- 📚 **Swagger UI** — interactive API documentation at `/swagger`
- 🔐 **Secure by default** — API key stored via .NET user-secrets, never committed to the repo

## 🏗️ Architecture
Api/
├── Models/          → Request/response DTOs (ChatRequest, ChatResponse, KnowledgeItem)
├── Options/         → AiOptions (BaseUrl, ApiKey, AgentId) via Options pattern
├── Services/        → Business logic
│   ├── InMemoryKnowledgeStore.cs   → Knowledge base (in-memory)
│   ├── IKnowledgeSearch.cs         → Retrieval interface
│   ├── SimpleKnowledgeSearch.cs    → Keyword-based retrieval (tokenization + scoring)
│   ├── IAiClient.cs                → AI provider interface
│   ├── GptMakerAiClient.cs         → GPT Maker HTTP client
│   └── ChatService.cs              → Orchestrates: question → retrieve → generate
└── Controllers/     → REST endpoints
├── ChatController.cs           → POST /api/chat
└── KnowledgeItemsController.cs → CRUD for knowledge items


## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A [GPT Maker](https://gptmaker.ai) account with an agent + API token

### 1. Clone & run
```bash
git clone https://github.com/your-user/KnowledgeAssistant.git
cd KnowledgeAssistant/KnowledgeAssistant.

Api
dotnet run
```
### 2. Configure your AI credentials (user-secrets)
```bash
dotnet user-secrets init
dotnet user-secrets set "Ai:ApiKey" "YOUR_GPTMAKER_TOKEN"
dotnet user-secrets set "Ai:AgentId" "YOUR_AGENT_ID"
dotnet user-secrets set "Ai:BaseUrl" "https://api.gptmaker.ai"
```
🔒 Security: credentials live in user-secrets, outside the repository. Never commit your API key.

### 3. Try it

POST /api/chat

```bash
{
  "question": "What is RAG?"
}

Response:
{
  "answer": "RAG (Retrieval-Augmented Generation) combines search over a knowledge base with text generation...",
  "sources": ["What is RAG?"],
  "isDemo": false
}
```
## 📡 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/knowledgeitems` | List all knowledge items |
| `GET` | `/api/knowledgeitems/{id}` | Get a single item |
| `POST` | `/api/knowledgeitems` | Create an item |
| `PUT` | `/api/knowledgeitems/{id}` | Update an item |
| `DELETE` | `/api/knowledgeitems/{id}` | Delete an item |
| `POST` | `/api/chat` | Ask the assistant a question (RAG) |

## 🧠 How the RAG flow works
Retrieve — your question is tokenized and scored against the knowledge base to find the most relevant snippets
Generate — the retrieved context is sent to the GPT Maker agent, which answers grounded in that context
Return — the answer comes back with the sources it used

## 🛣️ Roadmap
 Function calling — the agent deciding to invoke real services/flows
 Vector search with Qdrant for semantic retrieval
 Persistence with EF Core + SQLite
 Simple chat front-end (Angular)

## 📄 License
MIT

