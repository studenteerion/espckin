// src/app/page.tsx
import Link from 'next/link'

export default function Home() {
  return (
    <main className="p-8">
      <h1 className="text-3xl font-bold text-blue-600">
        Ciao da Next.js con TypeScript!
      </h1>
      <Link href="/Ciao" className="text-blue-500">
        Vai alla pagina Ciao
      </Link>
    </main>
  )
}