import Image from 'next/image'
import { Project } from 't/Project'
import ProjectCard from 'c/ProjectCard'
import Footer from 'c/Footer'

const projects: Project[] = [
  {
    id: '1',
    title: 'Any Sort',
    desc: 'C++ program parallelizing the selection sort algorithm via threads',
    tech: ['C++', 'Threads'],
    image: '/images/anysort.png',
    link: 'https://www.github.com/Frozonovic/AnySort',
  },
  {
    id: '2',
    title: 'Array List',
    desc: 'Java program modeling a custom implementation of the built-in ArrayList class',
    tech: ['Java', 'Data Structures'],
    image: '/images/arraylist.png',
    link: 'https://www.github.com/Frozonovic/ArrayList',
  },
  {
    id: '3',
    title: 'By Reference',
    desc: 'Small C++ program illustrating how references and pointers work',
    tech: ['C++', 'References', 'Pointers'],
    image: '/images/byref.png',
    link: 'https://www.github.com/Frozonovic/by_ref',
  },
  {
    id: '4',
    title: 'Chess',
    desc: 'C++ program for playing a game of chess',
    tech: ['C++'],
    image: '/images/chess.png',
    link: 'https://www.github.com/Frozonovic/Chess',
  },
  {
    id: '5',
    title: 'Deque',
    desc: 'Java program modeling a custom implementation of the built-in Deque class',
    tech: ['Java', 'Data Structures'],
    image: '/images/deque.png',
    link: 'https://www.github.com/Frozonovic/Deque',
  },
  {
    id: '6',
    title: 'Directed Graph',
    desc: 'Java program modeling a custom implementation of the built-in Graph class',
    tech: ['Java', 'Data Structures'],
    image: '/images/directedgraph.png',
    link: 'https://www.github.com/Frozonovic/DirectedGraph',
  },
  {
    id: '7',
    title: 'DU',
    desc: 'C program modeling a custom implementation of the built-in "du" shell command',
    tech: ['C'],
    image: '/images/du.png',
    link: 'https://www.github.com/Frozonovic/du',
  },
  {
    id: '8',
    title: 'Dynamic Array',
    desc: 'C++ program illustrating how dynamic allocation works',
    tech: ['C++'],
    image: '/images/dynamicarray.png',
    link: 'https://www.github.com/Frozonovic/dyn_array',
  },
  {
    id: '9',
    title: 'Fortran Statistics',
    desc: 'Fortran program that generates statistics from a file provided via command line',
    tech: ['Fortran'],
    image: '/images/fortranstatistics.png',
    link: 'https://www.github.com/Frozonovic/fortran-statistics',
  },
  {
    id: '10',
    title: 'Generative Grammar',
    desc: 'Python program that generates valid sentences from a grammar file provided via command line',
    tech: ['Python'],
    image: '/images/generativegrammar.png',
    link: 'https://www.github.com/Frozonovic/generative-grammar',
  },
  {
    id: '11',
    title: 'Graph',
    desc: 'Scala program implementing various graphing/pathing algorithms',
    tech: ['Scala', '2opt', 'Branch-Bound', 'Dijkstra', 'Genetic', 'Held-Karp', 'MST'],
    image: '/images/graph.png',
    link: 'https://www.github.com/Frozonovic/graph',
  },
  {
    id: '12',
    title: 'Haskell Statistics',
    desc: 'Haskell program that generates statistics from a file provided via command line',
    tech: ['Haskell'],
    image: '/images/haskellstatistics.png',
    link: 'https://www.github.com/Frozonovic/haskell-statistics',
  },
  {
    id: '13',
    title: 'Linked List',
    desc: 'Java program modeling a custom implementation of the built-in LinkedList class',
    tech: ['Java', 'Data Structures'],
    image: '/images/linkedlist.png',
    link: 'https://www.github.com/Frozonovic/LinkedList',
  },
  {
    id: '14',
    title: 'Minimum',
    desc: 'Scala program that finds the minimum value within a series of values',
    tech: ['Scala'],
    image: '/images/minimum.png',
    link: 'https://www.github.com/Frozonovic/min',
  },
  {
    id: '15',
    title: 'Monte Carlo Pi',
    desc: 'Sequential and parallel C++ program that generates an approximation of pi',
    tech: ['C++', 'Threads'],
    image: '/images/montecarlopi.png',
    link: 'https://www.github.com/Frozonovic/montecarlopi',
  },
  {
    id: '16',
    title: 'Phone Number',
    desc: 'Java program modeling a custom phone number class adhering to the North American Numbering Plan',
    tech: ['Java', 'Data Structures'],
    image: '/images/phonenumber.png',
    link: 'https://www.github.com/Frozonovic/PhoneNumber',
  },
  {
    id: '17',
    title: 'Predator Prey',
    desc: 'C++ program implementing a sequential and parallel version of the classic predator-prey simulation',
    tech: ['C++', 'Threads'],
    image: '/images/predatorprey.png',
    link: 'https://www.github.com/Frozonovic/PredatorPrey',
  },
  {
    id: '18',
    title: 'Priority Queue',
    desc: 'Java program modeling a custom implementation of the built-in PriorityQueue class',
    tech: ['Java', 'Data Structures'],
    image: '/images/priorityqueue.png',
    link: 'https://www.github.com/Frozonovic/PriorityQueue',
  },
  {
    id: '19',
    title: 'Queue',
    desc: 'Java program modeling a custom implementation of the built-in Queue class',
    tech: ['Java', 'Data Structures'],
    image: '/images/queue.png',
    link: 'https://www.github.com/Frozonovic/Queue',
  },
  {
    id: '20',
    title: 'Shell',
    desc: 'C program implementing a basic shell simulation',
    tech: ['C'],
    image: '/images/shell.png',
    link: 'https://www.github.com/Frozonovic/Shell',
  },
  {
    id: '21',
    title: 'Stack',
    desc: 'Java program modeling a custom implementation of the built-in Stack class',
    tech: ['Java', 'Data Structures'],
    image: '/images/stack.png',
    link: 'https://www.github.com/Frozonovic/Stack',
  },
  {
    id: '22',
    title: 'Stack Calculator',
    desc: 'Java program implementing a stack-based command-line calculator program',
    tech: ['Java'],
    image: '/images/stackcalculator.png',
    link: 'https://www.github.com/Frozonovic/StackCalc',
  },
]

export default function ProjectsPage() {
  return (
    <main className='flex-grow'>
      {/* Profile Header */}
      <section className='py-12 bg-secondary text-white'>
        <div className='container mx-auto max-w-7xl flex flex-col lg:flex-row items-center gap-12 px-6'>
          {/* Profile Picture */}
          <div className='w-48 h-48'>
            <Image
              src='/images/profile.jpg'
              alt='Profile'
              className='w-full h-full rounded-full object-cover shadow-lg'
            />
          </div>

          {/* About Me */}
          <div className='lg:w-2/3 text-center lg:text-left'>
            <h1 className='text-4xl font-bold mb-4'>Hello, I&apos;m James Lee</h1>
            <p className='text-lg leading-relaxed'>
              I&apos;m a passionate software developer with expertise in React, TypeScript, and Next.js.
              My projects reflect my dedication to building clean, efficient, and user-focused solutions.
              Outside of coding, I enjoy staying curious about the latest tech trends and exploring creative side projects.
            </p>
          </div>
        </div>
      </section>

      {/* Project Grid */}
      <section className='py-12 bg-gray-50'>
        <div className='container mx-auto max-w-7xl px-6'>
          <h2 className='text-3xl font-bold text-center mb-8 text-gray-800'>My Projects</h2>
          <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8'>
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
