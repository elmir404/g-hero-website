import { locales, type Locale } from '@/i18n/config';
import messages from '@/i18n/messages';

export type Translations = typeof messages.en;

export function getTranslations(locale: Locale): Translations {
  return messages[locale];
}

export { locales };
export type { Locale };

