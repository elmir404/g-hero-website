'use client';

import { useLocale } from '@/contexts/LocaleContext';

export default function Mission() {
  const { getNestedTranslation } = useLocale();
  const mission = getNestedTranslation('mission');

  return (
    <section id="mission" className="py-20 lg:py-32 bg-white">
      <div className="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-12">
          <h2 className="text-3xl md:text-4xl lg:text-5xl font-bold mb-6 text-gray-900">
            <span className="text-teal-600">{mission.title}</span>
          </h2>
        </div>
        <div className="bg-gradient-to-br from-teal-50 to-sakura-50 rounded-3xl shadow-lg p-8 md:p-12 lg:p-16 border border-teal-100">
          <div className="max-w-3xl mx-auto text-center">
            <p className="text-gray-700 leading-relaxed text-lg md:text-xl font-medium">
              {mission.content}
            </p>
          </div>
        </div>
      </div>
    </section>
  );
}

