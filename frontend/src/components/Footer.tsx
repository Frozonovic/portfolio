import Link from 'next/link'

const Footer = () => {
    return (
        <footer className='footer py-6'>
            <div className='text-center'>
                <p>&copy; {new Date().getFullYear()} James Lee. All rights reserved. Powered by Next.js and React.</p>
                <div className='flex justify-center space-x-4 mt-4'>
                    <Link
                        href='https://www.linkedin.com/in/james-b-lee'
                        target='_blank'
                        rel='noopener noreferrer'
                        className='footer-link'
                    >
                        LinkedIn
                    </Link>
                    <Link
                        href='mailto:james.b.lee2020@gmail.com'
                        className='footer-link'
                    >
                        Email
                    </Link>
                    <Link
                        href='/resume'
                        target='_blank'
                        rel='noopener noreferrer'
                        className='footer-link'
                    >
                        Resume
                    </Link>
                </div>
            </div>
        </footer>
    )
}

export default Footer
