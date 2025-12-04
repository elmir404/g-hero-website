'use client';

import { useEffect, useState } from 'react';
import { useParams } from 'next/navigation';
import apiClient from '@/lib/api-client';
import Hero from "@/components/Hero";
import Philosophy from "@/components/Philosophy";
import Mission from "@/components/Mission";
import Vision from "@/components/Vision";
import CoreValues from "@/components/CoreValues";
import BusinessDomains from "@/components/BusinessDomains";
import About from "@/components/About";
import Contact from "@/components/Contact";
import Footer from "@/components/Footer";

export default function Home() {
  const params = useParams();
  const locale = (params?.locale as string) || 'ja';
  const [homeData, setHomeData] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function fetchHomePage() {
      try {
        setLoading(true);
        // Use locale from URL params (ja, en, az)
        const data = await apiClient.getHomePage(locale);
        setHomeData(data);
        setError(null);
      } catch (err: any) {
        console.error('Failed to fetch home page:', err);
        setError(err.message || 'Failed to load content');
      } finally {
        setLoading(false);
      }
    }

    fetchHomePage();
  }, [locale]);

  // Show loading state only on initial load
  if (loading && !homeData) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-sakura-600 mx-auto mb-4"></div>
          <div className="text-gray-600">Loading...</div>
        </div>
      </div>
    );
  }

  // If there's an error but we have no data, show error message
  // But still try to render with locale translations as fallback
  if (error && !homeData) {
    console.warn('API data unavailable, using locale translations as fallback:', error);
  }

  // Always render the page - components will use locale translations if homeData is null
  return (
    <>
      <Hero homeData={homeData} />
      <Philosophy homeData={homeData} />
      <Mission homeData={homeData} />
      <Vision homeData={homeData} />
      <CoreValues homeData={homeData} />
      <BusinessDomains homeData={homeData} />
      <About homeData={homeData} />
      <Contact homeData={homeData} />
      <Footer />
    </>
  );
}
