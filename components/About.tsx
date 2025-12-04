'use client';

import Image from 'next/image';
import { useLocale } from '@/contexts/LocaleContext';

interface AboutProps {
  homeData?: any;
}

export default function About({ homeData }: AboutProps) {
  const { getNestedTranslation } = useLocale();
  const about = getNestedTranslation('about');

  return (
    <section id="about" className="py-16 lg:py-24 bg-gradient-to-b from-white to-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 lg:gap-12 items-center">
          {/* Image Column */}
          <div className="relative order-2 lg:order-1">
            <div className="relative w-full h-64 md:h-80 lg:h-96 rounded-2xl overflow-hidden shadow-xl">
              <Image
                src="https://images.unsplash.com/photo-1556761175-5973dc0f32e7?ixlib=rb-4.0.3&auto=format&fit=crop&w=2032&q=80"
                alt="About G-Hero Company - professional business team and office environment"
                fill
                className="object-cover"
                quality={90}
              />
              <div className="absolute inset-0 bg-gradient-to-t from-teal-600/20 to-transparent"></div>
            </div>
            {/* Decorative accent */}
            <div className="absolute -bottom-4 -left-4 w-32 h-32 bg-sakura-500 rounded-2xl opacity-20 -z-10 hidden lg:block"></div>
          </div>

          {/* Content Column */}
          <div className="order-1 lg:order-2">
            <div className="bg-gradient-to-br from-teal-50 to-white rounded-2xl shadow-lg p-8 md:p-10 lg:p-12 border border-teal-100">
              <h2 className="text-3xl md:text-4xl lg:text-5xl font-bold mb-6 text-gray-900">
                <span className="text-teal-600">{about.title}</span>
              </h2>
              <p className="text-gray-700 leading-relaxed text-base md:text-lg lg:text-xl">
                {about.content}
              </p>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

