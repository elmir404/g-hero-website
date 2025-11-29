'use client';

import { useLocale } from '@/contexts/LocaleContext';

export default function Vision() {
  const { getNestedTranslation } = useLocale();
  const vision = getNestedTranslation('vision');

  return (
    <section id="vision" className="py-20 lg:py-32 bg-gradient-to-b from-sakura-50/30 to-white">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-16">
          <h2 className="text-3xl md:text-4xl lg:text-5xl font-bold mb-6 text-gray-900">
            <span className="text-sakura-600">{vision.title}</span>
          </h2>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 lg:gap-8">
          {vision.items.map((item: string, index: number) => (
            <div
              key={index}
              className="bg-white rounded-2xl shadow-md p-6 md:p-8 border border-sakura-100 hover:shadow-xl hover:border-sakura-300 transition-all duration-300"
            >
              <div className="flex items-start space-x-4">
                <div className="flex-shrink-0">
                  <div className="w-12 h-12 bg-gradient-to-br from-sakura-500 to-teal-500 rounded-full flex items-center justify-center text-white font-bold text-lg">
                    {index + 1}
                  </div>
                </div>
                <p className="text-gray-700 leading-relaxed text-base md:text-lg flex-1">
                  {item}
                </p>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

