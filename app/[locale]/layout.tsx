import { locales, type Locale } from "@/i18n/config";
import LocaleLayoutClient from "./layout-client";

export function generateStaticParams() {
  return locales.map((locale) => ({ locale }));
}

export default async function LocaleLayout({
  children,
  params,
}: {
  children: React.ReactNode;
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  const validLocale = locales.includes(locale as Locale) ? (locale as Locale) : 'ja';
  
  return <LocaleLayoutClient locale={validLocale}>{children}</LocaleLayoutClient>;
}

