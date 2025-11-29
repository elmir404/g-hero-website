'use client';

import Link from 'next/link';
import { useLocale } from '@/contexts/LocaleContext';

export default function Footer() {
  const { locale } = useLocale();

  return (
    <footer className="bg-gray-900 text-white py-12 lg:py-16">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8 lg:gap-12 mb-8">
          <div>
            <h3 className="text-2xl font-bold mb-4 bg-gradient-to-r from-sakura-400 to-teal-400 bg-clip-text text-transparent">
              G-Hero Company
            </h3>
            <p className="text-gray-400 text-sm">
              Japanese Quality to the World, Global Potential to Japan
            </p>
          </div>
          <div>
            <h4 className="font-semibold mb-4">Quick Links</h4>
            <ul className="space-y-2 text-sm">
              <li>
                <Link href={`/${locale}#philosophy`} className="text-gray-400 hover:text-sakura-400 transition-colors">
                  Philosophy
                </Link>
              </li>
              <li>
                <Link href={`/${locale}#domains`} className="text-gray-400 hover:text-sakura-400 transition-colors">
                  Services
                </Link>
              </li>
              <li>
                <Link href={`/${locale}#about`} className="text-gray-400 hover:text-sakura-400 transition-colors">
                  About
                </Link>
              </li>
              <li>
                <Link href={`/${locale}#contact`} className="text-gray-400 hover:text-sakura-400 transition-colors">
                  Contact
                </Link>
              </li>
            </ul>
          </div>
          <div>
            <h4 className="font-semibold mb-4">Contact</h4>
            <ul className="space-y-2 text-sm text-gray-400">
              <li>Chiyoda-ku, Tokyo, Japan</li>
              <li>+81-3-0000-0000</li>
              <li>info@g-hero-company.com</li>
            </ul>
          </div>
        </div>
        <div className="border-t border-gray-800 pt-8 text-center text-sm text-gray-400">
          <p>&copy; {new Date().getFullYear()} G-Hero Company. All rights reserved.</p>
        </div>
      </div>
    </footer>
  );
}

