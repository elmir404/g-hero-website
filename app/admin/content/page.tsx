'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import apiClient from '@/lib/api-client';

interface ContentSection {
  id: number;
  sectionKey: string;
  languageCode: string;
  title?: string;
  content?: string;
  subtitle?: string;
  additionalData?: string;
  displayOrder: number;
  isActive: boolean;
}

export default function ContentManagementPage() {
  const [sections, setSections] = useState<ContentSection[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedLang, setSelectedLang] = useState<string>('en');
  const [success, setSuccess] = useState<string | null>(null);

  useEffect(() => {
    loadSections();
  }, [selectedLang]);

  async function loadSections() {
    setLoading(true);
    setError(null);
    try {
      const response = await apiClient.getContentSections(selectedLang);
      setSections(response.data);
    } catch (err: any) {
      setError(err.message || 'Failed to load content sections');
    } finally {
      setLoading(false);
    }
  }

  async function handleToggleActive(id: number) {
    try {
      await apiClient.toggleContentSectionActive(id);
      setSuccess('Content section status updated');
      loadSections();
      setTimeout(() => setSuccess(null), 3000);
    } catch (err: any) {
      setError(err.message || 'Failed to update section');
      setTimeout(() => setError(null), 3000);
    }
  }

  async function handleDelete(id: number) {
    if (!confirm('Are you sure you want to delete this content section?')) {
      return;
    }
    try {
      await apiClient.deleteContentSection(id);
      setSuccess('Content section deleted');
      loadSections();
      setTimeout(() => setSuccess(null), 3000);
    } catch (err: any) {
      setError(err.message || 'Failed to delete section');
      setTimeout(() => setError(null), 3000);
    }
  }

  if (loading && sections.length === 0) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="text-gray-600">Loading content sections...</div>
      </div>
    );
  }

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Content Management</h1>
        <Link
          href="/admin/content/new"
          className="px-4 py-2 bg-sakura-600 text-white rounded-md hover:bg-sakura-700 text-sm font-medium"
        >
          + New Section
        </Link>
      </div>

      {/* Language Filter */}
      <div className="mb-6 flex space-x-2">
        {['en', 'az', 'ja'].map((lang) => (
          <button
            key={lang}
            onClick={() => setSelectedLang(lang)}
            className={`px-4 py-2 rounded-md text-sm font-medium ${
              selectedLang === lang
                ? 'bg-sakura-600 text-white'
                : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
            }`}
          >
            {lang === 'ja' ? '日本語' : lang.toUpperCase()}
          </button>
        ))}
      </div>

      {success && (
        <div className="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded mb-4">
          {success}
        </div>
      )}

      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {error}
        </div>
      )}

      {sections.length === 0 ? (
        <div className="bg-white rounded-lg shadow p-8 text-center border border-gray-200">
          <p className="text-gray-600 mb-4">No content sections found for {selectedLang.toUpperCase()}.</p>
          <Link
            href="/admin/content/new"
            className="text-sakura-600 hover:text-sakura-900 font-medium"
          >
            Create your first section →
          </Link>
        </div>
      ) : (
        <div className="bg-white rounded-lg shadow overflow-hidden border border-gray-200">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Section Key
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Title
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Language
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Order
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Status
                </th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Actions
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {sections.map((section) => (
                <tr key={section.id}>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="text-sm font-medium text-gray-900">{section.sectionKey}</div>
                  </td>
                  <td className="px-6 py-4">
                    <div className="text-sm text-gray-900">
                      {section.title || <span className="text-gray-400">No title</span>}
                    </div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span className="px-2 py-1 text-xs font-medium text-gray-800 bg-gray-100 rounded-full">
                      {section.languageCode.toUpperCase()}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {section.displayOrder}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    {section.isActive ? (
                      <span className="px-2 py-1 text-xs font-medium text-teal-800 bg-teal-100 rounded-full">
                        Active
                      </span>
                    ) : (
                      <span className="px-2 py-1 text-xs font-medium text-gray-800 bg-gray-100 rounded-full">
                        Inactive
                      </span>
                    )}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                    <div className="flex justify-end space-x-2">
                      <Link
                        href={`/admin/content/${section.id}`}
                        className="text-sakura-600 hover:text-sakura-900"
                      >
                        Edit
                      </Link>
                      <button
                        onClick={() => handleToggleActive(section.id)}
                        className="text-teal-600 hover:text-teal-900"
                      >
                        {section.isActive ? 'Deactivate' : 'Activate'}
                      </button>
                      <button
                        onClick={() => handleDelete(section.id)}
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
      )}
    </div>
  );
}

