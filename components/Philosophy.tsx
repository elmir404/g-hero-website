'use client';

import Image from 'next/image';
import { useLocale } from '@/contexts/LocaleContext';

interface PhilosophyProps {
  homeData?: any;
}

export default function Philosophy({ homeData }: PhilosophyProps) {
  const { getNestedTranslation } = useLocale();
  const philosophyStatic = getNestedTranslation('philosophy');
  
  // Use API data if available, otherwise fallback to static translations
  const philosophy = homeData?.philosophy || {
    title: philosophyStatic?.title || 'Philosophy',
    content: philosophyStatic?.content || 'Content not available'
  };

  return (
    <section id="philosophy" className="py-16 lg:py-24 bg-white">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 lg:gap-12 items-center">
          {/* Image Column */}
          <div className="relative order-2 lg:order-1">
            <div className="relative w-full h-64 md:h-80 lg:h-96 rounded-2xl overflow-hidden shadow-xl">
              <Image
                src="https://images.unsplash.com/photo-1521737604893-d14cc237f11d?ixlib=rb-4.0.3&auto=format&fit=crop&w=2084&q=80"
                alt="Business philosophy and corporate values - team collaboration and strategic planning"
                fill
                className="object-cover"
                quality={90}
              />
              <div className="absolute inset-0 bg-gradient-to-t from-sakura-600/20 to-transparent"></div>
            </div>
            {/* Decorative accent */}
            <div className="absolute -bottom-4 -right-4 w-24 h-24 bg-teal-500 rounded-2xl opacity-20 -z-10 hidden lg:block"></div>
          </div>

          {/* Content Column */}
          <div className="order-1 lg:order-2">
            <div className="bg-gradient-to-br from-sakura-50/50 to-white rounded-2xl p-8 md:p-10 lg:p-12 border border-sakura-100 shadow-lg">
              <h2 className="text-3xl md:text-4xl lg:text-5xl font-bold mb-6 text-gray-900">
                <span className="text-sakura-600">{philosophy.title || philosophyStatic?.title}</span>
              </h2>
              <div className="prose prose-lg max-w-none">
                <p className="text-gray-700 leading-relaxed text-base md:text-lg lg:text-xl whitespace-pre-line">
                  {philosophy.content || philosophyStatic?.content || 'Content not available'}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

