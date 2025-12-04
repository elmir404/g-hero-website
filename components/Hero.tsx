'use client';

import Link from 'next/link';
import Image from 'next/image';
import { useLocale } from '@/contexts/LocaleContext';

interface HeroProps {
  homeData?: any;
}

export default function Hero({ homeData }: HeroProps) {
  const { locale, t, getNestedTranslation } = useLocale();
  const hero = getNestedTranslation('hero');

  return (
    <section id="home" className="relative min-h-screen flex items-center justify-center pt-20 lg:pt-32 pb-20 overflow-hidden bg-gradient-to-br from-gray-50 via-white to-sakura-50/30">
      {/* Professional Business Background Image */}
      <div className="absolute inset-0 z-0">
        <Image
          src="https://images.unsplash.com/photo-1522071820081-009f0129c71c?ixlib=rb-4.0.3&auto=format&fit=crop&w=2070&q=80"
          alt="Professional business team collaboration"
          fill
          className="object-cover opacity-10"
          priority
          quality={90}
        />
        <div className="absolute inset-0 bg-gradient-to-br from-white/80 via-white/60 to-sakura-50/40"></div>
      </div>

      {/* Subtle decorative elements */}
      <div className="absolute inset-0 overflow-hidden pointer-events-none z-0">
        <div className="absolute top-20 left-10 w-72 h-72 bg-sakura-200 rounded-full mix-blend-multiply filter blur-xl opacity-20 animate-blob"></div>
        <div className="absolute top-40 right-10 w-72 h-72 bg-teal-200 rounded-full mix-blend-multiply filter blur-xl opacity-20 animate-blob animation-delay-2000"></div>
        <div className="absolute -bottom-8 left-1/2 w-72 h-72 bg-sakura-300 rounded-full mix-blend-multiply filter blur-xl opacity-20 animate-blob animation-delay-4000"></div>
      </div>

      <div className="relative z-10 max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-12 lg:gap-16 items-center">
          {/* Left Column - Content */}
          <div className="text-center lg:text-left">
            {/* Company Name */}
            <h1 className="text-4xl sm:text-5xl md:text-6xl lg:text-7xl font-bold mb-6 leading-tight">
              <span className="bg-gradient-to-r from-sakura-600 via-teal-600 to-sakura-600 bg-clip-text text-transparent">
                {hero.companyName}
              </span>
            </h1>

            {/* Tagline */}
            <p className="text-xl md:text-2xl lg:text-3xl font-semibold text-gray-900 mb-6 leading-tight">
              {hero.tagline}
            </p>

            {/* Description */}
            <p className="text-base md:text-lg lg:text-xl text-gray-600 mb-8 lg:mb-12 leading-relaxed max-w-2xl mx-auto lg:mx-0">
              {hero.description}
            </p>

            {/* CTA Buttons */}
            <div className="flex flex-col sm:flex-row gap-4 justify-center lg:justify-start">
              <Link
                href={`/${locale}#contact`}
                className="px-8 py-4 bg-gradient-to-r from-sakura-500 to-sakura-600 text-white rounded-lg font-semibold text-base lg:text-lg hover:shadow-2xl hover:scale-105 transition-all duration-300 w-full sm:w-auto text-center"
              >
                {hero.ctaPrimary}
              </Link>
              <Link
                href={`/${locale}#domains`}
                className="px-8 py-4 bg-white text-sakura-600 border-2 border-sakura-500 rounded-lg font-semibold text-base lg:text-lg hover:bg-sakura-50 hover:shadow-xl transition-all duration-300 w-full sm:w-auto text-center"
              >
                {hero.ctaSecondary}
              </Link>
            </div>
          </div>

          {/* Right Column - Professional Business Image */}
          <div className="relative hidden lg:block">
            <div className="relative w-full h-[500px] lg:h-[600px] rounded-2xl overflow-hidden shadow-2xl">
              <Image
                src="https://images.unsplash.com/photo-1552664730-d307ca884978?ixlib=rb-4.0.3&auto=format&fit=crop&w=2070&q=80"
                alt="Modern business office with technology and teamwork"
                fill
                className="object-cover"
                priority
                quality={90}
              />
              <div className="absolute inset-0 bg-gradient-to-t from-sakura-600/20 to-transparent"></div>
            </div>
            {/* Decorative accent */}
            <div className="absolute -bottom-6 -left-6 w-32 h-32 bg-teal-500 rounded-2xl opacity-20 -z-10"></div>
            <div className="absolute -top-6 -right-6 w-24 h-24 bg-sakura-500 rounded-full opacity-20 -z-10"></div>
          </div>
        </div>

        {/* Mobile Image */}
        <div className="mt-12 lg:hidden">
          <div className="relative w-full h-64 rounded-2xl overflow-hidden shadow-xl">
            <Image
              src="https://images.unsplash.com/photo-1552664730-d307ca884978?ixlib=rb-4.0.3&auto=format&fit=crop&w=2070&q=80"
              alt="Modern business office with technology and teamwork"
              fill
              className="object-cover"
              quality={90}
            />
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
      `}</style>
    </section>
  );
}

