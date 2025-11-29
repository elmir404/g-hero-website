'use client';

import { useEffect } from 'react';
import Navigation from "@/components/Navigation";
import { LocaleProvider } from "@/contexts/LocaleContext";
import type { Locale } from "@/i18n/config";

export default function LocaleLayoutClient({
  children,
  locale,
}: {
  children: React.ReactNode;
  locale: Locale;
}) {
  useEffect(() => {
    document.documentElement.lang = locale;
  }, [locale]);

  return (
    <LocaleProvider>
      <Navigation />
      <main className="min-h-screen">{children}</main>
    </LocaleProvider>
  );
}

