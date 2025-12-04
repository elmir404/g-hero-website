'use client';

import { useEffect, useState } from 'react';
import { useRouter, useParams } from 'next/navigation';
import Link from 'next/link';
import apiClient from '@/lib/api-client';

interface ContactMessage {
  id: number;
  name: string;
  email: string;
  subject?: string;
  company?: string;
  message: string;
  createdAt: string;
  isRead: boolean;
  readAt?: string;
}

export default function ContactMessageDetail() {
  const router = useRouter();
  const params = useParams();
  const id = params?.id ? parseInt(params.id as string) : null;
  const [message, setMessage] = useState<ContactMessage | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (id && !isNaN(id)) {
      loadMessage();
    } else {
      setError('Invalid message ID');
      setLoading(false);
    }
  }, [id]);

  async function loadMessage() {
    if (!id || isNaN(id)) {
      setError('Invalid message ID');
      setLoading(false);
      return;
    }

    setLoading(true);
    setError(null);
    try {
      const data = await apiClient.getContactMessage(id);
      setMessage(data);
      // Auto-mark as read if unread
      if (!data.isRead) {
        await apiClient.markAsRead(id);
        setMessage({ ...data, isRead: true });
      }
    } catch (err: any) {
      setError(err.message || 'Failed to load message');
    } finally {
      setLoading(false);
    }
  }

  async function handleMarkAsRead() {
    if (!message) return;
    try {
      await apiClient.markAsRead(message.id);
      setMessage({ ...message, isRead: true });
    } catch (err: any) {
      alert(err.message || 'Failed to mark as read');
    }
  }

  async function handleMarkAsUnread() {
    if (!message) return;
    try {
      await apiClient.markAsUnread(message.id);
      setMessage({ ...message, isRead: false, readAt: undefined });
    } catch (err: any) {
      alert(err.message || 'Failed to mark as unread');
    }
  }

  async function handleDelete() {
    if (!message) return;
    if (!confirm('Are you sure you want to delete this message?')) {
      return;
    }
    try {
      await apiClient.deleteContactMessage(message.id);
      router.push('/admin/contacts');
    } catch (err: any) {
      alert(err.message || 'Failed to delete message');
    }
  }

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleString();
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="text-gray-600">Loading message...</div>
      </div>
    );
  }

  if (error || !message) {
    return (
      <div>
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {error || 'Message not found'}
        </div>
        <Link href="/admin/contacts" className="text-sakura-600 hover:text-sakura-900">
          ← Back to Messages
        </Link>
      </div>
    );
  }

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <Link
          href="/admin/contacts"
          className="text-sakura-600 hover:text-sakura-900 flex items-center"
        >
          <svg
            className="w-5 h-5 mr-2"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M15 19l-7-7 7-7"
            />
          </svg>
          Back to Messages
        </Link>
        <div className="flex space-x-2">
          {message.isRead ? (
            <button
              onClick={handleMarkAsUnread}
              className="px-4 py-2 bg-gray-200 text-gray-700 rounded-md hover:bg-gray-300 text-sm font-medium"
            >
              Mark as Unread
            </button>
          ) : (
            <button
              onClick={handleMarkAsRead}
              className="px-4 py-2 bg-teal-600 text-white rounded-md hover:bg-teal-700 text-sm font-medium"
            >
              Mark as Read
            </button>
          )}
          <button
            onClick={handleDelete}
            className="px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700 text-sm font-medium"
          >
            Delete
          </button>
        </div>
      </div>

      <div className="bg-white rounded-lg shadow border border-gray-200">
        <div className="px-6 py-4 border-b border-gray-200">
          <div className="flex justify-between items-start">
            <div>
              <h1 className="text-2xl font-bold text-gray-900">{message.name}</h1>
              <p className="text-gray-600 mt-1">{message.email}</p>
              {message.company && <p className="text-gray-600">{message.company}</p>}
            </div>
            <div className="text-right">
              {message.isRead ? (
                <span className="px-3 py-1 text-sm font-medium text-teal-800 bg-teal-100 rounded-full">
                  Read
                </span>
              ) : (
                <span className="px-3 py-1 text-sm font-medium text-sakura-800 bg-sakura-100 rounded-full">
                  Unread
                </span>
              )}
            </div>
          </div>
        </div>

        <div className="px-6 py-4 space-y-4">
          <div>
            <label className="text-sm font-medium text-gray-500">Subject</label>
            <p className="mt-1 text-gray-900">{message.subject || 'No subject'}</p>
          </div>

          <div>
            <label className="text-sm font-medium text-gray-500">Message</label>
            <div className="mt-1 p-4 bg-gray-50 rounded-md">
              <p className="text-gray-900 whitespace-pre-wrap">{message.message}</p>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4 pt-4 border-t border-gray-200">
            <div>
              <label className="text-sm font-medium text-gray-500">Received</label>
              <p className="mt-1 text-gray-900">{formatDate(message.createdAt)}</p>
            </div>
            {message.readAt && (
              <div>
                <label className="text-sm font-medium text-gray-500">Read At</label>
                <p className="mt-1 text-gray-900">{formatDate(message.readAt)}</p>
              </div>
            )}
          </div>

          <div className="pt-4 border-t border-gray-200">
            <a
              href={`mailto:${message.email}${message.subject ? `?subject=Re: ${encodeURIComponent(message.subject)}` : ''}`}
              className="inline-flex items-center px-4 py-2 bg-sakura-600 text-white rounded-md hover:bg-sakura-700 text-sm font-medium"
            >
              <svg
                className="w-5 h-5 mr-2"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"
                />
              </svg>
              Reply via Email
            </a>
          </div>
        </div>
      </div>
    </div>
  );
}

