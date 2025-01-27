import Link from 'next/link'

const Footer = () => {
    return (
        <footer className='bg-gray-800 text-white py-6'>
            <div className='text-center'>
                <p>&copy; {new Date().getFullYear()} James Lee. All rights reserved.</p>
                <div className='flex justify-center space-x-4 mt-4'>
                    <Link
                        href='https://www.linkedin.com/in/james-b-lee'
                        target='_blank'
                        rel='noopener noreferrer'
                        className='hover:underline'
                    >
                        LinkedIn
                    </Link>
                    <Link
                        href='mailto:james.b.lee2020@gmail.com'
                        className='hover:underline'
                    >
                        Email
                    </Link>
                    <Link
                        href='/resume'
                        target='_blank'
                        rel='noopener noreferrer'
                        className='hover:underline'
                    >
                        Resume
                    </Link>
                </div>
            </div>
        </footer>
    )
}

export default Footer
