'use client';

import Image from 'next/image';
import { useLocale } from '@/contexts/LocaleContext';

// Professional business icons as SVG components
const BusinessIcon = ({ className }: { className?: string }) => (
  <svg className={className} fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 13.255A23.931 23.931 0 0112 15c-3.183 0-6.22-.62-9-1.745M16 6V4a2 2 0 00-2-2h-4a2 2 0 00-2 2v2m4 6h.01M5 20h14a2 2 0 002-2V8a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
  </svg>
);

const domainIcons = [
  <BusinessIcon key="0" className="w-8 h-8" />,
  <BusinessIcon key="1" className="w-8 h-8" />,
  <BusinessIcon key="2" className="w-8 h-8" />,
  <BusinessIcon key="3" className="w-8 h-8" />,
  <BusinessIcon key="4" className="w-8 h-8" />,
  <BusinessIcon key="5" className="w-8 h-8" />,
  <BusinessIcon key="6" className="w-8 h-8" />,
  <BusinessIcon key="7" className="w-8 h-8" />,
  <BusinessIcon key="8" className="w-8 h-8" />,
  <BusinessIcon key="9" className="w-8 h-8" />,
  <BusinessIcon key="10" className="w-8 h-8" />,
  <BusinessIcon key="11" className="w-8 h-8" />,
  <BusinessIcon key="12" className="w-8 h-8" />,
  <BusinessIcon key="13" className="w-8 h-8" />,
  <BusinessIcon key="14" className="w-8 h-8" />,
];

interface BusinessDomainsProps {
  homeData?: any;
}

export default function BusinessDomains({ homeData }: BusinessDomainsProps) {
  const { getNestedTranslation } = useLocale();
  const domains = getNestedTranslation('domains');

  return (
    <section id="domains" className="py-16 lg:py-24 bg-gradient-to-b from-gray-50 to-white">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-12 lg:mb-16">
          <h2 className="text-3xl md:text-4xl lg:text-5xl font-bold mb-4 text-gray-900">
            <span className="text-sakura-600">{domains.title}</span>
          </h2>
          <p className="text-base md:text-lg lg:text-xl text-gray-600 max-w-3xl mx-auto">
            {domains.subtitle}
          </p>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 lg:gap-8">
          {domains.items.map((domain: string, index: number) => (
            <div
              key={index}
              className="group bg-white rounded-xl shadow-md overflow-hidden border border-gray-200 hover:shadow-xl hover:border-sakura-300 transition-all duration-300"
            >
              {/* Icon Header */}
              <div className="bg-gradient-to-br from-sakura-500 to-teal-500 p-6 flex items-center justify-center">
                <div className="w-16 h-16 bg-white/20 backdrop-blur-sm rounded-xl flex items-center justify-center text-white transform group-hover:scale-110 transition-transform duration-300">
                  {domainIcons[index] || <BusinessIcon className="w-8 h-8" />}
                </div>
              </div>
              {/* Content */}
              <div className="p-6">
                <div className="flex items-center mb-3">
                  <span className="text-sakura-600 font-bold text-sm mr-2 bg-sakura-50 px-2 py-1 rounded">
                    {(index + 1).toString().padStart(2, '0')}
                  </span>
                </div>
                <p className="text-gray-700 leading-relaxed text-sm md:text-base">
                  {domain}
                </p>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

