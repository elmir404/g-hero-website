// Next.js API route example - or use direct API calls in components
import type { NextApiRequest, NextApiResponse } from 'next';
import apiClient from '../../lib/api-client';

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse
) {
  if (req.method !== 'GET') {
    return res.status(405).json({ message: 'Method not allowed' });
  }

  try {
    const lang = (req.query.lang as string) || 'en';
    const homePage = await apiClient.getHomePage(lang);
    res.status(200).json(homePage);
  } catch (error: any) {
    res.status(500).json({ message: error.message || 'Failed to fetch home page' });
  }
}

