import Link from 'next/link'

const NotFound: React.FC = () => {
    return (
        <div className="flex flex-col items-center justify-center min-h-screen bg-background text-foreground transition-colors duration-300">
            <h1 className="text-6xl font-bold mb-4">404</h1>
            <p className="text-lg mb-6">Oops! The page you&apos;re looking for doesn&apos;t exist.</p>
            <Link href="/" className="rounded shadow-lg">
                <button className="px-6 py-2 bg-primary text-white font-semibold rounded-lg hover:bg-opacity-80 transition-all">
                    Home
                </button>
            </Link>
        </div>
    )
}

export default NotFound
