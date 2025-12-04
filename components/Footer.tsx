'use client';

import Link from 'next/link';
import Image from 'next/image';
import { useLocale } from '@/contexts/LocaleContext';

export default function Footer() {
  const { locale } = useLocale();

  return (
    <footer className="bg-gray-900 text-white">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 lg:py-16">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8 lg:gap-12 mb-8">
          {/* Company Info */}
          <div className="lg:col-span-1">
            <Link href={`/${locale}#home`} className="inline-block mb-4">
              <Image
                src="/assets/images/logo.webp"
                alt="G-Hero Company Logo"
                width={150}
                height={60}
                className="h-10 w-auto brightness-0 invert"
                style={{ objectFit: 'contain' }}
              />
            </Link>
            <p className="text-gray-400 text-sm leading-relaxed mb-4">
              Japanese Quality to the World, Global Potential to Japan
            </p>
            <p className="text-gray-500 text-xs">
              A global IT & solutions bridge connecting Japan and the world.
            </p>
          </div>

          {/* Quick Links */}
          <div>
            <h4 className="font-bold text-base mb-4 text-white">Quick Links</h4>
            <ul className="space-y-3 text-sm">
              <li>
                <Link href={`/${locale}#philosophy`} className="text-gray-400 hover:text-sakura-400 transition-colors">
                  Philosophy
                </Link>
              </li>
              <li>
                <Link href={`/${locale}#mission`} className="text-gray-400 hover:text-sakura-400 transition-colors">
                  Mission
                </Link>
              </li>
              <li>
                <Link href={`/${locale}#vision`} className="text-gray-400 hover:text-sakura-400 transition-colors">
                  Vision
                </Link>
              </li>
              <li>
                <Link href={`/${locale}#domains`} className="text-gray-400 hover:text-sakura-400 transition-colors">
                  Business Domains
                </Link>
              </li>
              <li>
                <Link href={`/${locale}#about`} className="text-gray-400 hover:text-sakura-400 transition-colors">
                  About Us
                </Link>
              </li>
            </ul>
          </div>

          {/* Resources */}
          <div>
            <h4 className="font-bold text-base mb-4 text-white">Resources</h4>
            <ul className="space-y-3 text-sm">
              <li>
                <Link href={`/${locale}#values`} className="text-gray-400 hover:text-sakura-400 transition-colors">
                  Core Values
                </Link>
              </li>
              <li>
                <Link href={`/${locale}#contact`} className="text-gray-400 hover:text-sakura-400 transition-colors">
                  Contact Us
                </Link>
              </li>
              <li>
                <a href="#" className="text-gray-400 hover:text-sakura-400 transition-colors">
                  Privacy Policy
                </a>
              </li>
              <li>
                <a href="#" className="text-gray-400 hover:text-sakura-400 transition-colors">
                  Terms of Service
                </a>
              </li>
            </ul>
          </div>

          {/* Contact Info */}
          <div>
            <h4 className="font-bold text-base mb-4 text-white">Contact</h4>
            <ul className="space-y-3 text-sm text-gray-400">
              <li className="flex items-start space-x-3">
                <svg className="w-5 h-5 text-sakura-400 mt-0.5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                </svg>
                <span>Chiyoda-ku, Tokyo, Japan</span>
              </li>
              <li className="flex items-center space-x-3">
                <svg className="w-5 h-5 text-sakura-400 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z" />
                </svg>
                <a href="tel:+81300000000" className="hover:text-sakura-400 transition-colors">+81-3-0000-0000</a>
              </li>
              <li className="flex items-center space-x-3">
                <svg className="w-5 h-5 text-sakura-400 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
                </svg>
                <a href="mailto:info@g-hero-company.com" className="hover:text-sakura-400 transition-colors">info@g-hero-company.com</a>
              </li>
            </ul>
          </div>
        </div>

        {/* Bottom Bar */}
        <div className="border-t border-gray-800 pt-8 mt-8">
          <div className="flex flex-col md:flex-row justify-between items-center space-y-4 md:space-y-0">
            <p className="text-sm text-gray-400">
              &copy; {new Date().getFullYear()} G-Hero Company. All rights reserved.
            </p>
            <div className="flex space-x-6">
              <a href="#" className="text-gray-400 hover:text-sakura-400 transition-colors text-sm">
                Privacy
              </a>
              <a href="#" className="text-gray-400 hover:text-sakura-400 transition-colors text-sm">
                Terms
              </a>
              <a href="#" className="text-gray-400 hover:text-sakura-400 transition-colors text-sm">
                Cookies
              </a>
            </div>
          </div>
        </div>
      </div>
    </footer>
  );
}

