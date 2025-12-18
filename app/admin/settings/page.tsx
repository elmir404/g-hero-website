'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import apiClient from '@/lib/api-client';

export const dynamic = 'force-dynamic';
export const revalidate = 0;

interface SiteSettings {
  id?: number;
  phone?: string;
  email?: string;
  address?: string;
  workingHours?: string;
  whatsAppUrl?: string;
  instagramUrl?: string;
  facebookUrl?: string;
  linkedInUrl?: string;
  mapEmbedUrl?: string;
}

export default function SiteSettingsPage() {
  const router = useRouter();
  const [settings, setSettings] = useState<SiteSettings>({});
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  useEffect(() => {
    loadSettings();
  }, []);

  async function loadSettings() {
    setLoading(true);
    setError(null);
    try {
      const data = await apiClient.getAdminSiteSettings();
      setSettings(data);
    } catch (err: any) {
      setError(err.message || 'Failed to load site settings');
    } finally {
      setLoading(false);
    }
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setSaving(true);
    setError(null);
    setSuccess(null);

    try {
      await apiClient.updateSiteSettings(settings);
      setSuccess('Site settings updated successfully!');
      setTimeout(() => setSuccess(null), 3000);
    } catch (err: any) {
      setError(err.message || 'Failed to update site settings');
      setTimeout(() => setError(null), 5000);
    } finally {
      setSaving(false);
    }
  }

  const handleChange = (field: keyof SiteSettings, value: string) => {
    setSettings(prev => ({ ...prev, [field]: value }));
    // Clear error when user starts typing
    if (error) setError(null);
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="text-gray-600">Loading site settings...</div>
      </div>
    );
  }

  return (
    <div>
      <div className="mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Site Settings</h1>
        <p className="mt-2 text-sm text-gray-600">
          Manage contact information and social media links displayed on the website
        </p>
      </div>

      {success && (
        <div className="bg-green-50 border border-green-200 text-green-800 px-4 py-3 rounded-lg mb-6">
          {success}
        </div>
      )}

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-800 px-4 py-3 rounded-lg mb-6">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit} className="bg-white rounded-lg shadow border border-gray-200 p-6">
        <div className="space-y-6">
          {/* Contact Information Section */}
          <div>
            <h2 className="text-xl font-semibold text-gray-900 mb-4 pb-2 border-b">Contact Information</h2>
            
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label htmlFor="phone" className="block text-sm font-medium text-gray-700 mb-2">
                  Phone Number
                </label>
                <input
                  type="text"
                  id="phone"
                  value={settings.phone || ''}
                  onChange={(e) => handleChange('phone', e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-sakura-500 focus:border-transparent"
                  placeholder="+1 234 567 8900"
                />
              </div>

              <div>
                <label htmlFor="email" className="block text-sm font-medium text-gray-700 mb-2">
                  Email Address
                </label>
                <input
                  type="email"
                  id="email"
                  value={settings.email || ''}
                  onChange={(e) => handleChange('email', e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-sakura-500 focus:border-transparent"
                  placeholder="contact@example.com"
                />
              </div>

              <div className="md:col-span-2">
                <label htmlFor="address" className="block text-sm font-medium text-gray-700 mb-2">
                  Address
                </label>
                <textarea
                  id="address"
                  rows={3}
                  value={settings.address || ''}
                  onChange={(e) => handleChange('address', e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-sakura-500 focus:border-transparent"
                  placeholder="123 Main Street, City, Country"
                />
              </div>

              <div className="md:col-span-2">
                <label htmlFor="workingHours" className="block text-sm font-medium text-gray-700 mb-2">
                  Working Hours
                </label>
                <input
                  type="text"
                  id="workingHours"
                  value={settings.workingHours || ''}
                  onChange={(e) => handleChange('workingHours', e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-sakura-500 focus:border-transparent"
                  placeholder="Monday - Friday: 9:00 AM - 6:00 PM"
                />
                <p className="mt-1 text-xs text-gray-500">Example: Monday - Friday: 9:00 AM - 6:00 PM</p>
              </div>
            </div>
          </div>

          {/* Social Media Links Section */}
          <div>
            <h2 className="text-xl font-semibold text-gray-900 mb-4 pb-2 border-b">Social Media Links</h2>
            
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label htmlFor="whatsAppUrl" className="block text-sm font-medium text-gray-700 mb-2">
                  WhatsApp URL
                </label>
                <input
                  type="url"
                  id="whatsAppUrl"
                  value={settings.whatsAppUrl || ''}
                  onChange={(e) => handleChange('whatsAppUrl', e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-sakura-500 focus:border-transparent"
                  placeholder="https://wa.me/1234567890"
                />
              </div>

              <div>
                <label htmlFor="instagramUrl" className="block text-sm font-medium text-gray-700 mb-2">
                  Instagram URL
                </label>
                <input
                  type="url"
                  id="instagramUrl"
                  value={settings.instagramUrl || ''}
                  onChange={(e) => handleChange('instagramUrl', e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-sakura-500 focus:border-transparent"
                  placeholder="https://instagram.com/yourcompany"
                />
              </div>

              <div>
                <label htmlFor="facebookUrl" className="block text-sm font-medium text-gray-700 mb-2">
                  Facebook URL
                </label>
                <input
                  type="url"
                  id="facebookUrl"
                  value={settings.facebookUrl || ''}
                  onChange={(e) => handleChange('facebookUrl', e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-sakura-500 focus:border-transparent"
                  placeholder="https://facebook.com/yourcompany"
                />
              </div>

              <div>
                <label htmlFor="linkedInUrl" className="block text-sm font-medium text-gray-700 mb-2">
                  LinkedIn URL
                </label>
                <input
                  type="url"
                  id="linkedInUrl"
                  value={settings.linkedInUrl || ''}
                  onChange={(e) => handleChange('linkedInUrl', e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-sakura-500 focus:border-transparent"
                  placeholder="https://linkedin.com/company/yourcompany"
                />
              </div>
            </div>
          </div>

          {/* Map Section */}
          <div>
            <h2 className="text-xl font-semibold text-gray-900 mb-4 pb-2 border-b">Location Map</h2>
            
            <div>
              <label htmlFor="mapEmbedUrl" className="block text-sm font-medium text-gray-700 mb-2">
                Google Maps Embed URL
              </label>
              <textarea
                id="mapEmbedUrl"
                rows={4}
                value={settings.mapEmbedUrl || ''}
                onChange={(e) => handleChange('mapEmbedUrl', e.target.value)}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-sakura-500 focus:border-transparent font-mono text-sm"
                placeholder="https://www.google.com/maps/embed?pb=..."
              />
              <p className="mt-1 text-xs text-gray-500">
                Get the embed URL from Google Maps: Share → Embed a map → Copy the iframe src URL
              </p>
            </div>
          </div>

          {/* Submit Button */}
          <div className="flex justify-end pt-4 border-t">
            <button
              type="submit"
              disabled={saving}
              className="px-6 py-2 bg-sakura-600 text-white rounded-lg font-medium hover:bg-sakura-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
            >
              {saving ? 'Saving...' : 'Save Settings'}
            </button>
          </div>
        </div>
      </form>
    </div>
  );
}

