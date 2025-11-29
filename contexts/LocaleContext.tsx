'use client';

import React, { createContext, useContext, useState, useEffect } from 'react';
import { usePathname, useRouter } from 'next/navigation';
import { locales, type Locale } from '@/i18n/config';
import messages from '@/i18n/messages';

type Messages = typeof messages.en;

interface LocaleContextType {
  locale: Locale;
  setLocale: (locale: Locale) => void;
  t: (key: string) => string;
  getNestedTranslation: (path: string) => any;
}

const LocaleContext = createContext<LocaleContextType | undefined>(undefined);

export function LocaleProvider({ children }: { children: React.ReactNode }) {
  const [locale, setLocaleState] = useState<Locale>('ja');
  const pathname = usePathname();
  const router = useRouter();

  useEffect(() => {
    const pathLocale = pathname.split('/')[1] as Locale;
    if (locales.includes(pathLocale)) {
      setLocaleState(pathLocale);
    }
  }, [pathname]);

  const setLocale = (newLocale: Locale) => {
    const pathWithoutLocale = pathname.replace(`/${locale}`, '') || '/';
    router.push(`/${newLocale}${pathWithoutLocale}`);
    setLocaleState(newLocale);
  };

  const getNestedValue = (obj: any, path: string): any => {
    return path.split('.').reduce((current, key) => current?.[key], obj);
  };

  const t = (key: string): string => {
    const translation = getNestedValue(messages[locale], key);
    return translation || key;
  };

  const getNestedTranslation = (path: string) => {
    return getNestedValue(messages[locale], path);
  };

  return (
    <LocaleContext.Provider value={{ locale, setLocale, t, getNestedTranslation }}>
      {children}
    </LocaleContext.Provider>
  );
}

export function useLocale() {
  const context = useContext(LocaleContext);
  if (context === undefined) {
    throw new Error('useLocale must be used within a LocaleProvider');
  }
  return context;
}

