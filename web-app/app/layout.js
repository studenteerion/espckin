// app/layout.js
import '../styles/globals.css';
import { Rubik } from 'next/font/google';

const rubik = Rubik({
  subsets: ['latin'],
  weight: ['300', '400', '500', '600', '700', '800', '900'],
  variable: '--font-rubik',
});

export const metadata = {
  title: 'Navbar Verticale',
};

export default function RootLayout({ children }) {
  return (
    <html lang="it" className={rubik.className}>
      <body className="">
        <nav className="">
          <h1 className="">EspcKin</h1>
          <ul className="space-y-2">
            <li><a href="/" className="text-gray-800 hover:underline">Home</a></li>
            <li><a href="/about" className="text-gray-800 hover:underline">Chi siamo</a></li>
            <li><a href="/contact" className="text-gray-800 hover:underline">Contatti</a></li>
          </ul>
        </nav>
        <main className="flex-1 p-6 bg-gray-100">{children}</main>
      </body>
    </html>
  );
}
