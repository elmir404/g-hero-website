'use client';

import Link from 'next/link';
import { useLocale } from '@/contexts/LocaleContext';

export default function Hero() {
  const { locale, t, getNestedTranslation } = useLocale();
  const hero = getNestedTranslation('hero');

  return (
    <section id="home" className="relative min-h-screen flex items-center justify-center pt-20 lg:pt-32 pb-20 overflow-hidden">
      {/* Background decorative elements */}
      <div className="absolute inset-0 overflow-hidden pointer-events-none">
        <div className="absolute top-20 left-10 w-72 h-72 bg-sakura-200 rounded-full mix-blend-multiply filter blur-xl opacity-30 animate-blob"></div>
        <div className="absolute top-40 right-10 w-72 h-72 bg-teal-200 rounded-full mix-blend-multiply filter blur-xl opacity-30 animate-blob animation-delay-2000"></div>
        <div className="absolute -bottom-8 left-1/2 w-72 h-72 bg-sakura-300 rounded-full mix-blend-multiply filter blur-xl opacity-30 animate-blob animation-delay-4000"></div>
      </div>

      <div className="relative z-10 max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
        <div className="max-w-4xl mx-auto">
          {/* Company Name */}
          <h1 className="text-5xl md:text-6xl lg:text-7xl font-bold mb-6">
            <span className="bg-gradient-to-r from-sakura-600 via-teal-600 to-sakura-600 bg-clip-text text-transparent animate-gradient">
              {hero.companyName}
            </span>
          </h1>

          {/* Tagline */}
          <p className="text-xl md:text-2xl lg:text-3xl font-medium text-gray-800 mb-6 leading-relaxed">
            {hero.tagline}
          </p>

          {/* Description */}
          <p className="text-lg md:text-xl text-gray-600 mb-12 max-w-3xl mx-auto leading-relaxed">
            {hero.description}
          </p>

          {/* CTA Buttons */}
          <div className="flex flex-col sm:flex-row gap-4 justify-center items-center">
            <Link
              href={`/${locale}#contact`}
              className="px-8 py-4 bg-gradient-to-r from-sakura-500 to-sakura-600 text-white rounded-full font-semibold text-lg hover:shadow-2xl hover:scale-105 transition-all duration-300 w-full sm:w-auto"
            >
              {hero.ctaPrimary}
            </Link>
            <Link
              href={`/${locale}#domains`}
              className="px-8 py-4 bg-white text-sakura-600 border-2 border-sakura-500 rounded-full font-semibold text-lg hover:bg-sakura-50 hover:shadow-xl transition-all duration-300 w-full sm:w-auto"
            >
              {hero.ctaSecondary}
            </Link>
          </div>
        </div>

        {/* Decorative illustration placeholder */}
        <div className="mt-16 flex justify-center">
          <div className="relative w-full max-w-4xl h-64 md:h-96">
            <div className="absolute inset-0 bg-gradient-to-br from-sakura-100/50 to-teal-100/50 rounded-3xl backdrop-blur-sm border border-sakura-200/50"></div>
            <div className="absolute inset-0 flex items-center justify-center">
              <svg
                className="w-64 h-64 md:w-96 md:h-96 text-sakura-300/50"
                fill="none"
                viewBox="0 0 400 400"
                xmlns="http://www.w3.org/2000/svg"
              >
                <circle cx="200" cy="200" r="180" stroke="currentColor" strokeWidth="2" fill="none" opacity="0.3" />
                <path
                  d="M200 20 L220 80 L280 100 L260 140 L280 180 L220 200 L200 260 L180 200 L120 180 L140 140 L120 100 L180 80 Z"
                  stroke="currentColor"
                  strokeWidth="2"
                  fill="none"
                  opacity="0.4"
                />
                <circle cx="200" cy="200" r="60" stroke="currentColor" strokeWidth="2" fill="none" opacity="0.5" />
              </svg>
            </div>
          </div>
        </div>
      </div>

      <style jsx>{`
        @keyframes blob {
          0% {
            transform: translate(0px, 0px) scale(1);
          }
          33% {
            transform: translate(30px, -50px) scale(1.1);
          }
          66% {
            transform: translate(-20px, 20px) scale(0.9);
          }
          100% {
            transform: translate(0px, 0px) scale(1);
          }
        }
        .animate-blob {
          animation: blob 7s infinite;
        }
        .animation-delay-2000 {
          animation-delay: 2s;
        }
        .animation-delay-4000 {
          animation-delay: 4s;
        }
        @keyframes gradient {
          0%, 100% {
            background-position: 0% 50%;
          }
          50% {
            background-position: 100% 50%;
          }
        }
        .animate-gradient {
          background-size: 200% 200%;
          animation: gradient 3s ease infinite;
        }
      `}</style>
    </section>
  );
}

