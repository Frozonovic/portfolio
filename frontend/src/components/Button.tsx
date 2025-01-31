interface ButtonProps {
    text: string
    className?: string
    onClick?: () => void // Optional onClick prop
}

const Button: React.FC<ButtonProps> = ({ text, className, onClick }) => {
    return (
        <button
            onClick={onClick}
            className={`px-6 py-3 rounded shadow-lg ${className} focus:outline-none`}
        >
            {text}
        </button>
    )
}

export default Button
