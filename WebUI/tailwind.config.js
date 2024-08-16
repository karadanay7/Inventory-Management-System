/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ['./**/*.{razor,html,cshtml}'],
    theme: {
        extend: {},
        fontFamily:{
            'main': ['Poppins', 'sans-serif'],
        }

        
    },
  plugins: [
      require('daisyui'),
    ],
  daisyui: {
      themes: [
          "dark",
          "black",
          "night",
          "sunset"
      ],
  },
  
}

