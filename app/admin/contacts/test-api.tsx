// Test component to debug API calls
// You can temporarily add this to the contacts page to test

'use client';

import { useEffect } from 'react';
import apiClient from '@/lib/api-client';

export default function TestApi() {
  useEffect(() => {
    async function test() {
      console.log('=== Testing Contact Messages API ===');
      
      // Test 1: Check token
      const token = localStorage.getItem('auth_token');
      console.log('Token exists:', !!token);
      console.log('Token (first 20 chars):', token?.substring(0, 20));
      
      // Test 2: Try to get current user
      try {
        const user = await apiClient.getCurrentUser();
        console.log('Current user:', user);
      } catch (err: any) {
        console.error('Get current user failed:', err);
      }
      
      // Test 3: Try to get contact messages
      try {
        const messages = await apiClient.getContactMessages(1, 20);
        console.log('Contact messages:', messages);
        console.log('Messages count:', messages.data?.length || 0);
      } catch (err: any) {
        console.error('Get contact messages failed:', err);
      }
      
      // Test 4: Try unread messages
      try {
        const unread = await apiClient.getUnreadMessages();
        console.log('Unread messages:', unread);
      } catch (err: any) {
        console.error('Get unread messages failed:', err);
      }
    }
    
    test();
  }, []);
  
  return (
    <div className="p-4 bg-yellow-100 border border-yellow-400 rounded mb-4">
      <p className="font-semibold">API Test Component</p>
      <p className="text-sm">Check browser console for test results</p>
    </div>
  );
}

