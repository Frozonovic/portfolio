'use client'

import Image from 'next/image'
import { useState } from 'react'
import { ProjectCardProps } from 't/ProjectCardProps'
import Link from 'next/link'

const ProjectCard: React.FC<ProjectCardProps> = ({ project }) => {
    const [isModalOpen, setIsModalOpen] = useState(false)

    const openModal = () => setIsModalOpen(true)
    const closeModal = () => setIsModalOpen(false)

    return (
        <>
            {/* Card */}
            <div
                className="border border-gray-300 bg-white p-4 rounded-lg shadow-lg flex flex-col justify-between h-full transition-all duration-500 hover:shadow-xl hover:scale-105 cursor-pointer"
                onClick={openModal}
            >
                <div>
                    <div className="h-48 overflow-hidden rounded-lg mb-4">
                        <Image
                            src={project.image}
                            alt={project.title}
                            className="w-full h-full object-cover"
                        />
                    </div>
                    <h3 className="text-2xl font-semibold text-gray-800 min-h-[3rem] flex items-center">
                        {project.title}
                    </h3>
                    <p className="text-gray-700 mt-2 min-h-[4rem]">
                        {project.desc}
                    </p>
                    <div className="mt-2 min-h-[2.5rem] flex flex-wrap items-start">
                        {project.tech.map((t) => (
                            <span
                                key={t}
                                className="bg-blue-100 text-blue-800 text-sm font-medium rounded-full px-3 py-1 mr-2 mb-1"
                            >
                                {t}
                            </span>
                        ))}
                    </div>
                </div>
                <div className="mt-4">
                    <Link
                        href={project.link}
                        className="inline-block w-full text-center px-5 py-2 bg-blue-600 text-white rounded-lg font-semibold hover:bg-blue-700 transition-colors"
                        target="_blank"
                    >
                        View Project
                    </Link>
                </div>
            </div>

            {/* Modal */}
            {isModalOpen && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
                    <div className="relative bg-white p-6 rounded-lg shadow-lg w-11/12 max-w-3xl">
                        {/* Close Button */}
                        <button
                            onClick={closeModal}
                            className="absolute top-2 right-2 w-8 h-8 bg-white text-gray-600 flex items-center justify-center hover:text-gray-800 hover:bg-gray-300 rounded-full"
                            aria-label="Close"
                        >
                            &times;
                        </button>
                        <div className="h-64 overflow-hidden rounded-lg mb-4">
                            <Image
                                src={project.image}
                                alt={project.title}
                                className="w-full h-full object-cover"
                            />
                        </div>
                        <h3 className="text-2xl font-bold text-gray-800 mb-2">
                            {project.title}
                        </h3>
                        <p className="text-gray-700">{project.desc}</p>
                        <div className="mt-4 flex flex-wrap">
                            {project.tech.map((t) => (
                                <span
                                    key={t}
                                    className="bg-blue-100 text-blue-800 text-sm font-medium rounded-full px-3 py-1 mr-2 mb-2"
                                >
                                    {t}
                                </span>
                            ))}
                        </div>
                        <Link
                            href={project.link}
                            className="inline-block mt-4 px-5 py-2 bg-blue-600 text-white rounded-lg font-semibold hover:bg-blue-700 transition-colors"
                            target="_blank"
                        >
                            View Project
                        </Link>
                    </div>
                </div>
            )}
        </>
    )
}

export default ProjectCard
