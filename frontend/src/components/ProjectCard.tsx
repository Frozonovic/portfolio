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
                className="section border  p-4 rounded-lg shadow-lg flex flex-col justify-between h-full transition-all duration-500 hover:shadow-xl hover:scale-105 cursor-pointer"
                onClick={openModal}
            >
                <div>
                    <div className="h-48 overflow-hidden rounded-lg mb-4">
                        <Image
                            src={project.image}
                            alt={project.name}
                            width={1000}
                            height={1000}
                            className="w-full h-full object-cover"
                        />
                    </div>
                    <h3 className="section-header text-2xl font-semibold min-h-[3rem] flex items-center">
                        {project.name}
                    </h3>
                    <p className="section-content mt-2 min-h-[4rem]">
                        {project.description}
                    </p>
                    <div className="mt-2 min-h-[2.5rem] flex flex-wrap items-start">
                        {project.languages.map((t) => (
                            <span
                                key={t}
                                className="text-sm bg-gray-400 font-medium rounded-full px-3 py-1 mr-2 mb-2"
                            >
                                {t}
                            </span>
                        ))}
                    </div>
                </div>
                <div className="mt-4">
                    <Link
                        href={project.svn_url}
                        className="bg-blue-800 inline-block mt-4 px-5 py-2 rounded-lg font-semibold transition-colors"
                        target="_blank"
                        onClick={(e) => e.stopPropagation()}
                    >
                        View Project
                    </Link>
                </div>
            </div>

            {/* Modal */}
            {isModalOpen && (
                <div className="fixed inset-0 bg-opacity-50 flex items-center justify-center z-50">
                    <div className="section relative p-6 rounded-lg shadow-lg w-11/12 max-w-3xl">
                        {/* Close Button */}
                        <button
                            onClick={closeModal}
                            className="absolute top-2 right-2 w-8 h-8 flex items-center justify-center rounded-full"
                            aria-label="Close"
                        >
                            &times;
                        </button>
                        <div className="h-64 overflow-hidden rounded-lg mb-4">
                            <Image
                                src={project.image}
                                alt={project.name}
                                width={200}
                                height={200}
                                className="w-full h-full object-cover"
                            />
                        </div>
                        <h3 className="text-2xl font-bold mb-2">
                            {project.name}
                        </h3>
                        <p>{project.description}</p>
                        <div className="mt-4 flex flex-wrap">
                            {project.languages.map((t) => (
                                <span
                                    key={t}
                                    className="text-sm bg-gray-400 font-medium rounded-full px-3 py-1 mr-2 mb-2"
                                >
                                    {t}
                                </span>
                            ))}
                        </div>
                        <Link
                            href={project.svn_url}
                            className="bg-blue-800 inline-block mt-4 px-5 py-2 rounded-lg font-semibold transition-colors"
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
