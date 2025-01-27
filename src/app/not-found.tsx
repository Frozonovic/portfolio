import Link from 'next/link'
import Button from 'c/Button'

const NotFound: React.FC = () => {
    return (
        <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100 text-gray-800">
            <h1 className="text-6xl font-bold mb-4">404</h1>
            <p className="text-lg mb-6">Oops! The page you're looking for doesn't exist.</p>
            <Link href="/" className="bg-blue-600 text-white rounded shadow-lg hover:bg-blue-700">
                <Button text='Home' />
            </Link>
        </div>
    )
}

export default NotFound