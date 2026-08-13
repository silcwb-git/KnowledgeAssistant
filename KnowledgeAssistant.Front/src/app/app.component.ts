import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService, KnowledgeItem, ChatResponse } from './api.service';

interface Message {
  role: 'user' | 'bot';
  text: string;
  time: string;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  messages: Message[] = [];
  input = '';
  loading = false;
  typing = false;

  // Estado das ferramentas
  countResult: string | null = null;
  items: KnowledgeItem[] = [];
  searchResults: KnowledgeItem[] = [];

  constructor(private api: ApiService) {
    this.addBot('Olá! 👋 Eu sou o RAG, assistente da base de conhecimento da KETHER. Pergunte qualquer coisa sobre as políticas, ou use os botões abaixo para contar, criar ou buscar itens.');
  }

  send() {
    const text = this.input.trim();
    if (!text || this.loading) return;

    this.addUser(text);
    this.input = '';
    this.loading = true;
    this.typing = true;

    this.api.chat(text).subscribe({
      next: (res: ChatResponse) => {
        this.typing = false;
        this.addBot(res.answer);
        this.loading = false;
      },
      error: (err) => {
        this.typing = false;
        this.loading = false;
        this.addBot('Ops, não consegui falar com a API. Verifica se ela está rodando na porta 5219. 😅');
      }
    });
  }

  // 📊 Contar registros
  doCount() {
    this.loading = true;
    this.api.count().subscribe({
      next: (n: number) => {
        this.loading = false;
        this.countResult = `📊 Existem ${n} itens na base de conhecimento.`;
        this.addBot(this.countResult);
      },
      error: () => {
        this.loading = false;
        this.addBot('Não consegui contar os registros. 😅');
      }
    });
  }

  // ➕ Criar item
  doCreate() {
    const title = prompt('Título do novo item:');
    if (!title) return;
    const content = prompt('Conteúdo do item:');
    if (!content) return;
    const category = prompt('Categoria (ex: Políticas):') || 'Geral';

    this.loading = true;
    this.api.create({ title, content, category }).subscribe({
      next: (item: KnowledgeItem) => {
        this.loading = false;
        this.addBot(`✅ Item criado com sucesso!\n\n**Título:** ${item.title}\n**Categoria:** ${item.category}\n**ID:** ${item.id.slice(0, 8)}...`);
      },
      error: () => {
        this.loading = false;
        this.addBot('Não consegui criar o item. 😅');
      }
    });
  }

  // 🔍 Buscar
  doSearch() {
    const q = prompt('O que você quer buscar na base?');
    if (!q) return;

    this.loading = true;
    this.api.search(q).subscribe({
      next: (results: KnowledgeItem[]) => {
        this.loading = false;
        if (results.length === 0) {
          this.addBot(`🔍 Não encontrei nada para "${q}".`);
        } else {
          this.searchResults = results;
          const list = results
            .map((r, i) => `**${i + 1}. ${r.title}** (${r.category})\n${r.content}`)
            .join('\n\n');
          this.addBot(`🔍 Encontrei ${results.length} resultado(s) para "${q}":\n\n${list}`);
        }
      },
      error: () => {
        this.loading = false;
        this.addBot('Não consegui buscar. 😅');
      }
    });
  }

  private addUser(text: string) {
    this.messages.push({ role: 'user', text, time: this.now() });
  }

  private addBot(text: string) {
    this.messages.push({ role: 'bot', text, time: this.now() });
  }

  private now(): string {
    return new Date().toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });
  }
}