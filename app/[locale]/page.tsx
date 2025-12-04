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

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-gray-600">Loading...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-red-600">Error: {error}</div>
      </div>
    );
  }

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
