type GitHubRepo = {
    id: number
    name: string
    description: string | null
    languages: string[] | null
    svn_url: string
}

export default GitHubRepo