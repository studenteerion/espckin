//src/app/ciao/page.tsx
import Link from 'next/link'

export default function Ciao() {
  return (
    <main className="p-6">
      <h2 className="text-2xl text-purple-500">Benvenuto nella pagina Ciao!</h2>
      <Link href="/" className="text-blue-500">
        Vai alla pagina Home
      </Link>
    </main>
  )
}
