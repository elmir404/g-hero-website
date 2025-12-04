'use client';

import Image from 'next/image';
import { useLocale } from '@/contexts/LocaleContext';

const visionImages = [
  'https://images.unsplash.com/photo-1454165804606-c3d57bc86b40?ixlib=rb-4.0.3&auto=format&fit=crop&w=2070&q=80',
  'https://images.unsplash.com/photo-1556761175-4b46a572b786?ixlib=rb-4.0.3&auto=format&fit=crop&w=2032&q=80',
  'https://images.unsplash.com/photo-1522071820081-009f0129c71c?ixlib=rb-4.0.3&auto=format&fit=crop&w=2070&q=80',
  'https://images.unsplash.com/photo-1552664730-d307ca884978?ixlib=rb-4.0.3&auto=format&fit=crop&w=2070&q=80',
];

interface VisionProps {
  homeData?: any;
}

export default function Vision({ homeData }: VisionProps) {
  const { getNestedTranslation } = useLocale();
  const vision = getNestedTranslation('vision');

  return (
    <section id="vision" className="py-16 lg:py-24 bg-white">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-12 lg:mb-16">
          <h2 className="text-3xl md:text-4xl lg:text-5xl font-bold mb-4 text-gray-900">
            <span className="text-sakura-600">{vision.title}</span>
          </h2>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 lg:gap-8">
          {vision.items.map((item: string, index: number) => (
            <div
              key={index}
              className="group bg-white rounded-2xl shadow-md overflow-hidden border border-sakura-100 hover:shadow-xl hover:border-sakura-300 transition-all duration-300"
            >
              {/* Image */}
              <div className="relative w-full h-48 overflow-hidden">
                <Image
                  src={visionImages[index % visionImages.length]}
                  alt={`Vision goal ${index + 1}: ${item.substring(0, 50)}`}
                  fill
                  className="object-cover group-hover:scale-110 transition-transform duration-500"
                  quality={90}
                />
                <div className="absolute inset-0 bg-gradient-to-t from-sakura-600/40 to-transparent"></div>
                <div className="absolute top-4 left-4">
                  <div className="w-12 h-12 bg-white/90 backdrop-blur-sm rounded-full flex items-center justify-center text-sakura-600 font-bold text-lg shadow-lg">
                    {index + 1}
                  </div>
                </div>
              </div>
              {/* Content */}
              <div className="p-6 md:p-8">
                <p className="text-gray-700 leading-relaxed text-base md:text-lg">
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

