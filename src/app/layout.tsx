import { Inter } from 'next/font/google';
import type { Metadata } from 'next';
import '@/globals.css';

export const metadata: Metadata = {
  title: 'James Lee - Portfolio',
  description: 'A showcase of projects by James Lee, highlighting skills in React, Next.js, and TypeScript.',
};

const inter = Inter({
  subsets: ['latin'],
  display: 'swap', // Improve font rendering performance
});

export default function LandingLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body className="bg-background text-foreground antialiased min-h-screen flex flex-col">
        {children}
      </body>
    </html>
  );
}
