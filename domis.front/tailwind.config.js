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
      spacing: {
        1: "0.25rem", // 4px
        2: "0.5rem", // 8px
        3: "0.75rem", // 12px
        4: "1rem", // 16px
        5: "1.25rem", // 20px
        6: "1.5rem", // 24px
        7: "1.75rem", // 28px
        8: "2rem", // 32px
        9: "2.25rem", // 36px
        10: "2.5rem", // 40px
        11: "2.75rem", // 44px (Missing in default Tailwind)
        12: "3rem", // 48px
        13: "3.25rem", // 52px (Missing)
        14: "3.5rem", // 56px (Missing)
        15: "3.75rem", // 60px (Missing)
        16: "4rem", // 64px
        17: "4.25rem", // 68px (Missing)
        18: "4.5rem", // 72px (Missing)
        19: "4.75rem", // 76px (Missing)
        20: "5rem", // 80px
      },
    },
  },
  plugins: [],
}

