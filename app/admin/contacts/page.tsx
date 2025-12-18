'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import apiClient from '@/lib/api-client';

// Force dynamic rendering - no caching
export const dynamic = 'force-dynamic';
export const revalidate = 0;

// Uncomment to enable API testing
// import TestApi from './test-api';

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

export default function ContactMessagesPage() {
  const router = useRouter();
  const [messages, setMessages] = useState<ContactMessage[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [page, setPage] = useState(1);
  const [pageSize] = useState(20);
  const [total, setTotal] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [filter, setFilter] = useState<'all' | 'unread'>('all');

  useEffect(() => {
    loadMessages();
  }, [page, filter]);

  async function loadMessages() {
    setLoading(true);
    setError(null);
    try {
      console.log('Loading messages...', { filter, page, pageSize });
      
      if (filter === 'unread') {
        // Force fresh fetch with cache bypass
        const response = await apiClient.getUnreadMessages();
        console.log('Unread messages response:', response);
        setMessages(response.data || []);
        setTotal(response.count || 0);
        setTotalPages(1);
      } else {
        // Force fresh fetch with cache bypass
        const response = await apiClient.getContactMessages(page, pageSize);
        console.log('All messages response:', response);
        setMessages(response.data || []);
        setTotal(response.total || 0);
        setTotalPages(response.totalPages || 0);
      }
    } catch (err: any) {
      console.error('Error loading messages:', err);
      console.error('Error details:', {
        message: err.message,
        stack: err.stack,
        name: err.name
      });
      setError(err.message || 'Failed to load messages. Please check your authentication and try again.');
    } finally {
      setLoading(false);
    }
  }

  async function handleMarkAsRead(id: number) {
    try {
      await apiClient.markAsRead(id);
      loadMessages();
    } catch (err: any) {
      alert(err.message || 'Failed to mark as read');
    }
  }

  async function handleMarkAsUnread(id: number) {
    try {
      await apiClient.markAsUnread(id);
      loadMessages();
    } catch (err: any) {
      alert(err.message || 'Failed to mark as unread');
    }
  }

  async function handleDelete(id: number) {
    if (!confirm('Are you sure you want to delete this message?')) {
      return;
    }
    try {
      await apiClient.deleteContactMessage(id);
      loadMessages();
    } catch (err: any) {
      alert(err.message || 'Failed to delete message');
    }
  }

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleString();
  };

  if (loading && messages.length === 0) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="text-gray-600">Loading messages...</div>
      </div>
    );
  }

  return (
    <div>
      {/* Uncomment to enable API testing */}
      {/* <TestApi /> */}
      
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Contact Messages</h1>
        <div className="flex space-x-2">
          <button
            onClick={() => {
              setFilter('all');
              setPage(1);
            }}
            className={`px-4 py-2 rounded-md text-sm font-medium ${
              filter === 'all'
                ? 'bg-sakura-600 text-white'
                : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
            }`}
          >
            All Messages
          </button>
          <button
            onClick={() => {
              setFilter('unread');
              setPage(1);
            }}
            className={`px-4 py-2 rounded-md text-sm font-medium ${
              filter === 'unread'
                ? 'bg-sakura-600 text-white'
                : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
            }`}
          >
            Unread Only
          </button>
        </div>
      </div>

      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          <p className="font-semibold">Error loading messages:</p>
          <p>{error}</p>
          <p className="text-sm mt-2">
            Make sure you're logged in and the Web API is running.
          </p>
        </div>
      )}

      {!loading && messages.length === 0 && !error ? (
        <div className="bg-white rounded-lg shadow p-8 text-center border border-gray-200">
          <p className="text-gray-600 mb-2">No messages found.</p>
          <p className="text-sm text-gray-500">
            Contact messages submitted through the website will appear here.
          </p>
        </div>
      ) : messages.length > 0 ? (
        <>
          <div className="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Status
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Name
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Email
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Subject
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Date
                  </th>
                  <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {messages.map((message) => (
                  <tr key={message.id} className={!message.isRead ? 'bg-sakura-50' : ''}>
                    <td className="px-6 py-4 whitespace-nowrap">
                      {message.isRead ? (
                        <span className="px-2 py-1 text-xs font-medium text-teal-800 bg-teal-100 rounded-full">
                          Read
                        </span>
                      ) : (
                        <span className="px-2 py-1 text-xs font-medium text-sakura-800 bg-sakura-100 rounded-full">
                          Unread
                        </span>
                      )}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm font-medium text-gray-900">{message.name}</div>
                      {message.company && (
                        <div className="text-sm text-gray-500">{message.company}</div>
                      )}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm text-gray-900">{message.email}</div>
                    </td>
                    <td className="px-6 py-4">
                      <div className="text-sm text-gray-900">
                        {message.subject || <span className="text-gray-400">No subject</span>}
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {formatDate(message.createdAt)}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <div className="flex justify-end space-x-2">
                        <Link
                          href={`/admin/contacts/${message.id}`}
                          className="text-sakura-600 hover:text-sakura-900"
                        >
                          View
                        </Link>
                        {message.isRead ? (
                          <button
                            onClick={() => handleMarkAsUnread(message.id)}
                            className="text-gray-600 hover:text-gray-900"
                          >
                            Mark Unread
                          </button>
                        ) : (
                          <button
                            onClick={() => handleMarkAsRead(message.id)}
                            className="text-teal-600 hover:text-teal-900"
                          >
                            Mark Read
                          </button>
                        )}
                        <button
                          onClick={() => handleDelete(message.id)}
                          className="text-red-600 hover:text-red-900"
                        >
                          Delete
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {filter === 'all' && totalPages > 1 && (
            <div className="mt-4 flex justify-between items-center">
              <div className="text-sm text-gray-700">
                Showing {(page - 1) * pageSize + 1} to {Math.min(page * pageSize, total)} of {total}{' '}
                messages
              </div>
              <div className="flex space-x-2">
                <button
                  onClick={() => setPage(page - 1)}
                  disabled={page === 1}
                  className="px-4 py-2 border border-gray-300 rounded-md text-sm font-medium text-gray-700 bg-white hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  Previous
                </button>
                <button
                  onClick={() => setPage(page + 1)}
                  disabled={page >= totalPages}
                  className="px-4 py-2 border border-gray-300 rounded-md text-sm font-medium text-gray-700 bg-white hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  Next
                </button>
              </div>
            </div>
          )}
        </>
      ) : null}
    </div>
  );
}

