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
        <div className="flex flex-col items-center justify-center min-h-screen bg-background text-foreground transition-colors duration-300">
            <h1 className="text-4xl font-bold mb-4">Something Went Wrong</h1>
            <p className="text-lg mb-6">We encountered an unexpected error. Please try again later.</p>
            <div className="flex space-x-4">
                <button
                    onClick={reset}
                    className="px-6 py-3 bg-primary text-white font-semibold rounded-lg hover:bg-opacity-80 transition-all"
                >
                    Try Again
                </button>
                <Link href="/">
                    <button className="px-6 py-3 bg-secondary text-white font-semibold rounded-lg hover:bg-opacity-80 transition-all">
                        Home
                    </button>
                </Link>
            </div>
        </div>
    )
}

export default ErrorPage
