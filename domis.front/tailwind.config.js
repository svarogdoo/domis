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
      },
      width: {
        '100': '25rem', 
        '104': '26rem',  
        '108': '27rem',  
        '112': '28rem',  
        '116': '29rem',  
        '120': '30rem',  
        '124': '31rem',  
        '128': '32rem',  
      },
    },
  },
  plugins: [],
}

