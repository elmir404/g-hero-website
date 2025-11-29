'use client';

import { useLocale } from '@/contexts/LocaleContext';

const domainIcons = [
  '💼', '🍱', '🛒', '🚚', '💄', '✈️', '🚗', '🏢', '🎓', '💻',
  '🏨', '🏠', '☕', '🍷', '🔗'
];

export default function BusinessDomains() {
  const { getNestedTranslation } = useLocale();
  const domains = getNestedTranslation('domains');

  return (
    <section id="domains" className="py-20 lg:py-32 bg-gradient-to-b from-white to-teal-50/30">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-16">
          <h2 className="text-3xl md:text-4xl lg:text-5xl font-bold mb-6 text-gray-900">
            <span className="text-sakura-600">{domains.title}</span>
          </h2>
          <p className="text-lg md:text-xl text-gray-600 max-w-3xl mx-auto">
            {domains.subtitle}
          </p>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 lg:gap-8">
          {domains.items.map((domain: string, index: number) => (
            <div
              key={index}
              className="bg-white rounded-2xl shadow-md p-6 md:p-8 border border-teal-100 hover:shadow-xl hover:border-sakura-300 transition-all duration-300 group"
            >
              <div className="flex items-start space-x-4">
                <div className="flex-shrink-0 text-4xl transform group-hover:scale-110 transition-transform duration-300">
                  {domainIcons[index]}
                </div>
                <div className="flex-1">
                  <div className="flex items-center mb-2">
                    <span className="text-sakura-600 font-bold text-lg mr-2">{(index + 1).toString().padStart(2, '0')}</span>
                  </div>
                  <p className="text-gray-700 leading-relaxed text-base md:text-lg">
                    {domain}
                  </p>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

