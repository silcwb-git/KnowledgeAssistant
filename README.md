# 🤖 KnowledgeAssistant

A complete **knowledge assistant** with **RAG (Retrieval-Augmented Generation)** + **Function Calling**, integrated with **GPT Maker** and featuring an **Angular front-end**. The user chats with an AI agent that answers based on a real knowledge base (SQLite), counts records, creates items, and searches content — end to end.

> 🎯 Portfolio project demonstrating **full-stack architecture with AI**: Angular → .NET API → RAG → SQLite → GPT Maker.

---

## ✨ Features

- 🔍 **RAG pipeline** — retrieves relevant snippets from the knowledge base, then generates answers grounded in that context
- 🧠 **Real AI integration** — talks to a GPT Maker agent via `HttpClient` + Bearer token
- 🛠️ **Function Calling (Intentions)** — the agent decides to call real API endpoints (count, create, search)
- 💾 **SQLite persistence** — items survive restarts (EF Core)
- 🖥️ **Angular front-end** — modern, responsive chat UI
- 🧪 **Demo mode** — runs without credentials, returning retrieved snippets
- 📚 **Swagger UI** — interactive API docs at `/swagger`
- 🔐 **Secure by default** — API key via .NET user-secrets, never committed

---

## 🏗️ Architecture
```
KnowledgeAssistant/
├── KnowledgeAssistant.

Api/        → .NET API (backend)
│   ├── Models/                    → DTOs (ChatRequest, ChatResponse, KnowledgeItem)
│   ├── Options/                   → AiOptions (BaseUrl, ApiKey, AgentId)
│   ├── Services/
│   │   ├── KnowledgeStore.cs      → SQLite persistence (EF Core)
│   │   ├── IKnowledgeSearch.cs    → Retrieval interface
│   │   ├── SimpleKnowledgeSearch.cs → RAG (tokenization + scoring)
│   │   ├── IAiClient.cs           → AI provider interface
│   │   ├── GptMakerAiClient.cs    → GPT Maker HTTP client
│   │   └── ChatService.cs         → Orchestrates: question → retrieve → generate
│   └── Controllers/               → REST endpoints
└── KnowledgeAssistant.

Front/      → Angular front-end (chat)
    └── src/app/
        ├── api.service.ts         → HTTP client for the API
        └── app.component.*        → Chat (messages, tools, input)
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) + [Angular CLI](https://angular.io/cli)
- A [GPT Maker](https://gptmaker.ai) account with an agent + API token

### 1. Clone & run the API
```bash
git clone https://github.com/silcwb-git/KnowledgeAssistant.git
cd KnowledgeAssistant/KnowledgeAssistant.

Api
dotnet run
```

The API runs at `http://localhost:5219` (Swagger at `/swagger`).

### 2. Configure your AI credentials (user-secrets)
```bash
dotnet user-secrets init
dotnet user-secrets set "AiOptions:ApiKey" "YOUR_GPTMAKER_TOKEN"
dotnet user-secrets set "AiOptions:AgentId" "YOUR_AGENT_ID"
dotnet user-secrets set "AiOptions:BaseUrl" "https://api.gptmaker.ai"
```

🔒 **Security:** credentials live in user-secrets, outside the repository. Never commit your API key.

### 3. Run the Angular front-end

In another terminal:
```bash
cd KnowledgeAssistant/KnowledgeAssistant.

Front
npm install
ng serve
```

Open `http://localhost:4200` — the chat appears. The `proxy.conf.json` redirects `/api/*` calls to the API on port 5219.

### 4. (Optional) Expose the API to GPT Maker

In development, use **ngrok** to expose the API publicly:
```bash
ngrok http 5219
```

Copy the generated URL (e.g. `https://xxxx.ngrok-free.dev`) and use it in the GPT Maker **intentions**.

---

## 📡 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/knowledgeitems` | List all knowledge items |
| `GET` | `/api/knowledgeitems/{id}` | Get a single item |
| `POST` | `/api/knowledgeitems` | Create an item |
| `PUT` | `/api/knowledgeitems/{id}` | Update an item |
| `DELETE` | `/api/knowledgeitems/{id}` | Delete an item |
| `GET` | `/api/knowledgeitems/count` | Count items |
| `POST` | `/api/knowledgeitems/search` | Search the base |
| `POST` | `/api/chat` | Ask the assistant a question (RAG) |

---

## 🧠 How the RAG flow works

1. **Retrieve** — your question is tokenized and scored against the knowledge base to find the most relevant snippets
2. **Generate** — the retrieved context is sent to the GPT Maker agent, which answers grounded in that context
3. **Return** — the answer comes back with the sources it used

### End-to-end flow (front-end)
```text
User types in the Angular chat (port 4200)
→ Angular calls POST /api/chat (via proxy → API on 5219)
→ API performs RAG: SimpleKnowledgeSearch on SQLite → finds the relevant snippet
→ API builds the context and calls GPT Maker (GptMakerAiClient)
→ GPT Maker writes the final answer
→ API returns it to Angular → appears in the bubble
```

---

## 🧠 Function Calling (Intentions)

The agent doesn't just respond — it **acts**. Using the GPT Maker **intentions** mechanism, the agent decides to call real API endpoints to execute actions, reading and writing to the knowledge base.

### Configured intentions

| Intention | Method | Endpoint | Description |
|-----------|--------|----------|-------------|
| Count knowledge base items | `GET` | `/api/knowledgeitems/count` | Returns the number of items |
| Create knowledge base item | `POST` | `/api/knowledgeitems` | Adds a new item |

### Usage example

**User:** *"how many items are in the knowledge base?"*

**Agent:** *"The knowledge base has 8 items."* 👋

**User:** *"add to the base: how does home office work? the employee can work 2 days a week from home, category: policies"*

**Agent:** *"The item 'Home Office' was added to the knowledge base."*

> **Note:** in development, the API is exposed via **ngrok** so GPT Maker (cloud) can reach it. In production, just point the intention to the API's public URL.

---

## 🛣️ Roadmap

- ✅ Function Calling — the agent deciding to invoke real services
- ✅ SQLite persistence (EF Core)
- ✅ Angular front-end (chat)
- 🔜 Vector search with Qdrant for semantic retrieval
- 🔜 Deploy the API (Render/Railway) and the front (Vercel)

---

## 📄 License

MIT