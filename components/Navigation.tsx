'use client';

import { useState, useEffect } from 'react';
import Link from 'next/link';
import Image from 'next/image';
import { locales, type Locale } from '@/i18n/config';
import { useLocale } from '@/contexts/LocaleContext';

export default function Navigation() {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [scrolled, setScrolled] = useState(false);
  const { locale, setLocale, t } = useLocale();

  useEffect(() => {
    const handleScroll = () => {
      setScrolled(window.scrollY > 20);
    };
    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []);

  const navItems = [
    { key: 'nav.home', href: `/${locale}#home` },
    { key: 'nav.philosophy', href: `/${locale}#philosophy` },
    { key: 'nav.mission', href: `/${locale}#mission` },
    { key: 'nav.vision', href: `/${locale}#vision` },
    { key: 'nav.values', href: `/${locale}#values` },
    { key: 'nav.domains', href: `/${locale}#domains` },
    { key: 'nav.about', href: `/${locale}#about` },
    { key: 'nav.contact', href: `/${locale}#contact` },
  ];

  return (
    <header>
      <nav className={`fixed top-0 left-0 right-0 z-50 transition-all duration-300 ${
        scrolled 
          ? 'bg-white/98 backdrop-blur-md shadow-lg' 
          : 'bg-white/95 backdrop-blur-sm shadow-sm'
      }`}>
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center h-16 lg:h-20">
            {/* Logo */}
            <Link 
              href={`/${locale}#home`} 
              className="flex items-center hover:opacity-80 transition-opacity group"
              aria-label="G-Hero Company Home"
            >
              <div className="h-8 lg:h-10 flex items-center">
                <Image
                  src="/assets/images/logo.webp"
                  alt="G-Hero Company Logo"
                  width={120}
                  height={40}
                  className="h-8 lg:h-10 w-auto"
                  style={{ objectFit: 'contain' }}
                />
              </div>
            </Link>

            {/* Desktop Navigation */}
            <div className="hidden lg:flex items-center space-x-1">
              {navItems.slice(0, 5).map((item) => (
                <Link
                  key={item.key}
                  href={item.href}
                  className="px-4 py-2 text-gray-700 hover:text-sakura-600 transition-colors font-medium text-sm rounded-md hover:bg-gray-50"
                >
                  {t(item.key)}
                </Link>
              ))}
            </div>

            {/* Language Switcher & CTA */}
            <div className="flex items-center space-x-3 lg:space-x-4">
              {/* Language Switcher */}
              <div className="flex items-center space-x-1 bg-gray-100 rounded-lg p-1 border border-gray-200">
                {locales.map((loc) => (
                  <button
                    key={loc}
                    onClick={() => setLocale(loc)}
                    className={`px-3 py-1.5 rounded-md text-xs font-semibold transition-all ${
                      locale === loc
                        ? 'bg-white text-sakura-600 shadow-sm'
                        : 'text-gray-600 hover:text-gray-900'
                    }`}
                    aria-label={`Switch to ${loc.toUpperCase()} language`}
                  >
                    {loc.toUpperCase()}
                  </button>
                ))}
              </div>

              {/* Contact CTA */}
              <Link
                href={`/${locale}#contact`}
                className="hidden lg:block px-6 py-2.5 bg-gradient-to-r from-sakura-500 to-sakura-600 text-white rounded-lg font-semibold hover:shadow-lg hover:scale-105 transition-all text-sm"
              >
                {t('nav.contact')}
              </Link>

              {/* Mobile Menu Button */}
              <button
                onClick={() => setIsMenuOpen(!isMenuOpen)}
                className="lg:hidden p-2 text-gray-700 hover:text-sakura-600 transition-colors rounded-md hover:bg-gray-100"
                aria-label="Toggle menu"
                aria-expanded={isMenuOpen}
              >
                <svg
                  className="w-6 h-6"
                  fill="none"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  {isMenuOpen ? (
                    <path d="M6 18L18 6M6 6l12 12" />
                  ) : (
                    <path d="M4 6h16M4 12h16M4 18h16" />
                  )}
                </svg>
              </button>
            </div>
          </div>

          {/* Mobile Menu */}
          {isMenuOpen && (
            <div className="lg:hidden py-4 border-t border-gray-200 animate-fadeIn">
              <div className="flex flex-col space-y-1">
                {navItems.map((item) => (
                  <Link
                    key={item.key}
                    href={item.href}
                    onClick={() => setIsMenuOpen(false)}
                    className="px-4 py-3 text-gray-700 hover:bg-sakura-50 hover:text-sakura-600 rounded-md transition-colors font-medium"
                  >
                    {t(item.key)}
                  </Link>
                ))}
                <Link
                  href={`/${locale}#contact`}
                  onClick={() => setIsMenuOpen(false)}
                  className="mx-4 mt-2 px-4 py-3 bg-gradient-to-r from-sakura-500 to-sakura-600 text-white rounded-lg font-semibold text-center"
                >
                  {t('nav.contact')}
                </Link>
              </div>
            </div>
          )}
        </div>
      </nav>
    </header>
  );
}

