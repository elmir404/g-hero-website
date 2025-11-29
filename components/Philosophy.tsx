'use client';

import { useLocale } from '@/contexts/LocaleContext';

export default function Philosophy() {
  const { getNestedTranslation } = useLocale();
  const philosophy = getNestedTranslation('philosophy');

  return (
    <section id="philosophy" className="py-20 lg:py-32 bg-gradient-to-b from-white to-sakura-50/30">
      <div className="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="bg-white rounded-3xl shadow-lg p-8 md:p-12 lg:p-16 border border-sakura-100">
          <h2 className="text-3xl md:text-4xl lg:text-5xl font-bold mb-8 text-gray-900">
            <span className="text-sakura-600">{philosophy.title}</span>
          </h2>
          <div className="prose prose-lg max-w-none">
            <p className="text-gray-700 leading-relaxed text-lg md:text-xl whitespace-pre-line">
              {philosophy.content}
            </p>
          </div>
        </div>
      </div>
    </section>
  );
}

