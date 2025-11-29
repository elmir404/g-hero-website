'use client';

import { useLocale } from '@/contexts/LocaleContext';

export default function About() {
  const { getNestedTranslation } = useLocale();
  const about = getNestedTranslation('about');

  return (
    <section id="about" className="py-20 lg:py-32 bg-white">
      <div className="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="bg-gradient-to-br from-sakura-50 to-teal-50 rounded-3xl shadow-lg p-8 md:p-12 lg:p-16 border border-sakura-100">
          <h2 className="text-3xl md:text-4xl lg:text-5xl font-bold mb-8 text-gray-900 text-center">
            <span className="text-teal-600">{about.title}</span>
          </h2>
          <p className="text-gray-700 leading-relaxed text-lg md:text-xl text-center max-w-3xl mx-auto">
            {about.content}
          </p>
        </div>
      </div>
    </section>
  );
}

