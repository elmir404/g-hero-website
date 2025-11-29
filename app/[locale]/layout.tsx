import { locales } from "@/i18n/config";
import LocaleLayoutClient from "./layout-client";

export function generateStaticParams() {
  return locales.map((locale) => ({ locale }));
}

export default function LocaleLayout({
  children,
  params: { locale },
}: {
  children: React.ReactNode;
  params: { locale: any };
}) {
  return <LocaleLayoutClient locale={locale}>{children}</LocaleLayoutClient>;
}

