// API Client for Next.js to consume the .NET Web API

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000/api';

class ApiClient {
  private baseUrl: string;
  private token: string | null = null;

  constructor(baseUrl: string = API_BASE_URL) {
    this.baseUrl = baseUrl;
    // Load token from localStorage or cookie (only on client)
    if (typeof window !== 'undefined') {
      try {
        this.token = localStorage.getItem('auth_token');
      } catch (e) {
        // localStorage might not be available
        console.warn('localStorage not available:', e);
      }
    }
  }

  setToken(token: string) {
    this.token = token;
    if (typeof window !== 'undefined') {
      localStorage.setItem('auth_token', token);
    }
  }

  clearToken() {
    this.token = null;
    if (typeof window !== 'undefined') {
      localStorage.removeItem('auth_token');
    }
  }

  private async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<T> {
    const url = `${this.baseUrl}${endpoint}`;
    const headers: Record<string, string> = {
      'Content-Type': 'application/json',
      ...(options.headers as Record<string, string>),
    };

    if (this.token) {
      headers['Authorization'] = `Bearer ${this.token}`;
    }

    const response = await fetch(url, {
      ...options,
      headers,
    });

    if (!response.ok) {
      let errorMessage = `HTTP error! status: ${response.status}`;
      try {
        const error = await response.json();
        errorMessage = error.message || errorMessage;
        
        // Handle 401 Unauthorized - redirect to login
        if (response.status === 401) {
          this.clearToken();
          if (typeof window !== 'undefined') {
            window.location.href = '/admin/login';
          }
        }
      } catch {
        // If JSON parsing fails, use status text
        errorMessage = response.statusText || errorMessage;
      }
      throw new Error(errorMessage);
    }

    return response.json();
  }

  // Home Page API
  async getHomePage(lang: string = 'en') {
    return this.request<any>(`/home?lang=${lang}`);
  }

  async getHero(lang: string = 'en') {
    return this.request<any>(`/home/hero?lang=${lang}`);
  }

  async getServices(lang: string = 'en') {
    return this.request<any[]>(`/home/services?lang=${lang}`);
  }

  async getTestimonials(lang: string = 'en') {
    return this.request<any[]>(`/home/testimonials?lang=${lang}`);
  }

  async getFaqs(lang: string = 'en') {
    return this.request<any[]>(`/home/faqs?lang=${lang}`);
  }

  async getContactInfo(lang: string = 'en') {
    return this.request<any>(`/home/contact-info?lang=${lang}`);
  }

  // Contact API
  async submitContact(data: {
    name: string;
    email: string;
    subject?: string;
    company?: string;
    message: string;
  }) {
    return this.request<{ success: boolean; message: string; id: number }>(
      '/contact',
      {
        method: 'POST',
        body: JSON.stringify(data),
      }
    );
  }

  // Auth API
  async login(username: string, password: string) {
    const response = await this.request<{
      token: string;
      username: string;
      email: string;
      fullName?: string;
      roles: string[];
      expiresAt: string;
    }>('/auth/login', {
      method: 'POST',
      body: JSON.stringify({ username, password }),
    });

    if (response.token) {
      this.setToken(response.token);
    }

    return response;
  }

  async getCurrentUser() {
    return this.request<{
      username: string;
      email: string;
      roles: string[];
    }>('/auth/me');
  }

  logout() {
    this.clearToken();
  }

  // Admin API - Contact Messages
  async getContactMessages(page: number = 1, pageSize: number = 20) {
    return this.request<{
      data: any[];
      total: number;
      page: number;
      pageSize: number;
      totalPages: number;
    }>(`/admin/contact?page=${page}&pageSize=${pageSize}`);
  }

  async getUnreadMessages() {
    return this.request<{ data: any[]; count: number }>('/admin/contact/unread');
  }

  async getContactMessage(id: number) {
    return this.request<any>(`/admin/contact/${id}`);
  }

  async markAsRead(id: number) {
    return this.request<{ message: string }>(`/admin/contact/${id}/read`, {
      method: 'PATCH',
    });
  }

  async markAsUnread(id: number) {
    return this.request<{ message: string }>(`/admin/contact/${id}/unread`, {
      method: 'PATCH',
    });
  }

  async deleteContactMessage(id: number) {
    return this.request<{ message: string }>(`/admin/contact/${id}`, {
      method: 'DELETE',
    });
  }

  // Admin API - Content Management
  async getContentSections(lang?: string) {
    const url = lang ? `/admin/content?lang=${lang}` : '/admin/content';
    return this.request<{ data: any[] }>(url);
  }

  async getContentSection(id: number) {
    return this.request<any>(`/admin/content/${id}`);
  }

  async createContentSection(data: any) {
    return this.request<{ id: number; message: string }>('/admin/content', {
      method: 'POST',
      body: JSON.stringify(data),
    });
  }

  async updateContentSection(id: number, data: any) {
    return this.request<{ message: string }>(`/admin/content/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  }

  async deleteContentSection(id: number) {
    return this.request<{ message: string }>(`/admin/content/${id}`, {
      method: 'DELETE',
    });
  }

  async toggleContentSectionActive(id: number) {
    return this.request<{ message: string }>(`/admin/content/${id}/toggle-active`, {
      method: 'PATCH',
    });
  }
}

export const apiClient = new ApiClient();
export default apiClient;

