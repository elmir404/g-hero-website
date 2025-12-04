'use client';

import { useEffect, useState } from 'react';
import { useRouter, useParams } from 'next/navigation';
import Link from 'next/link';
import apiClient from '@/lib/api-client';

interface ContentSectionForm {
  sectionKey: string;
  languageCode: string;
  title?: string;
  content?: string;
  subtitle?: string;
  additionalData?: string;
  displayOrder: number;
  isActive: boolean;
}

export default function ContentSectionEdit() {
  const router = useRouter();
  const params = useParams();
  const id = params?.id && params.id !== 'new' ? parseInt(params.id as string) : null;
  const isNew = id === null;

  const [formData, setFormData] = useState<ContentSectionForm>({
    sectionKey: '',
    languageCode: 'en',
    title: '',
    content: '',
    subtitle: '',
    additionalData: '',
    displayOrder: 0,
    isActive: true,
  });

  const [loading, setLoading] = useState(!isNew);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!isNew && id) {
      loadSection();
    }
  }, [id, isNew]);

  async function loadSection() {
    setLoading(true);
    setError(null);
    try {
      const data = await apiClient.getContentSection(id!);
      setFormData({
        sectionKey: data.sectionKey || '',
        languageCode: data.languageCode || 'en',
        title: data.title || '',
        content: data.content || '',
        subtitle: data.subtitle || '',
        additionalData: data.additionalData || '',
        displayOrder: data.displayOrder || 0,
        isActive: data.isActive ?? true,
      });
    } catch (err: any) {
      setError(err.message || 'Failed to load content section');
    } finally {
      setLoading(false);
    }
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setSaving(true);
    setError(null);

    try {
      if (isNew) {
        await apiClient.createContentSection(formData);
      } else {
        await apiClient.updateContentSection(id!, formData);
      }
      router.push('/admin/content');
    } catch (err: any) {
      setError(err.message || 'Failed to save content section');
    } finally {
      setSaving(false);
    }
  }

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="text-gray-600">Loading...</div>
      </div>
    );
  }

  return (
    <div>
      <div className="mb-6">
        <Link
          href="/admin/content"
          className="text-sakura-600 hover:text-sakura-900 flex items-center mb-4"
        >
          <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M15 19l-7-7 7-7"
            />
          </svg>
          Back to Content Management
        </Link>
        <h1 className="text-3xl font-bold text-gray-900">
          {isNew ? 'Create Content Section' : 'Edit Content Section'}
        </h1>
      </div>

      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit} className="bg-white rounded-lg shadow border border-gray-200 p-6">
        <div className="space-y-6">
          <div>
            <label htmlFor="sectionKey" className="block text-sm font-medium text-gray-700 mb-2">
              Section Key <span className="text-red-600">*</span>
            </label>
            <input
              type="text"
              id="sectionKey"
              required
              value={formData.sectionKey}
              onChange={(e) => setFormData({ ...formData, sectionKey: e.target.value })}
              className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-sakura-500 focus:border-transparent"
              placeholder="e.g., hero.title, philosophy.content"
            />
            <p className="mt-1 text-sm text-gray-500">
              Unique identifier for this content section
            </p>
          </div>

          <div>
            <label htmlFor="languageCode" className="block text-sm font-medium text-gray-700 mb-2">
              Language <span className="text-red-600">*</span>
            </label>
            <select
              id="languageCode"
              required
              value={formData.languageCode}
              onChange={(e) => setFormData({ ...formData, languageCode: e.target.value })}
              className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-sakura-500 focus:border-transparent"
            >
              <option value="en">English</option>
              <option value="az">Azerbaijani</option>
              <option value="ja">日本語</option>
            </select>
          </div>

          <div>
            <label htmlFor="title" className="block text-sm font-medium text-gray-700 mb-2">
              Title
            </label>
            <input
              type="text"
              id="title"
              value={formData.title}
              onChange={(e) => setFormData({ ...formData, title: e.target.value })}
              className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-sakura-500 focus:border-transparent"
            />
          </div>

          <div>
            <label htmlFor="subtitle" className="block text-sm font-medium text-gray-700 mb-2">
              Subtitle
            </label>
            <input
              type="text"
              id="subtitle"
              value={formData.subtitle}
              onChange={(e) => setFormData({ ...formData, subtitle: e.target.value })}
              className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-sakura-500 focus:border-transparent"
            />
          </div>

          <div>
            <label htmlFor="content" className="block text-sm font-medium text-gray-700 mb-2">
              Content
            </label>
            <textarea
              id="content"
              rows={8}
              value={formData.content}
              onChange={(e) => setFormData({ ...formData, content: e.target.value })}
              className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-sakura-500 focus:border-transparent"
              placeholder="Main content (supports HTML)"
            />
          </div>

          <div>
            <label htmlFor="additionalData" className="block text-sm font-medium text-gray-700 mb-2">
              Additional Data (JSON)
            </label>
            <textarea
              id="additionalData"
              rows={4}
              value={formData.additionalData}
              onChange={(e) => setFormData({ ...formData, additionalData: e.target.value })}
              className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-sakura-500 focus:border-transparent font-mono text-sm"
              placeholder='{"key": "value"}'
            />
            <p className="mt-1 text-sm text-gray-500">
              JSON format for structured data (e.g., lists, arrays)
            </p>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label htmlFor="displayOrder" className="block text-sm font-medium text-gray-700 mb-2">
                Display Order
              </label>
              <input
                type="number"
                id="displayOrder"
                value={formData.displayOrder}
                onChange={(e) =>
                  setFormData({ ...formData, displayOrder: parseInt(e.target.value) || 0 })
                }
                className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-sakura-500 focus:border-transparent"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Status</label>
              <div className="mt-2">
                <label className="inline-flex items-center">
                  <input
                    type="checkbox"
                    checked={formData.isActive}
                    onChange={(e) => setFormData({ ...formData, isActive: e.target.checked })}
                    className="rounded border-gray-300 text-sakura-600 focus:ring-sakura-500"
                  />
                  <span className="ml-2 text-sm text-gray-700">Active</span>
                </label>
              </div>
            </div>
          </div>

          <div className="flex justify-end space-x-4 pt-4 border-t">
            <Link
              href="/admin/content"
              className="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50 text-sm font-medium"
            >
              Cancel
            </Link>
            <button
              type="submit"
              disabled={saving}
              className="px-4 py-2 bg-sakura-600 text-white rounded-md hover:bg-sakura-700 text-sm font-medium disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {saving ? 'Saving...' : isNew ? 'Create' : 'Save Changes'}
            </button>
          </div>
        </div>
      </form>
    </div>
  );
}

