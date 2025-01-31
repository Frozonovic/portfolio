import React from 'react'

const PDFViewer: React.FC = () => {
    const url = '/files/James%20Lee%20Resume.pdf'

    return (
        <div className='flex justify-center items-cente w-full h-screen'>
            <iframe
                src={url}
                className='w-full h-full'
                style={{ border: 'none' }}
                title='James Lee Resume'
            />
        </div>
    )
}

export default PDFViewer