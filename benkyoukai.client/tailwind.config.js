/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./public/**/*.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  theme: {
    // extend: {
    //   colors: {
    //     "benkyoukai-primary": "#00668A",
    //     "benkyoukai-secondary": "#004E71"
    //   }
    // },
    fontFamily: {
      Roboto: ["Roboto, sans-serif"]
    },
    container: {
      padding: "2rem",
      center: true
    },
    screens: {
      sm: "640px",
      md: "748px"
    }
  },
  plugins: [],
}
