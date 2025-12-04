# Next.js Integration Guide

## Setup

### 1. Install Dependencies

```bash
npm install
```

### 2. Environment Variables

Create `.env.local`:

```env
NEXT_PUBLIC_API_URL=http://localhost:5000/api
```

### 3. API Client

The API client (`lib/api-client.ts`) is already created and handles:
- Token management
- Request/response handling
- Error handling

## Using the API in Next.js

### Option 1: Client-Side (useEffect)

```typescript
'use client';

import { useEffect, useState } from 'react';
import apiClient from '@/lib/api-client';

export default function HomePage() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchData() {
      try {
        const homeData = await apiClient.getHomePage('en');
        setHomeData(homeData);
      } catch (error) {
        console.error('Failed to fetch:', error);
      } finally {
        setLoading(false);
      }
    }
    fetchData();
  }, []);

  if (loading) return <div>Loading...</div>;
  if (!data) return <div>Error loading content</div>;

  return (
    <div>
      {/* Render your components with data */}
    </div>
  );
}
```

### Option 2: Server-Side (App Router)

```typescript
// app/page.tsx
export default async function HomePage({ 
  searchParams 
}: { 
  searchParams: { lang?: string } 
}) {
  const lang = searchParams.lang || 'en';
  const apiUrl = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000/api';
  
  const homeData = await fetch(`${apiUrl}/home?lang=${lang}`, {
    cache: 'no-store' // or 'revalidate' for ISR
  }).then(res => res.json());

  return <HomePageClient data={homeData} />;
}
```

### Option 3: Server-Side (Pages Router)

```typescript
// pages/index.tsx
import { GetServerSideProps } from 'next';
import apiClient from '../lib/api-client';

export const getServerSideProps: GetServerSideProps = async (context) => {
  const lang = (context.query.lang as string) || 'en';
  
  try {
    const homeData = await apiClient.getHomePage(lang);
    return {
      props: {
        homeData
      }
    };
  } catch (error) {
    return {
      props: {
        homeData: null,
        error: 'Failed to load content'
      }
    };
  }
};

export default function HomePage({ homeData }: { homeData: any }) {
  // Render with homeData
}
```

## Updating Components

### Contact Component

Update `components/Contact.tsx`:

```typescript
'use client';

import { useState } from 'react';
import apiClient from '@/lib/api-client';

export default function Contact() {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    subject: '',
    company: '',
    message: '',
  });
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const response = await apiClient.submitContact(formData);
      if (response.success) {
        setSuccess(true);
        setFormData({ name: '', email: '', subject: '', company: '', message: '' });
      }
    } catch (err: any) {
      setError(err.message || 'Failed to send message');
    } finally {
      setLoading(false);
    }
  };

  // ... rest of component
}
```

## Admin Panel in Next.js

### 1. Create Admin Layout

```typescript
// app/admin/layout.tsx
'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import apiClient from '@/lib/api-client';

export default function AdminLayout({ children }: { children: React.ReactNode }) {
  const router = useRouter();

  useEffect(() => {
    async function checkAuth() {
      try {
        await apiClient.getCurrentUser();
      } catch {
        router.push('/admin/login');
      }
    }
    checkAuth();
  }, []);

  return <div>{children}</div>;
}
```

### 2. Admin Dashboard

```typescript
// app/admin/dashboard/page.tsx
'use client';

import { useEffect, useState } from 'react';
import apiClient from '@/lib/api-client';

export default function AdminDashboard() {
  const [messages, setMessages] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function loadMessages() {
      try {
        const response = await apiClient.getContactMessages(1, 20);
        setMessages(response.data);
      } catch (error) {
        console.error('Failed to load messages:', error);
      } finally {
        setLoading(false);
      }
    }
    loadMessages();
  }, []);

  // Render dashboard
}
```

### 3. Content Management

```typescript
// app/admin/content/page.tsx
'use client';

import { useEffect, useState } from 'react';
import apiClient from '@/lib/api-client';

export default function ContentManagement() {
  const [sections, setSections] = useState<any[]>([]);
  const [lang, setLang] = useState('en');

  useEffect(() => {
    async function loadContent() {
      // Note: You'll need to add getContentSections to api-client
      const response = await fetch(
        `${process.env.NEXT_PUBLIC_API_URL}/admin/content?lang=${lang}`,
        {
          headers: {
            'Authorization': `Bearer ${localStorage.getItem('auth_token')}`
          }
        }
      );
      const data = await response.json();
      setSections(data.data);
    }
    loadContent();
  }, [lang]);

  // Render content management UI
}
```

## Authentication Flow

1. **Login Page** (`/admin/login`):
   ```typescript
   const handleLogin = async () => {
     const response = await apiClient.login(username, password);
     // Token is automatically stored
     router.push('/admin/dashboard');
   };
   ```

2. **Protected Routes**:
   - Check token on page load
   - Redirect to login if no token
   - Include token in API requests

3. **Logout**:
   ```typescript
   const handleLogout = () => {
     apiClient.logout();
     router.push('/admin/login');
   };
   ```

## Best Practices

1. **Error Handling**: Always wrap API calls in try-catch
2. **Loading States**: Show loading indicators during API calls
3. **Token Refresh**: Implement token refresh logic if needed
4. **Caching**: Use Next.js caching for static content
5. **Type Safety**: Create TypeScript interfaces for API responses

