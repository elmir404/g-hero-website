// Example: Updated Contact component to use API
'use client';

import { useState } from 'react';
import { useLocale } from '@/contexts/LocaleContext';
import apiClient from '../lib/api-client';

export default function Contact() {
  const { getNestedTranslation } = useLocale();
  const contact = getNestedTranslation('contact');
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
    setSuccess(false);

    try {
      const response = await apiClient.submitContact({
        name: formData.name,
        email: formData.email,
        subject: formData.subject,
        company: formData.company,
        message: formData.message,
      });

      if (response.success) {
        setSuccess(true);
        setFormData({ name: '', email: '', subject: '', company: '', message: '' });
      }
    } catch (err: any) {
      setError(err.message || 'Failed to send message. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  return (
    <section id="contact" className="py-20 lg:py-32 bg-gradient-to-b from-white to-sakura-50/30">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-16">
          <h2 className="text-3xl md:text-4xl lg:text-5xl font-bold mb-6 text-gray-900">
            <span className="text-sakura-600">{contact.title}</span>
          </h2>
          <p className="text-lg md:text-xl text-gray-600 max-w-3xl mx-auto">
            {contact.subtitle}
          </p>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-12 lg:gap-16">
          {/* Contact Form */}
          <div className="bg-white rounded-3xl shadow-lg p-8 md:p-10 border border-sakura-100">
            {success && (
              <div className="mb-6 bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded">
                Thank you for your message! We will get back to you soon.
              </div>
            )}
            {error && (
              <div className="mb-6 bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
                {error}
              </div>
            )}
            <form onSubmit={handleSubmit} className="space-y-6">
              <div>
                <label htmlFor="name" className="block text-sm font-medium text-gray-700 mb-2">
                  {contact.form.name} <span className="text-sakura-600">*</span>
                </label>
                <input
                  type="text"
                  id="name"
                  name="name"
                  required
                  value={formData.name}
                  onChange={handleChange}
                  className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-sakura-500 focus:border-transparent transition-all"
                  placeholder={contact.form.name}
                />
              </div>

              <div>
                <label htmlFor="email" className="block text-sm font-medium text-gray-700 mb-2">
                  {contact.form.email} <span className="text-sakura-600">*</span>
                </label>
                <input
                  type="email"
                  id="email"
                  name="email"
                  required
                  value={formData.email}
                  onChange={handleChange}
                  className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-sakura-500 focus:border-transparent transition-all"
                  placeholder={contact.form.email}
                />
              </div>

              <div>
                <label htmlFor="subject" className="block text-sm font-medium text-gray-700 mb-2">
                  Subject
                </label>
                <input
                  type="text"
                  id="subject"
                  name="subject"
                  value={formData.subject}
                  onChange={handleChange}
                  className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-sakura-500 focus:border-transparent transition-all"
                  placeholder="Subject"
                />
              </div>

              <div>
                <label htmlFor="company" className="block text-sm font-medium text-gray-700 mb-2">
                  {contact.form.company}
                </label>
                <input
                  type="text"
                  id="company"
                  name="company"
                  value={formData.company}
                  onChange={handleChange}
                  className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-sakura-500 focus:border-transparent transition-all"
                  placeholder={contact.form.company}
                />
              </div>

              <div>
                <label htmlFor="message" className="block text-sm font-medium text-gray-700 mb-2">
                  {contact.form.message} <span className="text-sakura-600">*</span>
                </label>
                <textarea
                  id="message"
                  name="message"
                  required
                  rows={6}
                  value={formData.message}
                  onChange={handleChange}
                  className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-sakura-500 focus:border-transparent transition-all resize-none"
                  placeholder={contact.form.message}
                />
              </div>

              <button
                type="submit"
                disabled={loading}
                className="w-full px-8 py-4 bg-gradient-to-r from-sakura-500 to-teal-500 text-white rounded-full font-semibold text-lg hover:shadow-2xl hover:scale-105 transition-all duration-300 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {loading ? 'Sending...' : contact.form.submit}
              </button>
            </form>
          </div>

          {/* Contact Information - can be loaded from API */}
          <div className="flex flex-col justify-center space-y-8">
            {/* This can fetch from apiClient.getContactInfo() */}
            <div className="bg-gradient-to-br from-teal-50 to-sakura-50 rounded-3xl shadow-lg p-8 md:p-10 border border-teal-100">
              <h3 className="text-2xl font-bold text-gray-900 mb-6">
                {contact.title}
              </h3>
              {/* Contact info from API */}
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

