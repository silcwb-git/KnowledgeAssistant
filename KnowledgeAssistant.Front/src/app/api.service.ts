import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface KnowledgeItem {
  id: string;
  title: string;
  content: string;
  category: string;
  createdAt: string;
  updatedAt?: string;
}

export interface ChatResponse {
  answer: string;
  sources: string[];
  isDemo?: boolean;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  private base = '/api';

  constructor(private http: HttpClient) {}

  // Conversa (RAG + GPT Maker)
  chat(question: string): Observable<ChatResponse> {
    return this.http.post<ChatResponse>(`${this.base}/chat`, { question });
  }

  // Contar registros
  count(): Observable<number> {
    return this.http.get<number>(`${this.base}/knowledgeitems/count`);
  }

  // Listar todos
  getAll(): Observable<KnowledgeItem[]> {
    return this.http.get<KnowledgeItem[]>(`${this.base}/knowledgeitems`);
  }

  // Criar item
  create(item: { title: string; content: string; category: string }): Observable<KnowledgeItem> {
    return this.http.post<KnowledgeItem>(`${this.base}/knowledgeitems`, item);
  }

  // Buscar
  search(q: string): Observable<KnowledgeItem[]> {
    return this.http.post<KnowledgeItem[]>(`${this.base}/knowledgeitems/search`, { q });
  }
}