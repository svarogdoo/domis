/** @type {import('tailwindcss').Config} */
export default {
  content: ['./src/**/*.{html,js,svelte,ts}'],
  theme: {
    extend: {
      colors:{
        blue:{
          600: '#2563eb'
        },
        domis:{
          dark: '#2F2F2F',
          red: '#B22222',
          white: '#F5F5F5',
          light: '#D8C3A5',
          blue: '#6B7A8F'
        }
      }
    },
  },
  plugins: [],
}

