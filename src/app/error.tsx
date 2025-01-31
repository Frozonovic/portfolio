'use client'

import { useEffect } from 'react'
import Link from 'next/link'

interface ErrorPageProps {
    error: Error
    reset: () => void
}

const ErrorPage: React.FC<ErrorPageProps> = ({ error, reset }) => {
    useEffect(() => {
        // Log the error to an error reporting service, if necessary
        console.error('Error:', error)
    }, [error])

    return (
        <div className="flex flex-col items-center justify-center min-h-screen">
            <h1 className="text-4xl font-bold mb-4">Something Went Wrong</h1>
            <p className="text-lg mb-6">We encountered an unexpected error. Please try again later.</p>
            <div className="flex space-x-4">
                <button
                    onClick={reset}
                    className="px-6 py-3 rounded shadow-lg"
                >
                    Try Again
                </button>
                <Link href="/" className="px-6 py-3 bg-gray-200 text-gray-800 rounded shadow-lg hover:bg-gray-300">
                    Home
                </Link>
            </div>
        </div>
    )
}

export default ErrorPage
