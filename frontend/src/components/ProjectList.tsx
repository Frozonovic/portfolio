'use client'

import { Project } from 't/Project'
import ProjectCard from 'c/ProjectCard'

interface Props {
    projects: Project[]
}

const ProjectList: React.FC<Props> = ({ projects }) => {
    return (
        <div className='container mx-auto max-w-7xl px-6'>
            <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8'>
                {projects.map((project) => (
                    <ProjectCard key={project.id} project={project} />
                ))}
            </div>
        </div>
    )
}

export default ProjectList
