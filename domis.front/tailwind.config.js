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
          primary: '#B22222',
          light: '#F5F5F5',
          secondary: '#D8C3A5',
          accent: '#6B7A8F'
        }
      }
    },
  },
  plugins: [],
}

