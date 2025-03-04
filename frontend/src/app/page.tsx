'use server'

import Footer from 'c/Footer'
import GitHubRepo from 't/GitHubRepo'
import Image from 'next/image'
import { Project } from 't/Project'
import ProjectCard from 'c/ProjectCard'

const fetchProjects = async (): Promise<Project[]> => {
  const url: string = process.env.NEXT_PUBLIC_INTERNAL || 'http://localhost'
  const port: string = process.env.NEXT_PUBLIC_PORT || '5000'

  const response = await fetch(`http://${url}:${port}/api/Database`, {
    headers: { Host: 'backend.railway.internal' },
    cache: 'no-store',
  })

  if (!response.ok) {
    const error = await response.text();
    throw new Error(`Failed to fetch data: ${response.statusText} - ${error}`);
  }

  const data = await response.json()

  return data.map((p: GitHubRepo) => ({
    id: p.id,
    name: p.name,
    description: p.description || 'No description available',
    languages: p.languages,
    image: `/images/${p.name.toLowerCase()}.png`,
    svn_url: p.svn_url,
  }))
}

export default async function ProjectsPage() {
  const projects = await fetchProjects();

  return (
    <main className="flex-grow bg-background text-foreground transition-colors duration-300">
      {/* Profile Header */}
      <section className="summary">
        <div className="container mx-auto max-w-7xl flex flex-col lg:flex-row items-center gap-12 px-6">
          {/* Profile Picture */}
          <div className="w-48 h-48">
            <Image
              src="/images/profile.jpg"
              alt="Profile"
              width={200}
              height={200}
              className="w-full h-full rounded-full object-cover shadow-lg"
            />
          </div>

          {/* About Me */}
          <div className="lg:w-2/3 text-center lg:text-left">
            <h1 className="text-4xl font-bold mb-4">Hello, I&apos;m James Lee</h1>
            <p className="text-lg leading-relaxed">
              I&apos;m a software developer exploring the web development side of programming. My projects
              reflect my dedication to building clean, efficient, and user-focused solutions. Outside of coding,
              I enjoy staying curious about new developments in technology and in my free time you can find me
              doing a variety of things including gaming, rock climbing, or playing music.
            </p>
          </div>
        </div>
      </section>

      {/* Project Grid */}
      <section className="py-8">
        <h2 className="section-header text-3xl font-bold flex items-center h-full justify-center mb-8">
          My Projects
        </h2>
        <div className="container mx-auto max-w-7xl px-6">
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
            {projects.map((project) => (
              <ProjectCard key={project.id} project={project} />
            ))}
          </div>
        </div>
      </section>

      <Footer />
    </main>
  )
}
